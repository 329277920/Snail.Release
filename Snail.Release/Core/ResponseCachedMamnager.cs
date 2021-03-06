﻿using Snail.Release.Business.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Unity;

namespace Snail.Release.Core
{
    /// <summary>
    /// 缓存项管理类
    /// </summary>
    public class ResponseCachedMamnager
    {
        private static UnityContainer Container;
     
        static ResponseCachedMamnager()
        {
            try
            {
                InitCachedProviders();
                SystemConfig.Instance.OnConfigChanged += (sender, e) =>
                {
                    InitCachedProviders();
                };
            }
            catch (Exception ex)
            {
                // todo: log
            }
           
        }

        private static void InitCachedProviders()
        {
            // 加载缓存提供者
            try
            {
                var container = new UnityContainer();
                var providers = SystemConfig.Instance?.Providers;
                if (providers == null || providers.Length <= 0)
                {
                    throw new Exception("未配置任何缓存提供者");
                }
                foreach (var provider in providers)
                {
                    if (!provider.Assembly.ToLower().EndsWith(".dll"))
                    {
                        provider.Assembly += ".dll";
                    }
                    var instance = Assembly.LoadFrom(provider.Assembly)?.CreateInstance(provider.Type) as IResponseCachedProvider;
                    if (instance == null)
                    {
                        throw new Exception(string.Format("在程序集{0}中未找到类型{1}.", provider.Assembly, provider.Type));
                    }
                    instance.Config = provider.Config;
                    container.RegisterInstance(provider.Name, instance);
                }              
                if (Container != null)
                {
                    Container.Dispose();
                }
                Container = container;
            }
            catch (Exception ex)
            {
                // todo: 日志
            }
        }

        public static async Task<ResponseCachedItem> Get(ReleaseParams releaseParams)
        {
            if (Container == null)
            {
                return null;
            }
            ResponseCachedItem result = null;
            IResponseCachedProvider preProvider = null;
            foreach (var item in releaseParams.CachedProviders)
            {
                try
                {
                    var provider = GetProvider(item);
                    if (provider == null)
                    {
                        throw new Exception(string.Format("未找到缓存提供者:{0}.", item));
                    }
                    result = await provider.Get(releaseParams);
                    if (result != null)
                    {
                        if (preProvider != null)
                        {
                            await provider.Set(releaseParams, result);
                        }
                        break;
                    }
                    preProvider = provider;
                }
                catch (Exception ex)
                {
                    // todo: log
                }
            }
            return result;
        }

        public static async Task<bool> Set(ReleaseParams releaseParams, ResponseCachedItem cachedItem)
        {
            if (Container == null)
            {
                return false;
            }
            var result = true;
            foreach (var item in releaseParams.CachedProviders)
            {
                try
                {
                    var provider = GetProvider(item);
                    if (provider == null)
                    {
                        throw new Exception(string.Format("未找到缓存提供者:{0}.", item));
                    }
                    if (!await provider.Set(releaseParams, cachedItem))
                    {
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    // todo: log
                }
            }
            return result;
        }

        private static IResponseCachedProvider GetProvider(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    if (Container.IsRegistered<IResponseCachedProvider>(name))
                    {
                        return Container.Resolve<IResponseCachedProvider>(name);
                    }
                }
                catch (Exception ex)
                {
                    // todo: 日志
                }
            }           
            return null;
        }
    }
}
