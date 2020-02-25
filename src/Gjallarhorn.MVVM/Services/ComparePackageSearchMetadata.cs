using System.Collections.Generic;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace Gjallarhorn.Client.UWP.Services
{
    public class ComparePackageSearchMetadata : IComparer<IPackageSearchMetadata>, IEqualityComparer<IPackageSearchMetadata>
    {
        public PackageIdentityComparer _comparer { get; set; } = PackageIdentityComparer.Default;

        public int Compare(IPackageSearchMetadata x, IPackageSearchMetadata y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (ReferenceEquals(x, null))
            {
                return -1;
            }

            if (ReferenceEquals(y, null))
            {
                return 1;
            }

            return _comparer.Compare(x.Identity, y.Identity);
        }

        public bool Equals(IPackageSearchMetadata x, IPackageSearchMetadata y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return _comparer.Equals(x.Identity, y.Identity);
        }

        public int GetHashCode(IPackageSearchMetadata obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            return _comparer.GetHashCode(obj.Identity);
        }
    }
}