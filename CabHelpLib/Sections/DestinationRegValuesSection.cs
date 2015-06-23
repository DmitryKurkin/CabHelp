namespace Emerson.Common.Sections
{
    using Emerson.Common.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using AcyclicVisitor;

    /// <summary>
    /// The section is optional and describes the keys and values that the .cab file adds to the target device registry
    /// </summary>
    public class DestinationRegValuesSection : BaseObject
    {
        private static int lastUsedValueId = 1;

        private readonly List<DestinationRegValue> _regValues = new List<DestinationRegValue>();

        public int SectionId { get; set; }

        /// <summary>
        /// Gets or sets the name of this section used in the <see cref="DefaultInstallSection"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this section must be included into the default installation of your application
        /// </summary>
        public bool IsDefault { get; set; }

        public IEnumerable<DestinationRegValue> RegValues
        {
            get
            {
                return _regValues;
            }
        }

        public override void Accept(IVisitor<BaseObject> visitor)
        {
            var ourSectionVisitor = visitor as IVisitor<DestinationRegValuesSection>;
            if (ourSectionVisitor != null)
            {
                ourSectionVisitor.Visit(this);
            }
        }

        public int AddRegValue(
            RegistryRoot registryRoot,
            string subkey,
            string valueName,
            string value,
            DestinationRegValueFlags flags)
        {
            var valueId = lastUsedValueId++;

            _regValues.Add(new DestinationRegValue
            {
                ValueId = valueId,
                RegistryRoot = registryRoot,
                Subkey = subkey,
                ValueName = valueName,
                Value = value,
                Flags = flags
            });

            return valueId;
        }

        public void RemoveRegValue(int valueId)
        {
            var valueToRemove = _regValues.SingleOrDefault(drv => drv.ValueId == valueId);

            if (valueToRemove != null)
            {
                _regValues.Remove(valueToRemove);
            }
        }
    }
}