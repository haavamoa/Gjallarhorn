using Blazor.Extensions.Storage;
using Gjallarhorn.Client.Helpers;
using Gjallarhorn.Client.Services.Http;
using Gjallarhorn.Client.Services.LocalStorage;
using Gjallarhorn.Client.Services.UserConfiguration;
using Gjallarhorn.Client.ViewModels;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using IHttpClientFactory = Gjallarhorn.Client.Services.Http.IHttpClientFactory;

namespace Gjallarhorn.Client
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
