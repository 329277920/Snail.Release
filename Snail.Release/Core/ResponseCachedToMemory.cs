using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Release.Core
{
    /// <summary>
    /// 缓存到内存
    /// </summary>
    public class ResponseCachedToMemory : IResponseCachedProvider
    {
        /// <summary>
        /// 获取或设置缓存提供者扩展配置
        /// </summary>
        public dynamic Config { get; set; }

        private static IDictionary<string, ResponseCachedItem> CachedBuffer;

        private static System.Threading.ReaderWriterLockSlim Lock;

        static ResponseCachedToMemory()
        {
            CachedBuffer = new Dictionary<string, ResponseCachedItem>();
            Lock = new System.Threading.ReaderWriterLockSlim();
        }

        public async Task<ResponseCachedItem> Get(ReleaseParams releaseParams)
        {
            try
            {
                Lock.EnterReadLock();
                if (CachedBuffer.ContainsKey(releaseParams.Key))
                {
                    return CachedBuffer[releaseParams.Key];
                }                
            }
            finally
            {
                Lock.ExitReadLock();
            }
            await Task.CompletedTask;
            return null;
        }

        public async Task<bool> Set(ReleaseParams releaseParams, ResponseCachedItem cachedItem)
        {
            try
            {
                Lock.EnterWriteLock();
                if (CachedBuffer.ContainsKey(releaseParams.Key))
                {
                    CachedBuffer.Remove(releaseParams.Key);
                }
                CachedBuffer.Add(releaseParams.Key, cachedItem);
                await Task.CompletedTask;
                return true;
            }
            finally
            {
                Lock.ExitWriteLock();
            }            
        }
    }
}
