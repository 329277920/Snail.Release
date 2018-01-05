using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Snail.Release.Business.Config;

namespace Snail.Release.Core
{
    /// <summary>
    /// 模板管理类，存储系统模板，解析当前地址的模板
    /// </summary>
    public class ReleaseRoutes
    {
        public static ReleaseRoutes Instance = new Lazy<ReleaseRoutes>(() =>
        {
            return new ReleaseRoutes();
        }, true).Value;
         
        private ReleaseRoutes()
        {
           
        }

        /// <summary>
        /// 获取当前请求的模板对象，以及物理路径
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ReleaseParams Parse(HttpContext context)
        {
            var items = ReleaseConfig.Instance.Items;
            if (items == null || items.Length <= 0)
            {
                return null;
            }
            var path = context.Request?.Path.Value;
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }            
            foreach (var temp in items)
            {
                if (temp.IsMatch(path))
                {
                    var pyPath = temp.PhysicalPath(path);
                    if (string.IsNullOrEmpty(pyPath))
                    {
                        continue;
                    }
                    pyPath = Path.Combine(SystemConfig.Instance.StaticFilePath, pyPath);
                    var releaseParams = new ReleaseParams()
                    {
                        Template = temp.Template,
                        FilePath = pyPath
                    };
                    return releaseParams;
                }               
            }
            return null;
        }
    }
}
