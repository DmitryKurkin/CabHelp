namespace Emerson.Common.Entities
{
    using System;

    /// <summary>
    /// Specifies the supported flag values
    /// </summary>
    [Flags]
    public enum DestinationRegValueFlags
    {
        /// <summary>
        /// If the registry key exists, do not overwrite it. This flag can be used with all other flags in this table
        /// </summary>
        FlgAddRegNoClobber = 0x00000002,

        /// <summary>
        /// The REG_SZ registry data type
        /// </summary>
        FlgAddRegTypeSz = 0x00000000,

        /// <summary>
        /// The REG_MULTI_SZ registry data type. The value field that follows can be a list of strings separated by commas
        /// </summary>
        FlgAddRegTypeMultiSz = 0x00010000,

        /// <summary>
        /// The REG_BINARY registry data type. The value field that follows must be a list of numeric values separated by commas, one byte per field, and must not use the 0x hexadecimal prefix
        /// </summary>
        FlgAddRegTypeBinary = 0x00000001,

        /// <summary>
        /// The REG_DWORD data type. Only the noncompatible format in the Win32 Setup .inf documentation is supported
        /// </summary>
        FlgAddRegTypeDword = 0x00010001
    }
}