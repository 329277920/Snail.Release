using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Release.Core
{
    /// <summary>
    /// 缓存项管理类
    /// </summary>
    public class ResponseCachedMamnager
    {
        public static async Task<ResponseCachedItem> Get(ReleaseParams releaseParams)
        {
            if (!File.Exists(releaseParams.FilePath))
            {
                return null;
            }
            var cachedItem = new ResponseCachedItem();
            using (var stream = new FileStream(releaseParams.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                cachedItem.Body = new byte[stream.Length];
                await stream.ReadAsync(cachedItem.Body, 0, cachedItem.Body.Length);
            }
            return cachedItem;
        }

        public static async Task<bool> Set(ReleaseParams releaseParams, ResponseCachedItem cachedItem)
        {
            if (!releaseParams.FilePath.TryCreateDirectory())
            {
                return false;
            }
            using (var stream = new FileStream(releaseParams.FilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                await stream.WriteAsync(cachedItem.Body, 0, cachedItem.Body.Length);
            }
            return File.Exists(releaseParams.FilePath);
        }
    }
}
