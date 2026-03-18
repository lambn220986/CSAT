using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CSAT.Data
{
    public abstract class BaseRepository
    {
        private readonly DbConnectionFactory _factory;
        protected BaseRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            object param = null)
        {
            using (var conn = _factory.CreateConnection())
            {
                return await conn.QueryAsync<T>(sql, param);
            }
        }

        protected async Task<T> QuerySingleAsync<T>(
            string sql,
            object param = null)
        {
            using (var conn = _factory.CreateConnection())
            {
                return await conn.QuerySingleOrDefaultAsync<T>(sql, param);
            }
        }

        protected async Task<int> ExecuteAsync(
            string sql,
            object param = null,
            IDbTransaction tran = null)
        {
            if (tran != null)
                return await tran.Connection.ExecuteAsync(sql, param, tran);

            using (var conn = _factory.CreateConnection())
            {
                return await conn.ExecuteAsync(sql, param);
            }
        }
    }
}
