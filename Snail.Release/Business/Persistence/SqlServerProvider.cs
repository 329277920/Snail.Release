using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Snail.Release.Business.Persistence
{
    public class SqlServerProvider : IDbProvider
    {
        private OrmLiteConnectionFactory _innerDb;
        private OrmLiteConnectionFactory _db
        {
            get
            {
                if (_innerDb != null)
                {
                    return _innerDb;
                }
                lock (this)
                {
                    if (_innerDb != null)
                    {
                        return _innerDb;
                    }
                    _innerDb = new OrmLiteConnectionFactory(Config.connectionString.ToString(), SqlServerDialect.Provider);
                }
                return _innerDb;
            }
        }

        public dynamic Config { get; set; }

        public async Task<List<T>> SelectAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken token)
        {
            using (var con = await _db.OpenAsync())
            {
                return await con.SelectAsync(predicate, token);
            }
        }

        public async Task<List<T>> SelectAsync<T>(Expression<Func<T, bool>> predicate)
        {
            return await SelectAsync(predicate, default(CancellationToken));
        }

        public async Task<T> SingleAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken token)
        {
            using (var con = await _db.OpenAsync())
            {
                return await con.SingleAsync(predicate, token);
            }
        }

        public async Task<T> SingleAsync<T>(Expression<Func<T, bool>> predicate)
        {
            return await SingleAsync(predicate, default(CancellationToken));
        }
    }
}
