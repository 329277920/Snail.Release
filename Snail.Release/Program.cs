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

            //var con = new OrmLiteConnectionFactory("server=192.168.10.82;uid=sa;pwd=!23fi!oOp;database=TestDb;", SqlServerDialect.Provider).Open();

            //con.Insert(new NewsInfo() { Title = "new1", Content = "content1" });
            //con.Insert(new NewsInfo() { Title = "new2", Content = "content2" });

            BuildWebHost(args).Run();

            // ServiceContext         
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://*:10000")
                .Build();
    }
}
