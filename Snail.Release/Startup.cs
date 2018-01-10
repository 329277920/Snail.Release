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
using Snail.Release.Business.Config;
using System.Text;

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

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
            app.UseMvc();
            app.UseStaticFiles(new StaticFileOptions()
            {
                 OnPrepareResponse = responseContext => 
                 {                     
                     if (SystemConfig.Instance.NeedClientCached(responseContext.Context.Request.Path))
                     {
                         responseContext.Context.Response.Headers.Remove(HeaderNames.ETag);
                         responseContext.Context.Response.Headers.Remove(HeaderNames.LastModified);
                         responseContext.Context.Response.Headers.Add(HeaderNames.CacheControl, "max-age=" + SystemConfig.Instance.ClientCachedTime);
                     }
                 }
            });            
        }
    }
}
