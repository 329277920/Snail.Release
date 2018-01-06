using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Snail.Release.Core
{
    /// <summary>
    /// 输出缓存项
    /// </summary>
    [Serializable]
    public class ResponseCachedItem
    {
        public Dictionary<string, string> Headers { get; set; }

        public byte[] Body { get; set; }

        public int StatusCode { get; set; }

        public ResponseCachedItem()
        {
            Headers = new Dictionary<string, string>();
        }

        public ResponseCachedItem(HttpContext context) : this()
        {
            Body = ((ResponseCachedStream)context.Response.Body).ToArray();
            Headers.Clear();
            Headers.Add("Content-Length", Body.Length.ToString());
            if (context.Response.Headers != null)
            {
                foreach (var header in context.Response.Headers)
                {
                    if (header.Key.Equals("Transfer-Encoding"))
                    {
                        continue;
                    }
                    Headers.Add(header.Key, header.Value);
                }
            }
            StatusCode = context.Response.StatusCode;
        }        
    }
}
