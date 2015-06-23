namespace Emerson.Common.Entities
{
    using AcyclicVisitor;

    public class StringDef : BaseObject
    {
        public int DefId { get; set; }

        /// <summary>
        /// Gets or sets the key component of the definition
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value component of the definition (a string consisting of letters, digits, or other printable characters)
        /// </summary>
        public string Value { get; set; }

        public override void Accept(IVisitor<BaseObject> visitor)
        {
            var ourObjectVisitor = visitor as IVisitor<StringDef>;
            if (ourObjectVisitor != null)
            {
                ourObjectVisitor.Visit(this);
            }
        }
    }
}