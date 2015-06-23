namespace Emerson.Common.Entities
{
    public enum ShortcutType
    {
        /// <summary>
        /// Zero or empty represents a shortcut to a file
        /// </summary>
        ShortcutToFile = 0,

        /// <summary>
        /// Any nonzero numeric value represents a shortcut to a folder
        /// </summary>
        ShortcutToFolder = 1
    }
}