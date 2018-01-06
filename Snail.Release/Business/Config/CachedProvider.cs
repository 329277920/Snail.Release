using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Release.Business.Config
{
    /// <summary>
    /// 缓存提供者
    /// </summary>
    public class CachedProvider
    {
        /// <summary>
        /// 提供者名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 程序集
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string Type { get; set; }
    }
}
