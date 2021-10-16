using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoComplete.Common.Models;

namespace AutoComplete.Common.Interfaces
{
    public interface IMySqlDal
    {
        public Task<List<DbRef>> FindAutoCompleteAsync<TScheme>(Query query, CancellationToken ct)
            where TScheme: DbObject, new();
        
        public Task<TScheme> FindByIdAsync<TScheme>(Query query, CancellationToken ct)
            where TScheme: DbObject, new();
    }
}