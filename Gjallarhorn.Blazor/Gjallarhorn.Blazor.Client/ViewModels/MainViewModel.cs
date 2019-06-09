using System.Collections.Generic;
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
                new Package(){Name = "Newtonsoft.Json", SourceA = "https://api.nuget.org/v3/", SourceB = "https://api.nuget.org/v3/" }
            };

            foreach (var package in hardCodedPackages)
            {
                var updatedPackage = await m_httpClient.PostJsonAsync<Package>(ApiBaseUrl+"/comparepackage/", package);
                Packages.Add(new PackageViewModel(updatedPackage));
                OnPropertyChanged(nameof(Packages));
            }
        }
    }
}