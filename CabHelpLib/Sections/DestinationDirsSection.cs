namespace Emerson.Common.Sections
{
    using Emerson.Common.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using AcyclicVisitor;

    /// <summary>
    /// The section is required and describes the names and paths of the destination directories for your application on the target device
    /// </summary>
    public class DestinationDirsSection : BaseObject
    {
        private static int lastUsedDirId = 1;

        private readonly List<DestinationDir> _directories = new List<DestinationDir>();

        public IEnumerable<DestinationDir> Directories
        {
            get
            {
                return _directories;
            }
        }

        public override void Accept(IVisitor<BaseObject> visitor)
        {
            var ourSectionVisitor = visitor as IVisitor<DestinationDirsSection>;
            if (ourSectionVisitor != null)
            {
                ourSectionVisitor.Visit(this);
            }
        }

        public int AddDirectory(string directoryName, bool isDefault, string sectionName)
        {
            var dirId = lastUsedDirId++;

            _directories.Add(
                new DestinationDir { DirId = dirId, DirectoryName = directoryName, IsDefault = isDefault, SectionName = sectionName });

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