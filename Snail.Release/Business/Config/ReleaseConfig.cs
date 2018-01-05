using Snail.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Snail.Release.Business.Config
{
    /// <summary>
    /// 模板配置文件管理
    /// </summary>
    public class ReleaseConfig : IConfiguration
    {
        private const string ConfigFilePath = "Config/release.json";

        public ReleaseItem[] Items { get; set; }

        public ReleaseConfig()
        {
            this.Items = new ReleaseItem[0];
        }

        public static ReleaseConfig Instance
        {
            get { return ConfigurationManager.Get<ReleaseConfig>(ConfigFilePath); }
        }

        public void Fill(Stream stream)
        {
            var content = "";
            using (StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8))
            {
                content = sr.ReadToEnd();
            }
            if (string.IsNullOrEmpty(content))
            {
                return;
            }
            Snail.Data.Serializer.JsonPopulateObject(content, this);
        }
    }
}
