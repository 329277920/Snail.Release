using Snail.Release.Business.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Unity;

namespace Snail.Release.Business.Persistence
{
    /// <summary>
    /// 数据库提供程序管理者
    /// </summary>
    public class DbProviderManager
    {         
        private static UnityContainer Container;

        static DbProviderManager()
        {           
            try
            {
                InitDbProviders();
                SystemConfig.Instance.OnConfigChanged += (sender, e) =>
                {
                    InitDbProviders();
                };
            }
            catch (Exception ex)
            {
                // todo: log
            }
        }

        /// <summary>
        /// 获取一个数据库管理程序
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IDbProvider Get(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {               
                try
                {
                    if (Container == null)
                    {
                        return null;
                    }
                    if (Container.IsRegistered<IDbProvider>(name))
                    {
                        return Container.Resolve<IDbProvider>(name);
                    }
                }
                catch (Exception ex)
                {
                    // todo: 日志
                }
            }
            return null;
        }       

        private static void InitDbProviders()
        {
            // 加载缓存提供者
            try
            {
                var container = new UnityContainer();
                var providers = SystemConfig.Instance?.DbProviders;
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
                    var instance = Assembly.LoadFrom(provider.Assembly)?.CreateInstance(provider.Type) as IDbProvider;
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
    }
}
