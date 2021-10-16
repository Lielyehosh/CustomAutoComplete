using System;
using AutoComplete.Common.Models;

namespace AutoComplete.Common.Interfaces
{
    public interface IQueryService
    {
        public void RegisterNewTableScheme(Type schemeType);
        Query CreateAutoCompleteQuery(string tableName, QueryOperation searchOperation,
            string substring, int limit);

        Query CreateQueryFromFilter(string tableName, QueryFilter queryFilter);
    }
}