using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Release.Core
{
    /// <summary>
    /// 缓存提供程序接口
    /// </summary>
    public interface IResponseCachedProvider
    {
        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <param name="releaseParams">缓存项解析参数</param>
        /// <returns>返回缓存项</returns>
        Task<ResponseCachedItem> Get(ReleaseParams releaseParams);

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <param name="releaseParams">缓存项解析参数</param>
        /// <param name="cachedItem">缓存项</param>
        /// <returns>返回成功或失败</returns>
        Task<bool> Set(ReleaseParams releaseParams, ResponseCachedItem cachedItem);
    }
}
