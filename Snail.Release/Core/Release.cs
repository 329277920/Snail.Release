using Microsoft.AspNetCore.Http;
using Snail.Release.Business.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Release.Core
{
    public class Release
    {
        public HttpContext Context
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取发布参数
        /// </summary>
        public ReleaseParams ReleaseParams
        {
            get { return Context.Items[SystemConfig.Instance.CacheItemParams] as ReleaseParams; }
        }

        public Release(HttpContext context)
        {
            Context = context;
        }

        public T Params<T>(int index)
        {
            var ps = ReleaseParams;
            if (ps.Parameters == null || ps.Parameters.Count <= index)
            {
                return default(T);
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(ps.Parameters[index]);
        }
    }
}
