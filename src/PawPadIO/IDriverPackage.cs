using System;
using System.Collections.Generic;
using System.Text;

using NuGet.Versioning;

namespace PawPadIO
{
    public interface IDriverPackage
    {
        string PackageId { get; }

        string Title { get; }

        string Summary { get; }

        string Description { get; }

        Uri ProjectUrl { get; }

        SemanticVersion Version { get; }

        IReadOnlyCollection<(string PackageId, SemanticVersion Version)> Dependencies { get; }

        string Source { get; }
    }
}