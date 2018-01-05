using Snail.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Snail.Release.Business.Config
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemConfig : IConfiguration
    {
        private const string ConfigFilePath = "Config/system.json";

        /// <summary>
        /// 系统编码
        /// </summary>
        public string Encoding { get; set; }

        private System.Text.Encoding _encode;
        public System.Text.Encoding TryGetEncoding
        {
            get
            {
                try
                {
                    if (_encode == null)
                    {
                        _encode = System.Text.Encoding.GetEncoding(this.Encoding);
                    }
                }
                catch (Exception ex)
                {                    
                    // todo: log
                }
                _encode = _encode ?? System.Text.Encoding.UTF8;
                return this._encode;
            }            
        }
        
        /// <summary>
        /// 静态文件保存路径
        /// </summary>
        public string StaticFilePath { get; set; }

        /// <summary>
        /// 发布路由地址
        /// </summary>
        public string ReleasePath { get; set; }  

        /// <summary>
        /// 模板文件保存路径
        /// </summary>
        public string TemplateFilePath { get; set; }

        /// <summary>
        /// 发布参数存储key
        /// </summary>
        public string CacheItemParams { get; private set; }  
        
        /// <summary>
        /// 重新发布请求头名称
        /// </summary>
        public string ReReleaseHeader { get; set; } 

        public static SystemConfig Instance
        {
            get { return ConfigurationManager.Get<SystemConfig>(ConfigFilePath); }
        }

        public void Fill(Stream stream)
        {
            CacheItemParams = "_____Cache_Item_Params_____";
            ReReleaseHeader = "_____Re_Release_Header_____";
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
