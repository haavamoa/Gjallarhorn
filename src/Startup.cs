using Client.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.DependencyInjection;

namespace Client {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            
            services.AddScoped<INuGetService, NuGetService>();

            services.AddMvc(options => 
            {
                options.InputFormatters.Insert(0, new RawRequestBodyInputFormatter());
            }
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    ConfigFile = "webpack.development.js",
                    HotModuleReplacement = true,
                });
            }

            // Serve all static files 
            app.UseStaticFiles();
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=SpaIndex}/{action=Index}");
                routes.MapRoute(
                    name: "api",
                    template: "{controller=API}/{action=Index}"
                );
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "SpaIndex", action = "Index" });
            });
        }
    }
}
