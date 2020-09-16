using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using NuGet.Versioning;

namespace PawPadIO
{
    /// <summary>
    /// Inforamtion about a driver.
    /// </summary>
    public interface IDriverInfo
    {
        /// <summary>
        /// The unique identifier of the driver.
        /// </summary>
        string Id{ get; }

        /// <summary>
        /// The type of the driver as recognised by the PawPadIO. 
        /// </summary>
        /// <remarks>
        /// The driver may be provided by multiple sources, either:
        /// <list type="bullet">
        ///     <item>
        ///         <description>Built-in</description>
        ///     </item>
        ///     <item>
        ///         <description>Bundled with PawPadIO</description>
        ///     </item>
        ///     <item>
        ///         <description>Downloaded from 3rd party sources</description>
        ///     </item>
        ///     <item>
        ///         <description>Or a development build is side-loaded</description>
        ///     </item>
        /// </list>
        /// </remarks>
        DriverType Type { get; }

        string Name { get; }

        /// <summary>
        /// The display name of the driver.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The display description detailing the features of the driver.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The semantic version of the driver that is easily sortable to quickly identify stable and beta releases.
        /// </summary>
        SemanticVersion Version { get; }

        /// <summary>
        /// The organisation that developed this driver.
        /// </summary>
        string Company { get; }

        /// <summary>
        /// A URL to any supporting documentation for the driver.
        /// </summary>
        Uri Documentation { get; }

        string Package { get; }

        /// <summary>
        /// Home page of the driver.
        /// </summary>
        Uri HomePage { get; }
    }
}
