using Snail.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Snail.Release.Business.Config
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemConfig : IConfiguration
    {
        private System.Text.Encoding _encode;

        private const string ConfigFilePath = "Config/system.json";

        /// <summary>
        /// 系统编码
        /// </summary>
        public string Encoding { get; set; }

        /// <summary>
        /// 客户单缓存时间(秒)
        /// </summary>
        public int ClientCachedTime { get; set; }

        private string[] _clientCachedExts;
      
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

        public CachedProvider[] Providers { get; set; }

        [JsonProperty("databases")]
        public SystemDbConfig[] DbProviders { get; set; }

        /// <summary>
        /// 重新发布请求头名称
        /// </summary>
        public string ReReleaseHeader { get; set; }

        private string _clientCached;
        /// <summary>
        /// 启用客户端缓存的扩展名列表
        /// </summary>
        public string ClientCached
        {
            get
            {
                return _clientCached;
            }
            set
            {
                _clientCached = value;
                if (!string.IsNullOrEmpty(_clientCached))
                {
                    _clientCachedExts = (from item in
                                            _clientCached.Split(new string[] { "," },
                                            StringSplitOptions.RemoveEmptyEntries)
                                         select item.ToLower()).ToArray();
                }
            }
        }

        public static SystemConfig Instance
        {
            get { return ConfigurationManager.Get<SystemConfig>(ConfigFilePath); }
        }

        public void Fill(Stream stream)
        {
            CacheItemParams = "_____Cache_Item_Params_____";
            ReReleaseHeader = "_____Re_Release_Header_____";
            ClientCachedTime = 365 * 24 * 3600;
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
            OnConfigChanged?.Invoke(this, null);
        }

        /// <summary>
        /// 校验当前是否启用客户端缓存
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool NeedClientCached(string path)
        {
            if (_clientCachedExts == null)
            {
                return false;
            }
            var idx = path.LastIndexOf(".");
            if (idx < 0)
            {
                return false;
            }
            var ext = path.Substring(idx + 1).ToLower();
            return _clientCachedExts.Contains(ext);
        }

        /// <summary>
        /// 在配置文件发生变更时触发
        /// </summary>
        public event EventHandler OnConfigChanged;
    }
}
