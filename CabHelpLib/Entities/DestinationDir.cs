namespace Emerson.Common.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using AcyclicVisitor;

    public class DestinationDir : BaseObject
    {
        private static int lastUsedFileId = 1;

        private readonly List<DestinationFile> _files = new List<DestinationFile>();

        public int DirId { get; set; }

        /// <summary>
        /// Gets or sets the destination directory, which uses the format of an absolute device path, a directory macro, or the install directory %InstallDir%
        /// </summary>
        public string DirectoryName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this directory must be included into the default installation of your application
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the destination identifier used to specify the destination directory
        /// </summary>
        public string SectionName { get; set; }

        public IEnumerable<DestinationFile> Files
        {
            get
            {
                return _files;
            }
        }

        public override void Accept(IVisitor<BaseObject> visitor)
        {
            var ourObjectVisitor = visitor as IVisitor<DestinationDir>;
            if (ourObjectVisitor != null)
            {
                ourObjectVisitor.Visit(this);
            }
        }

        public int AddFile(string fileName, string sourceFileName, DestinationFileFlags flags)
        {
            var fileId = lastUsedFileId++;

            _files.Add(
                new DestinationFile { FileId = fileId, FileName = fileName, SourceFileName = sourceFileName, Flags = flags });

            return fileId;
        }

        public void RemoveFile(int fileId)
        {
            var fileToRemove = _files.SingleOrDefault(df => df.FileId == fileId);

            if (fileToRemove != null)
            {
                _files.Remove(fileToRemove);
            }
        }
    }
}