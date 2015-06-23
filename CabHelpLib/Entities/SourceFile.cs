namespace Emerson.Common.Entities
{
    using System;
    using AcyclicVisitor;

    public class SourceFile : BaseObject
    {
        public int FileId { get; set; }

        /// <summary>
        /// Gets or sets the parent directory of this file
        /// </summary>
        public SourceDir ParentDir { get; internal set; }

        /// <summary>
        /// Gets or sets the source filename. Enclose long filenames in double quotation marks
        /// </summary>
        public string FileName { get; set; }

        public override void Accept(IVisitor<BaseObject> visitor)
        {
            var ourObjectVisitor = visitor as IVisitor<SourceFile>;
            if (ourObjectVisitor != null)
            {
                ourObjectVisitor.Visit(this);
            }
        }

        public void Validate()
        {
            if (ParentDir == null)
            {
                throw new InvalidOperationException("The parent directory is null");
            }

            if (string.IsNullOrEmpty(FileName))
            {
                throw new InvalidOperationException("The file name is empty");
            }
        }
    }
}