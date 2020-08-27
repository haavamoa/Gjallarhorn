using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Gjallarhorn.Server.Data;
using Gjallarhorn.Server.Services;
using Blazored.LocalStorage;
using Gjallarhorn.Server.Services.UserConfiguration;
using Gjallarhorn.Server.Services.LocalStorage;
using Gjallarhorn.Client.Helpers;
using Gjallarhorn.Server.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gjallarhorn.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddBlazoredLocalStorage();
            services.AddScoped<INuGetService, NuGetService>();
            services.AddScoped<IUserConfigurationService, UserConfigurationService>();
            services.AddTransient<ILocalStorage, LocalStorageAdapter>();
            services.AddTransient<IUserConfigurationService, UserConfigurationService>();
            services.AddTransient<IPackageFactory, PackageFactory>();
            services.AddScoped<MainViewModel>();
            services.AddScoped<SettingsViewModel>();


            services.AddRazorPages();
            services.AddServerSideBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
