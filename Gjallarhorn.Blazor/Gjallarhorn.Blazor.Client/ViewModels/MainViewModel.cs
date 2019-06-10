using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gjallarhorn.Blazor.Shared;
using Microsoft.AspNetCore.Components;

namespace Gjallarhorn.Blazor.Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly HttpClient m_httpClient;
        private const string ApiBaseUrl = "api/comparepackage";
        public MainViewModel(HttpClient httpClient)
        {
            m_httpClient = httpClient;
            Packages = new List<PackageViewModel>();
        }

        public List<PackageViewModel> Packages { get; set; }

        public async Task Initialize()
        {
            //TODO: Replace hardcoded packages with user configuration
            var hardCodedPackages = new List<Package>()
            {
                new Package(){Name = "LightInject", SourceA = "https://api.nuget.org/v3/", SourceB = "https://api.nuget.org/v3/" },
                new Package(){Name = "dips.embeddedbrowser.client", SourceA = "https://api.nuget.org/v3/", SourceB = "https://api.nuget.org/v3/" },
                new Package(){Name = "Xamarin.Social", SourceA = "https://api.nuget.org/v3/", SourceB = "https://api.nuget.org/v3/" },
                new Package(){Name = "Xamarin.Forms.Theme.Dark", SourceA = "https://api.nuget.org/v3/", SourceB = "https://api.nuget.org/v3/" },
                new Package(){Name = "Newtonsoft.Json", SourceA = "https://api.nuget.org/v3/", SourceB = "https://api.nuget.org/v3/" }
            };

            var tasks = new List<Task>();
            foreach (var hardCodedPackage in hardCodedPackages)
            {
                    var packageViewModel = new PackageViewModel(hardCodedPackage);
                    Packages.Add(packageViewModel);
                    tasks.Add(ComparePackage(packageViewModel, hardCodedPackage));
            }
            await Task.WhenAll(tasks);
        }

        private async Task ComparePackage(PackageViewModel packageViewModel, Package package)
        {
            packageViewModel.IsFetching = true;
            OnPropertyChanged(nameof(Packages));

            var updatedPackage = await m_httpClient.PostJsonAsync<Package>(ApiBaseUrl + "/comparepackage/", package);

            packageViewModel.UpdatePackage(updatedPackage);
            packageViewModel.IsFetching = false;
            packageViewModel.FetchDate = DateTime.Now;
        }
    }
}