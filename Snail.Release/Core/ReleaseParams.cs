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
        /// 获取请求资源对应的物理路径
        /// </summary>
        public string FilePath { get; set; }   
                 
        /// <summary>
        /// 获取请求路径对应的模板
        /// </summary>
        public string Template { get; set; }
      
        /// <summary>
        /// 获取指定缓存目标对象
        /// </summary>
        public int CachedTargets { get; set; }

        /// <summary>
        /// 获取在客户端请求路径中解析出的参数集合
        /// </summary>
        public List<string> Parameters { get;  }

        /// <summary>
        /// 获取在客户端请求路径中解析出的生成物理路径集合
        /// </summary>
        public List<string> Paths { get; }

        /// <summary>
        /// 获取获设置客户端真实请求路径
        /// </summary>
        public string Uri { get; set; }

        public ReleaseParams()
        {
            Parameters = new List<string>();
            Paths = new List<string>();
        }
    }
}
