using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Gjallarhorn.Blazor.Client.Resources.Commands;
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
            m_allPackages = Packages;
            ToggleShowLatestCommand = new DelegateCommand(_ => ToggleShowLatest());
        }

        public List<PackageViewModel> Packages { get; set; }
        private bool m_showLatest;
        private List<PackageViewModel> m_allPackages;

        public ICommand ToggleShowLatestCommand { get; }
        public bool ShowLatest  
        {
            get => m_showLatest;
            set => SetProperty(ref m_showLatest, value);
        }

        private void ToggleShowLatest()
        {
            ShowLatest = !ShowLatest;
            Packages = ShowLatest ? m_allPackages.Where(p => p.IsLatest).ToList() : m_allPackages;
        }

        public async Task Initialize()
        {
            //TODO: Replace hardcoded packages with user configuration
            var hardCodedPackages = new List<Package>()
            {
                new Package(){Name = "LightInject", SourceA = "https://api.nuget.org/v3/", SourceB = "https://api.nuget.org/v3/" },
                new Package(){Name = "a.very.very.very.very.very.long", SourceA = "https://api.nuget.org/v3/", SourceB = "https://api.nuget.org/v3/" },
                new Package(){Name = "Xamarin.Social", SourceA = "https://api.nuget.org/v3/", SourceB = "https://api.nuget.org/v3/" },
                new Package(){Name = "Xamarin.Forms.Theme.Dark", SourceA = "https://api.nuget.org/v3/", SourceB = "https://api.nuget.org/v3/" },
                new Package(){Name = "dips-arena-medicalcoding-client", SourceA = "http://dips-nuget/nuget/DIPS-Dev/", SourceB = "http://dips-nuget/nuget/Arena-18.1.0-Choco/" }
            };

            var tasks = new List<Task>();
            foreach (var hardCodedPackage in hardCodedPackages)
            {
                    var packageViewModel = new PackageViewModel(hardCodedPackage);
                    Packages.Add(packageViewModel);
                    tasks.Add(ComparePackage(packageViewModel, hardCodedPackage));
            }
            await Task.WhenAll(tasks);
            m_allPackages = Packages;
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