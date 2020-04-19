using System.IO;
using FORFarm.Application;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using FluentValidation.AspNetCore;
using FORFarm.Application.Common.Interfaces;

namespace WebUI
{
    public class Startup
    {
        
        
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment WebHostEnvironment { get;  }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddFluentValidation(o => o.RegisterValidatorsFromAssemblyContaining<IFarmContext>());
            
            services.AddInfrastructure();
            services.AddPersistence(Configuration, WebHostEnvironment.IsDevelopment());
            services.AddApplication();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });

            services.AddSwaggerDocument(settings =>
            {
                settings.Title = "FORFarm API";
                settings.Version = "v1";
            });

            services.AddRouting(options => { options.LowercaseUrls = true; });
	        services.AddNodeServices(options =>{ options.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp");  });
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
                //app.UseHsts();
                //app.UseHttpsRedirection();
            }

            
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            
            app.UseOpenApi();
            app.UseSwaggerUi3();
            
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            
            
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
            
        }
    }
}
