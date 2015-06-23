namespace Emerson.Common.Entities
{
    using System;

    /// <summary>
    /// Specifies the destination file flags are supported by Windows Embedded Compact
    /// </summary>
    [Flags]
    public enum DestinationFileFlags : uint
    {
        /// <summary>
        /// Warn a user if an attempt is made to skip a file after an error occurs
        /// </summary>
        CopyFlgWarnIfSkip = 0x00000001,

        /// <summary>
        /// Do not allow a user to skip copying a file
        /// </summary>
        CopyFlgNoSkip = 0x00000002,

        /// <summary>
        /// Do not overwrite a file in the destination directory
        /// </summary>
        CopyFlgNoOverwrite = 0x00000010,

        /// <summary>
        /// Copy the source file to the destination directory only if the file is in the destination directory
        /// </summary>
        CopyFlgReplaceOnly = 0x00000400,

        /// <summary>
        /// Do not copy files if the target file is newer
        /// </summary>
        CeCopyFlgNoDateDialog = 0x20000000,

        /// <summary>
        /// Ignore date while overwriting the target file
        /// </summary>
        CeCopyFlgNoDateCheck = 0x40000000,

        /// <summary>
        /// Create a reference when a shared DLL is counted
        /// </summary>
        CeCopyFlgShared = 0x80000000
    }
}