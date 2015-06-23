namespace Emerson.Common.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using AcyclicVisitor;

    public class SourceDir : BaseObject
    {
        private static int lastUsedFileId = 1;

        private readonly List<SourceFile> _files = new List<SourceFile>();

        /// <summary>
        /// Gets or sets the source identifier used to specify the source directory in the <see cref="SourceFile"/>
        /// </summary>
        public int DirId { get; set; }

        /// <summary>
        /// Gets or sets the friendly description of the source directory
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the path of the disk where your application resides
        /// </summary>
        public string Path { get; set; }

        public IEnumerable<SourceFile> Files
        {
            get
            {
                return _files;
            }
        }

        public override void Accept(IVisitor<BaseObject> visitor)
        {
            var ourObjectVisitor = visitor as IVisitor<SourceDir>;
            if (ourObjectVisitor != null)
            {
                ourObjectVisitor.Visit(this);
            }
        }

        public int AddFile(string fileName)
        {
            var fileId = lastUsedFileId++;

            _files.Add(new SourceFile { ParentDir = this, FileId = fileId, FileName = fileName });

            return fileId;
        }

        public void RemoveFile(int fileId)
        {
            var fileToRemove = _files.SingleOrDefault(sf => sf.FileId == fileId);

            if (fileToRemove != null)
            {
                _files.Remove(fileToRemove);
            }
        }

        public void LoadFiles(string[] fileNames)
        {
            _files.Clear();

            foreach (var n in fileNames)
            {
                AddFile(n);
            }
        }
    }
}