namespace Emerson.Common
{
    using Emerson.Common.Sections;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AcyclicVisitor;

    public class Project : BaseObject
    {
        private static int lastUsedRegValuesSectionId = 1;

        private static int lastUsedShortcutListSectionId = 1;

        private readonly List<DestinationRegValuesSection> _destinationRegValuesSections = new List<DestinationRegValuesSection>();

        private readonly List<ShortcutListSection> _shortcutListSections = new List<ShortcutListSection>();

        public Project()
        {
            StringDefs = new StringsSection();
            SourceDirs = new SourceDirsSection();
            DestinationDirs = new DestinationDirsSection();
        }

        public string CompanyName { get; set; }

        public string AppName { get; set; }

        public string InstallDirectory { get; set; }

        public StringsSection StringDefs { get; private set; }

        public SourceDirsSection SourceDirs { get; private set; }

        public DestinationDirsSection DestinationDirs { get; private set; }

        public IEnumerable<DestinationRegValuesSection> DestinationRegValuesSections
        {
            get { return _destinationRegValuesSections; }
        }

        public IEnumerable<ShortcutListSection> ShortcutListSections
        {
            get { return _shortcutListSections; }
        }

        public override void Accept(IVisitor<BaseObject> visitor)
        {
            var ourObjectVisitor = visitor as IVisitor<Project>;
            if (ourObjectVisitor != null)
            {
                ourObjectVisitor.Visit(this);
            }
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(CompanyName))
            {
                throw new InvalidOperationException("The company name is empty");
            }

            if (string.IsNullOrEmpty(AppName))
            {
                throw new InvalidOperationException("The application name is empty");
            }

            if (string.IsNullOrEmpty(InstallDirectory))
            {
                throw new InvalidOperationException("The install directory is empty");
            }

            foreach (var sd in SourceDirs.Directories)
            {
                foreach (var sf in sd.Files)
                {
                    sf.Validate();
                }
            }

            if (!DestinationDirs.Directories.Any(dd => dd.IsDefault))
            {
                throw new InvalidOperationException("There must be at least one destination directory specified");
            }
        }

        public int AddRegValuesSection(string name, bool isDefault)
        {
            var sectionId = lastUsedRegValuesSectionId++;

            _destinationRegValuesSections.Add(new DestinationRegValuesSection
            {
                SectionId = sectionId,
                Name = name,
                IsDefault = isDefault
            });

            return sectionId;
        }

        public void RemoveRegValuesSection(int sectionId)
        {
            var sectionToRemove = _destinationRegValuesSections.SingleOrDefault(drvs => drvs.SectionId == sectionId);

            if (sectionToRemove != null)
            {
                _destinationRegValuesSections.Remove(sectionToRemove);
            }
        }

        public int AddShortcutListSection(string name)
        {
            var sectionId = lastUsedShortcutListSectionId++;

            _shortcutListSections.Add(new ShortcutListSection
            {
                SectionId = sectionId,
                Name = name
            });

            return sectionId;
        }

        public void RemoveShortcutListSection(int sectionId)
        {
            var sectionToRemove = _shortcutListSections.SingleOrDefault(scls => scls.SectionId == sectionId);

            if (sectionToRemove != null)
            {
                _shortcutListSections.Remove(sectionToRemove);
            }
        }
    }
}