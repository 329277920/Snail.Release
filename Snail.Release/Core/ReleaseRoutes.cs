using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Snail.Release.Business.Config;
using System.Text;

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
        public ReleaseParams Route(HttpContext context)
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
            foreach (var item in items)
            {
                var releaseParams = InnerParse(path, item);
                if (releaseParams != null)
                {
                    return releaseParams;
                }     
            }
            return null;
        }

        /// <summary>
        /// 从当前请求中解析出ReleaseParams
        /// </summary>
        /// <param name="path"></param>
        /// <param name="releaseItem"></param>
        /// <returns>返回模板,物理路径</returns>
        private ReleaseParams InnerParse(string path, ReleaseConfigItem releaseItem)
        {
            if (!releaseItem.IsMatch(path))
            {
                return null;
            }             
            var releaseParams = new ReleaseParams() {
                Template = releaseItem.Template,
                ClientCached = releaseItem.ClientCached
            };
            releaseParams.CachedProviders = releaseItem.TryGetProviders;
            releaseParams.Uri = path;
            releaseParams.Key = path;
            releaseParams.Parameters.AddRange(path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
            return releaseParams;
        }

        private string[] SplitPath(string path)
        {
            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }
            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return path.Split('/');
        }
    }
}
