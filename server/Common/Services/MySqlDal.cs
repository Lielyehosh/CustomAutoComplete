using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoComplete.Common.Interfaces;
using AutoComplete.Common.Models;
using MySql.Data.MySqlClient;

namespace AutoComplete.Common.Services
{
    public class MySqlDal : IMySqlDal, IAsyncDisposable
    {
        private string ConnectionString { get; set; }
        public MySqlDal(string connectionString)
        {
            ConnectionString = connectionString;
        }


        public async Task<List<DbRef>> FindAutoCompleteAsync<TScheme>(Query query, CancellationToken ct) 
            where TScheme : DbObject, new()
        {
            var results = new List<DbRef>();
            await using var connection = new MySqlConnection(ConnectionString);
            try
            {
                var queryStr = query.ToSqlQuery();
                var command = new MySqlCommand(queryStr, connection);
                await connection.OpenAsync(ct);
                var reader = await command.ExecuteReaderAsync(ct);

                // Call Read before accessing data.
                while (await reader.ReadAsync(ct))
                {
                    var result = new TScheme();
                    if (result.UpdateScheme(reader))
                        results.Add(result.ToFieldChoice());
                }

                // Call Close when done reading.
                await reader.CloseAsync();
                return results;
            }
            catch (Exception ex)
            {
                // noop
                // normal case, this requests will be canceled each time the user type new substring
                throw;
            }
            finally
            {
                await connection.CloseAsync(ct);
            }
        }

        public async Task<TScheme> FindByIdAsync<TScheme>(Query query, CancellationToken ct) 
            where TScheme : DbObject, new()
        {
            await using var connection = new MySqlConnection(ConnectionString);
            var result = new TScheme();
            try
            {
                var queryStr = query.ToSqlQuery();
                var command = new MySqlCommand(queryStr, connection);
                await connection.OpenAsync(ct);
                var reader = await command.ExecuteReaderAsync(ct);

                // Call Read before accessing data.
                while (await reader.ReadAsync(ct))
                {
                    if (result.UpdateScheme(reader))
                        break;
                }

                // await reader.CloseAsync();
                return result;
            }
            catch (Exception ex)
            {
                // noop
                throw;
            }
            finally
            {
                // Call Close when done reading.
                await connection.CloseAsync(ct);
            }
        }

        public ValueTask DisposeAsync()
        {
            // dispose all resources
            return default;
        }
    }
}