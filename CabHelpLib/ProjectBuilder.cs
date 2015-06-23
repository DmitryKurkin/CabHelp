namespace Emerson.Common
{
    using System;
    using System.Linq;
    using System.Text;
    using Entities;
    using Microsoft.Practices.ObjectBuilder2;
    using Properties;
    using Sections;
    using AcyclicVisitor;

    public class ProjectBuilder : IVisitor<BaseObject>, IVisitor<DestinationDir>, IVisitor<DestinationFile>,
        IVisitor<DestinationRegValue>,
        IVisitor<Shortcut>, IVisitor<SourceDir>, IVisitor<SourceFile>, IVisitor<StringDef>,
        IVisitor<DestinationDirsSection>,
        IVisitor<DestinationRegValuesSection>,
        IVisitor<ShortcutListSection>, IVisitor<SourceDirsSection>, IVisitor<StringsSection>, IVisitor<Project>
    {
        private const string ProviderNameTemplate = "#PROVIDERNAME#";

        private const string AppNameTemplate = "#APPNAME#";

        private const string InstallDirectoryTemplate = "#INSTALLDIR#";

        public static string Build(Project project)
        {
            if (project == null) throw new ArgumentNullException("project");

            var builder = new ProjectBuilder(project);

            return builder.Build();
        }

        private readonly StringBuilder _projectBuilder = new StringBuilder(Resources.CabProjectTemplate);

        private readonly Project _project;

        private ProjectBuilder(Project project)
        {
            _project = project;
        }

        public string Build()
        {
            _project.Accept(this);

            return _projectBuilder.ToString();
        }

        object IVisitor<BaseObject>.Visit(BaseObject to)
        {
            throw new NotSupportedException();
        }

        object IVisitor<DestinationDir>.Visit(DestinationDir to)
        {
            _projectBuilder.AppendFormat("{0} = 0, \"{1}\"", to.SectionName, to.DirectoryName);
            _projectBuilder.AppendLine();

            return this;
        }

        object IVisitor<DestinationFile>.Visit(DestinationFile to)
        {
            if (to.SourceFileName != null)
            {
                _projectBuilder.AppendFormat("\"{0}\", \"{1}\", , 0x{2:X}", to.FileName, to.SourceFileName, to.Flags);
            }
            else
            {
                _projectBuilder.AppendFormat("\"{0}\", , , 0x{1:X}", to.FileName, to.Flags);
            }

            _projectBuilder.AppendLine();

            return this;
        }

        object IVisitor<DestinationRegValue>.Visit(DestinationRegValue to)
        {
            _projectBuilder.AppendFormat("{0}, {1}, {2}, 0x{3:X}, {4}", to.RegistryRoot, to.Subkey, to.ValueName,
                to.Flags, to.Value);
            _projectBuilder.AppendLine();

            return this;
        }

        object IVisitor<Shortcut>.Visit(Shortcut to)
        {
            _projectBuilder.AppendFormat(
                "{0}, {1}, {2}{3} {4}",
                to.Name,
                (int) to.Type,
                to.TargetPath,
                string.IsNullOrWhiteSpace(to.StandardDestinationPath) ? string.Empty : ",",
                string.IsNullOrWhiteSpace(to.StandardDestinationPath) ? string.Empty : to.StandardDestinationPath);
            _projectBuilder.AppendLine();

            return this;
        }

        object IVisitor<SourceDir>.Visit(SourceDir to)
        {
            _projectBuilder.AppendFormat("{0} = , \"{1}\", , \"{2}\"", to.DirId, to.Comment, to.Path);
            _projectBuilder.AppendLine();

            return this;
        }

        object IVisitor<SourceFile>.Visit(SourceFile to)
        {
            _projectBuilder.AppendFormat("\"{0}\" = {1}", to.FileName, to.ParentDir != null ? to.ParentDir.DirId : -1);
            _projectBuilder.AppendLine();

            return this;
        }

        object IVisitor<StringDef>.Visit(StringDef to)
        {
            _projectBuilder.AppendFormat("{0} = \"{1}\"", to.Key, to.Value);
            _projectBuilder.AppendLine();

            return this;
        }

        object IVisitor<DestinationDirsSection>.Visit(DestinationDirsSection to)
        {
            _projectBuilder.Append("[DestinationDirs]");
            _projectBuilder.AppendLine();

            to.Directories.ForEach(dd => dd.Accept(this));

            _projectBuilder.AppendLine();

            return this;
        }

        object IVisitor<DestinationRegValuesSection>.Visit(DestinationRegValuesSection to)
        {
            _projectBuilder.AppendFormat("[{0}]", to.Name);
            _projectBuilder.AppendLine();

            to.RegValues.ForEach(rv => rv.Accept(this));

            _projectBuilder.AppendLine();

            return this;
        }

        object IVisitor<ShortcutListSection>.Visit(ShortcutListSection to)
        {
            if (!to.Shortcuts.Any()) return this;

            _projectBuilder.AppendFormat("[{0}]", to.Name);
            _projectBuilder.AppendLine();

            to.Shortcuts.ForEach(sc => sc.Accept(this));

            _projectBuilder.AppendLine();

            return this;
        }

        object IVisitor<SourceDirsSection>.Visit(SourceDirsSection to)
        {
            _projectBuilder.Append("[SourceDisksNames]");
            _projectBuilder.AppendLine();

            to.Directories.ForEach(sd => sd.Accept(this));

            _projectBuilder.AppendLine();

            return this;
        }

        object IVisitor<StringsSection>.Visit(StringsSection to)
        {
            _projectBuilder.Append("[Strings]");
            _projectBuilder.AppendLine();

            to.Defs.ForEach(sd => sd.Accept(this));

            _projectBuilder.AppendLine();

            return this;
        }

        object IVisitor<Project>.Visit(Project to)
        {
            _projectBuilder.Replace(ProviderNameTemplate, to.CompanyName);
            _projectBuilder.Replace(AppNameTemplate, to.AppName);
            _projectBuilder.Replace(InstallDirectoryTemplate, to.InstallDirectory);

            to.StringDefs.Accept(this);

            AppendDefaultInstallSection();

            to.SourceDirs.Accept(this);

            AppendSourceDisksFilesSection();

            to.DestinationDirs.Accept(this);

            AppendDestinationDirsSections();

            to.DestinationRegValuesSections.ForEach(drvs => drvs.Accept(this));

            to.ShortcutListSections.ForEach(sls => sls.Accept(this));

            return this;
        }

        private void AppendDefaultInstallSection()
        {
            _projectBuilder.Append("[DefaultInstall]");
            _projectBuilder.AppendLine();

            // no check here because it should be already validated
            _projectBuilder.AppendFormat(
                "CopyFiles = {0}",
                string.Join(
                    ", ",
                    _project.DestinationDirs.Directories.Where(dd => dd.IsDefault).Select(dd => dd.SectionName)));

            if (_project.DestinationRegValuesSections.Any(drvs => drvs.IsDefault))
            {
                _projectBuilder.AppendLine();
                _projectBuilder.AppendFormat(
                    "AddReg = {0}",
                    string.Join(
                        ", ",
                        _project.DestinationRegValuesSections.Where(drvs => drvs.IsDefault).Select(drvs => drvs.Name)));
            }

            if (_project.ShortcutListSections.Any())
            {
                _projectBuilder.AppendLine();
                _projectBuilder.AppendFormat(
                    "CEShortcuts = {0}",
                    string.Join(", ", _project.ShortcutListSections.Select(scls => scls.Name)));
            }

            _projectBuilder.AppendLine();
            _projectBuilder.AppendLine();
        }

        private void AppendSourceDisksFilesSection()
        {
            _projectBuilder.Append("[SourceDisksFiles]");
            _projectBuilder.AppendLine();

            _project.SourceDirs.Directories.ForEach(sd => sd.Files.ForEach(sf => sf.Accept(this)));

            _projectBuilder.AppendLine();
        }

        private void AppendDestinationDirsSections()
        {
            foreach (var dd in _project.DestinationDirs.Directories)
            {
                _projectBuilder.AppendFormat("[{0}]", dd.SectionName);
                _projectBuilder.AppendLine();

                dd.Files.ForEach(df => df.Accept(this));

                _projectBuilder.AppendLine();
            }
        }
    }
}