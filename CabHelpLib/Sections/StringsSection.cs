namespace Emerson.Common.Sections
{
    using Emerson.Common.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using AcyclicVisitor;

    /// <summary>
    /// The [Strings] section is optional and defines one or more string keys, which represent strings of printable characters
    /// </summary>
    public class StringsSection : BaseObject
    {
        private static int lastUsedDefId = 1;

        private readonly List<StringDef> _defs = new List<StringDef>();

        public IEnumerable<StringDef> Defs
        {
            get
            {
                return _defs;
            }
        }

        public override void Accept(IVisitor<BaseObject> visitor)
        {
            var ourSectionVisitor = visitor as IVisitor<StringsSection>;
            if (ourSectionVisitor != null)
            {
                ourSectionVisitor.Visit(this);
            }
        }

        public int AddStringDef(string key, string value)
        {
            var defId = lastUsedDefId++;

            _defs.Add(new StringDef { DefId = defId, Key = key, Value = value });

            return defId;
        }

        public void RemoveStringDef(int defId)
        {
            var defToRemove = _defs.SingleOrDefault(d => d.DefId == defId);

            if (defToRemove != null)
            {
                _defs.Remove(defToRemove);
            }
        }
    }
}