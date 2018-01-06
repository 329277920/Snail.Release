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
                if (CachedBuffer.ContainsKey(releaseParams.FilePath))
                {
                    return CachedBuffer[releaseParams.FilePath];
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
                if (CachedBuffer.ContainsKey(releaseParams.FilePath))
                {
                    CachedBuffer.Remove(releaseParams.FilePath);
                }
                CachedBuffer.Add(releaseParams.FilePath, cachedItem);
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
