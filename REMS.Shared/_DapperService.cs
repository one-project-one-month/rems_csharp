namespace REMS.Shared
{
    public class _DapperService
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;

        public _DapperService(string connectionString)
        {
            _sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>)> QueryMultipleAsync<T1, T2>(string storedProcedure, object parameters = null)
        {
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            using var multi = await db.QueryMultipleAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            var result1 = (await multi.ReadAsync<T1>()).ToList();
            var result2 = (await multi.ReadAsync<T2>()).ToList();

            return (result1, result2);
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>)> QueryMultipleAsync<T1, T2, T3>(string storedProcedure, object parameters = null)
        {
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            using var multi = await db.QueryMultipleAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            var result1 = (await multi.ReadAsync<T1>()).ToList();
            var result2 = (await multi.ReadAsync<T2>()).ToList();
            var result3 = (await multi.ReadAsync<T3>()).ToList();

            return (result1, result2, result3);
        }
    }
}
