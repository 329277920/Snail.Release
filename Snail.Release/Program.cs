using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.ResponseCaching;
using System.Net.Http.Headers;
using System.Text;
using ServiceStack.OrmLite;
using ServiceStack.DataAnnotations;
using Snail.Release.Business.Model;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Net;

namespace Snail.Release
{
    public class Program
    {
        public class MyClass
        {
            public string Name { get; set; }
        }

        public static void Main(string[] args)
        {            
            BuildWebHost(args).Run();         
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(ConfigKestrel)
                .UseStartup<Startup>()
                // .UseUrls("http://*:10000")        
                .Build();

        private static void ConfigKestrel(KestrelServerOptions opts)
        {
            opts.Listen(IPAddress.Parse("192.168.10.82"), 10000, listenOptions =>
            {
                //填入之前iis中生成的pfx文件路径和指定的密码　　　　　　　　　　　　
                listenOptions.UseHttps(@"F:\Git\Snail.Release\Snail.Release\wwwroot\crt\webrtc1.pfx", "chennanfei");
            });
        }
    }
}
