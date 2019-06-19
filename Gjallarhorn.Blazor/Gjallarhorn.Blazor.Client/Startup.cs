using Blazor.Extensions.Storage;
using Gjallarhorn.Blazor.Client.Helpers;
using Gjallarhorn.Blazor.Client.Services.Http;
using Gjallarhorn.Blazor.Client.Services.LocalStorage;
using Gjallarhorn.Blazor.Client.Services.UserConfiguration;
using Gjallarhorn.Blazor.Client.ViewModels;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using IHttpClientFactory = Gjallarhorn.Blazor.Client.Services.Http.IHttpClientFactory;

namespace Gjallarhorn.Blazor.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            AddServices(services);
            AddViewModels(services);
        }

        private static void AddViewModels(IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<SettingsViewModel>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddStorage();
            services.AddSingleton<IHttpClient, HttpClientAdapter>();
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            services.AddTransient<ILocalStorage, LocalStorageAdapter>();
            services.AddTransient<IUserConfigurationService, UserConfigurationService>();
            services.AddTransient<IPackageFactory, PackageFactory>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
