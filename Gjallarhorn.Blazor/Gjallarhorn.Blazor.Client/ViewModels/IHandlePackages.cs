using System.Threading.Tasks;

namespace Gjallarhorn.Blazor.Client.ViewModels
{
    public interface IHandlePackages
    {
        Task RemovePackage(PackageViewModel packageViewModel);
    }
}