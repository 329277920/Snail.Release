using Newtonsoft.Json;
using Snail.Release.Business.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Release.Core
{
    /// <summary>
    /// 缓存到文件
    /// </summary>
    public class ResponseCachedToFile : IResponseCachedProvider
    {
        public async Task<ResponseCachedItem> Get(ReleaseParams releaseParams)
        {
            if (!File.Exists(releaseParams.FilePath))
            {
                return null;
            }
            byte[] buffer = null;
            using (var stream = new FileStream(releaseParams.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                buffer = new byte[stream.Length];                 
                await stream.ReadAsync(buffer, 0, buffer.Length);
            }
            var jsonObj = SystemConfig.Instance.TryGetEncoding.GetString(buffer);
            return JsonConvert.DeserializeObject<ResponseCachedItem>(jsonObj);
        }

        public async Task<bool> Set(ReleaseParams releaseParams, ResponseCachedItem cachedItem)
        {
            if (!releaseParams.FilePath.TryCreateDirectory())
            {
                return false;
            }
            var jsonObj = JsonConvert.SerializeObject(cachedItem);
            var buffer = SystemConfig.Instance.TryGetEncoding.GetBytes(jsonObj);
            using (var stream = new FileStream(releaseParams.FilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            return File.Exists(releaseParams.FilePath);
        }

        // private Byte[] Sear
    }
}
