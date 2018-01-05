using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Snail.Release.Core
{
    /// <summary>
    /// 处理未知异常
    /// </summary>
    public class UnknownExceptionMiddleware : BaseMiddleware
    {
        public UnknownExceptionMiddleware(RequestDelegate next) : base(next)
        {
           
        }

        public override async Task Invoke(HttpContext context)
        {                       
            try
            {
                await base.Invoke(context);
            }
            catch (Exception ex)
            {
                // todo : log
                Console.WriteLine(ex.Message);
            }           
        }
    }
}
