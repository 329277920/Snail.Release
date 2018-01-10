using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Release.Business.Config
{
    public class SystemDbConfig
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

        /// <summary>
        /// 配置参数
        /// </summary>
        public dynamic Config { get; set; }
    }
}
