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
        public string FilePath { get; set; }   
                 
        public string Template { get; set; }

        public int CachedTargets { get; set; }
    }
}
