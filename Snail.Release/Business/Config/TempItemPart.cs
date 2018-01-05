using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Release.Business.Config
{
    /// <summary>
    /// 模板配置项每个部分定义
    /// </summary>
    public class TempItemPart
    {
        public string Path { get; set; }
      
        /// <summary>
        /// 部分定义类型
        /// </summary>
        public string PartType { get; set; }
    }
}
