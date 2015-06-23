namespace Emerson.Common.Entities
{
    using AcyclicVisitor;

    public class Shortcut : BaseObject
    {
        public int ShortcutId { get; set; }

        /// <summary>
        /// String that identifies the shortcut name. It does not require the .lnk extension
        /// </summary>
        public string Name { get; set; }

        public ShortcutType Type { get; set; }

        /// <summary>
        /// String value that specifies the destination file or folder.
        /// For a file, use the target file name that must be defined in a file copy list.
        /// For a folder, use a file_list_section name defined in the [DestinationDirs] section or the %InstallDir% string
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        /// Optional string value that specifies where the shortcut file is created, using a standard %CEx% path or %InstallDir%.
        /// If no value is specified, the default value that is used is the destination directory listed for the shortcut_list_section from the [DestinationDirs] section
        /// </summary>
        public string StandardDestinationPath { get; set; }

        public override void Accept(IVisitor<BaseObject> visitor)
        {
            var ourObjectVisitor = visitor as IVisitor<Shortcut>;
            if (ourObjectVisitor != null)
            {
                ourObjectVisitor.Visit(this);
            }
        }
    }
}