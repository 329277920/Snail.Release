using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Release.Core
{
    /// <summary>
    /// 抽象处理类
    /// </summary>
    public abstract class BaseMiddleware
    {
        /// <summary>
        /// 获取或设置一下个处理对象
        /// </summary>
        protected RequestDelegate Next { get; set; }

        public BaseMiddleware(RequestDelegate next)
        {
            this.Next = next;                
        }

        public virtual Task Invoke(HttpContext context)
        {
            return this.Next(context);
        }
    }
}
