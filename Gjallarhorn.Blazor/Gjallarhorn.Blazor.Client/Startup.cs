using Blazor.Extensions.Storage;
using Gjallarhorn.Blazor.Client.Helpers;
using Gjallarhorn.Blazor.Client.Services;
using Gjallarhorn.Blazor.Client.ViewModels;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Gjallarhorn.Blazor.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStorage();
            services.AddTransient<IUserConfigurationService, UserConfigurationService>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddTransient<IPackageFactory, PackageFactory>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
