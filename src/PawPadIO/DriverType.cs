using System.ComponentModel;

namespace PawPadIO
{
    public enum DriverType
    {
        /// <summary>
        /// Built in drivers that are often enabled by default.
        /// </summary>
        [Description("Built in drivers that are often enabled by default.")]
        BuiltIn,
        /// <summary>
        /// Drivers that are officially supported by PawPadIO.
        /// </summary>
        [Description("Drivers that are officially supported by PawPadIO.")]
        Offical,
        /// <summary>
        /// 3rd-party drivers that come from officail or external sources.
        /// </summary>
        [Description("3rd-party drivers that come from officail or external sources.")]
        ThirdParty,
        /// <summary>
        /// Development drivers that may not be signed and can be side-loaded.
        /// </summary>
        [Description("Development drivers that may not be signed and can be side-loaded.")]
        Development,
    }
}
