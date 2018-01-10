using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Release.Core
{
    /// <summary>
    /// 缓存参数
    /// </summary>
    public class ReleaseParams
    {
        /// <summary>
        /// 获取请求资源对应的唯一Key值
        /// </summary>
        public string Key { get; set; }   
                 
        /// <summary>
        /// 获取请求路径对应的模板
        /// </summary>
        public string Template { get; set; }
      
        /// <summary>
        /// 获取指定缓存目标对象
        /// </summary>
        public string[] CachedProviders { get; set; }

        /// <summary>
        /// 获取在客户端请求路径中解析出的参数集合
        /// </summary>
        public List<string> Parameters { get;  }        

        /// <summary>
        /// 获取获设置客户端真实请求路径
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// 获取或设置是否启用客户端缓存
        /// </summary>
        public bool ClientCached { get; set; }

        public ReleaseParams()
        {
            Parameters = new List<string>();           
        }
    }
}
