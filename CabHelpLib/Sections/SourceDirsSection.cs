namespace Emerson.Common.Sections
{
    using Emerson.Common.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using AcyclicVisitor;

    /// <summary>
    /// The section is required and describes the name and path of the disk where your application resides
    /// </summary>
    public class SourceDirsSection : BaseObject
    {
        private static int lastUsedDirId = 1;

        private readonly List<SourceDir> _directories = new List<SourceDir>();

        public IEnumerable<SourceDir> Directories
        {
            get
            {
                return _directories;
            }
        }

        public override void Accept(IVisitor<BaseObject> visitor)
        {
            var ourSectionVisitor = visitor as IVisitor<SourceDirsSection>;
            if (ourSectionVisitor != null)
            {
                ourSectionVisitor.Visit(this);
            }
        }

        public int AddDirectory(string path, string comment)
        {
            var dirId = lastUsedDirId++;

            _directories.Add(new SourceDir { DirId = dirId, Path = path, Comment = comment });

            return dirId;
        }

        public void RemoveDirectory(int dirId)
        {
            var dirToRemove = _directories.SingleOrDefault(sd => sd.DirId == dirId);

            if (dirToRemove != null)
            {
                _directories.Remove(dirToRemove);
            }
        }
    }
}