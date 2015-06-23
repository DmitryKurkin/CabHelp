namespace Emerson.Common.Sections
{
    using Emerson.Common.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using AcyclicVisitor;

    public class ShortcutListSection : BaseObject
    {
        private static int lastUsedShortcutId = 1;

        private readonly List<Shortcut> _shortcuts = new List<Shortcut>();

        public int SectionId { get; set; }

        public string Name { get; set; }

        public IEnumerable<Shortcut> Shortcuts
        {
            get
            {
                return _shortcuts;
            }
        }

        public override void Accept(IVisitor<BaseObject> visitor)
        {
            var ourSectionVisitor = visitor as IVisitor<ShortcutListSection>;
            if (ourSectionVisitor != null)
            {
                ourSectionVisitor.Visit(this);
            }
        }

        public int AddShortcut(string name, ShortcutType type, string targetPath, string standardDestinationPath)
        {
            var shortcutId = lastUsedShortcutId++;

            _shortcuts.Add(new Shortcut
            {
                ShortcutId = shortcutId,
                Name = name,
                TargetPath = targetPath,
                StandardDestinationPath = standardDestinationPath
            });

            return shortcutId;
        }

        public void RemoveShortcut(int shortcutId)
        {
            var shortcutToRemove = _shortcuts.SingleOrDefault(sc => sc.ShortcutId == shortcutId);

            if (shortcutToRemove != null)
            {
                _shortcuts.Remove(shortcutToRemove);
            }
        }
    }
}