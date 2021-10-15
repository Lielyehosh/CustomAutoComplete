using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoComplete.Common.Models;

namespace AutoComplete.BFF.Interfaces
{
    public interface ISearchableAsync
    {
        Task<IEnumerable<T>> SearchAsync<T>(Query query, CancellationToken ct);
    }
}