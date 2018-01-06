using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Snail.Release.Business;
using Snail.Release.Business.Config;
using Snail.Release.Core; 
using System.IO;
using System.Threading;

namespace Snail.Release.Core
{
    /// <summary>
    /// 发布中间件
    /// </summary>
    public class ReleaseMiddleware : BaseMiddleware
    {
        public ReleaseMiddleware(RequestDelegate next) : base(next)
        {

        }

        public override async Task Invoke(HttpContext context)
        {
            var releaseParams = ReleaseRoutes.Instance.Route(context);
            if (releaseParams == null)
            {
                await base.Invoke(context);
                return;
            }
            var reRelease = context.Request.Method != "GET"
                || context.Request.Headers.ContainsKey(SystemConfig.Instance.ReReleaseHeader)
                || context.Request.Query.ContainsKey(SystemConfig.Instance.ReReleaseHeader);
            var oResult = reRelease ? false : await Output(context, releaseParams);
            if (oResult)
            {
                return;
            }
            context.Items.Add(SystemConfig.Instance.CacheItemParams, releaseParams);
            context.Response.Body = new ResponseCachedStream(context.Response.Body);
            context.Request.Path = SystemConfig.Instance.ReleasePath;
            await base.Invoke(context);
            var cachedItem = new ResponseCachedItem(context);
            await Input(cachedItem, releaseParams);            
        }

        /// <summary>
        /// 输出静态文件至前端
        /// </summary>
        /// <param name="context"></param>
        /// <param name="releaseParams"></param>
        /// <returns></returns>
        private async Task<bool> Output(HttpContext context, ReleaseParams releaseParams)
        {
            try
            {                
                var cachedItem = await ResponseCachedMamnager.Get(releaseParams);
                if (cachedItem == null)
                {
                    return false;
                }
                context.Response.StatusCode = cachedItem.StatusCode;
                foreach (var header in cachedItem.Headers)
                {
                    if (!context.Response.Headers.ContainsKey(header.Key))
                    {
                        context.Response.Headers.Add(header.Key, header.Value);
                    }
                }               
                await context.Response.Body.WriteAsync(cachedItem.Body, 0, cachedItem.Body.Length);                
                return true;
            }
            catch (Exception ex)
            {
                // todo: 日志
            }
            return false;
        }

        /// <summary>
        /// 写入静态文件
        /// </summary>       
        /// <param name="cachedItem"></param>
        /// <param name="releaseParams"></param>
        /// <returns></returns>
        private async Task<bool> Input(ResponseCachedItem cachedItem, ReleaseParams releaseParams)
        {
            try
            {
                return await ResponseCachedMamnager.Set(releaseParams, cachedItem);                 
            }
            catch (Exception ex)
            {
                // todo: 日志
            }
            return false;
        }
    }
}
