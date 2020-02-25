using System.Threading.Tasks;
using Gjallarhorn.Client.UWP.Services;

namespace Gjallarhorn.Client.UWP.ViewModels
{
    public class StatusViewModel
    {
        private readonly INuGetService m_nugetService;

        public StatusViewModel()
        {
            m_nugetService = new NuGetService();    
        }

        public async Task Initialize()
        {

            var version = await m_nugetService.GetLatestVersionAsync("lightinject", "https://api.nuget.org/v3/", false);
        }
    }
}