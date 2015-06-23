namespace CabHelpWebApplication.Models
{
    using Emerson.Common.Entities;

    public class ShortcutViewModel
    {
        public Shortcut Shortcut { get; set; }

        public int ParentSectionId { get; set; }
    }
}