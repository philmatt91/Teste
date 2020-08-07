using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BairesDev_Challenge.Models;
using BairesDev_Challenge.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BairesDev_Challenge
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
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<JsonTools>>();

            services.AddSingleton(typeof(ILogger), logger);

            services.AddSingleton(Configuration.GetSection("ConfigurationSettings").Get<ConfigurationSettings>());
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                app.UseHsts();
            }            

            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes                    
                    .MapRoute(name: "Route", template: "{controller}/{action}/{id?}");
            });        
        }
    }
}
