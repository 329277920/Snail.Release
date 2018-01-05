using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Snail.Release.Core;

namespace Snail.Release
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
            services.AddMvc();                      
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<UnknownExceptionMiddleware>();
            app.UseMiddleware<ReleaseMiddleware>();
            // app.UseResponseCaching();
            app.UseMvc();
            //app.Run(async (context) =>
            //{
            //    context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
            //    {
            //        Public = true,
            //        MaxAge = TimeSpan.FromSeconds(10)
            //    };
            //    context.Response.Headers[HeaderNames.Vary] = new string[] { "Accept-Encoding" };

            //    await context.Response.WriteAsync("Hello World! " + DateTime.UtcNow);
            //});
        }
    }
}
