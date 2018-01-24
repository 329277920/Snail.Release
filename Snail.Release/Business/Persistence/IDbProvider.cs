using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Snail.Release.Business.Persistence
{
    /// <summary>
    /// 数据库提供程序接口
    /// </summary>
    public interface IDbProvider
    {
        dynamic Config { get; set; }

        Task<List<T>> SelectAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken token);

        Task<List<T>> SelectAsync<T>(Expression<Func<T, bool>> predicate);

        Task<T> SingleAsync<T>(Expression<Func<T, bool>> predicate);

        Task<T> SingleAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken token);
    }
}
