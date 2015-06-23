namespace Emerson.Common.Entities
{
    using AcyclicVisitor;

    public class DestinationRegValue : BaseObject
    {
        public int ValueId { get; set; }

        /// <summary>
        /// Gets or sets the string that specifies the registry root location
        /// </summary>
        public RegistryRoot RegistryRoot { get; set; }

        /// <summary>
        /// Gets or sets the registry subkey
        /// </summary>
        public string Subkey { get; set; }

        /// <summary>
        /// Gets or sets the registry value name
        /// </summary>
        public string ValueName { get; set; }

        /// <summary>
        /// Gets or sets the registry value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the flags that specify information about the registry key
        /// </summary>
        public DestinationRegValueFlags Flags { get; set; }

        public override void Accept(IVisitor<BaseObject> visitor)
        {
            var ourObjectVisitor = visitor as IVisitor<DestinationRegValue>;
            if (ourObjectVisitor != null)
            {
                ourObjectVisitor.Visit(this);
            }
        }
    }
}