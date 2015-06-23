namespace Emerson.Common.Entities
{
    using AcyclicVisitor;

    public class DestinationFile : BaseObject
    {
        public int FileId { get; set; }

        /// <summary>
        /// Gets or sets the name of the destination file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the source file (Optional if this is identical to <see cref="FileName"/>)
        /// </summary>
        public string SourceFileName { get; set; }

        /// <summary>
        /// Gets or sets the flags that specify actions to be done while copying files
        /// </summary>
        public DestinationFileFlags Flags { get; set; }

        public override void Accept(IVisitor<BaseObject> visitor)
        {
            var ourObjectVisitor = visitor as IVisitor<DestinationFile>;
            if (ourObjectVisitor != null)
            {
                ourObjectVisitor.Visit(this);
            }
        }
    }
}