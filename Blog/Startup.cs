using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Blog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddSpaStaticFiles(c => c.RootPath = "Frontend/browser");
            //services.AddSingleton<INodeServices>(p =>
            //{
            //    return NodeServicesFactory.CreateNodeServices(new NodeServicesOptions(services.BuildServiceProvider())
            //    {
            //        ProjectPath = "Frontend/dist/server"
            //    });
            //});
            //services.AddNodeServices();
            services.AddSpaPrerenderer();
            services.AddSpaStaticFiles(c => c.RootPath = "Frontend/dist/browser");
            //services.AddSpaStaticFiles(c => c.RootPath = "Frontend/server");
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Frontend/dist/browser")),
                RequestPath = ""
            });
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(
            //    Path.Combine(Directory.GetCurrentDirectory(), "Frontend/dist/server")),
            //    RequestPath = ""
            //});
            app.UseCookiePolicy();

            //if (env.IsDevelopment())
            //{
            //    app.UseSpa(spa =>
            //    {
            //        spa.Options.SourcePath = "Frontend";
            //        //spa.UseAngularCliServer(npmScript: "start");
            //        spa.UseSpaPrerendering(config =>
            //        {
            //            //AngularCliMiddlewareExtensions.UseAngularCliServer()
            //            config.BootModuleBuilder = new AngularCliBuilder("serve:ssr");
            //            config.BootModulePath = "Frontend/dist/server";
            //        });
            //    });
            //}

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "Frontend";
                spa.UseSpaPrerendering(options =>
                {
                    options.BootModulePath = "Frontend/dist/server/main.js";
                    options.BootModuleBuilder = env.IsDevelopment()
                        ? new AngularCliBuilder(npmScript: "build:ssr")
                            : null;
                    options.ExcludeUrls = new[] { "/sockjs-node" };
                });

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            //if (env.IsDevelopment())
            //{
            //    app.UseWebpackDevMiddleware(new Microsoft.AspNetCore.SpaServices.Webpack.WebpackDevMiddlewareOptions() {

            //    });
            //}

            //app.UseWhen(x => !x.Request.Path.Value.StartsWith("/api"), builder =>
            //{
            //    builder.UseSpa(spa => {

            //    });
            //});

            //app.UseWhen(x => x.Request.Path.Value.StartsWith("/api"), builder =>
            //{
            //    builder.UseMvc(routes =>
            //    {
            //        routes.MapRoute(
            //            name: "default",
            //            template: "{controller=Home}/{action=Index}/{id?}");
            //    });

            //});


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
