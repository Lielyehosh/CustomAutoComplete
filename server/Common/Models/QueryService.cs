using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoComplete.Common.Attributes;
using AutoComplete.Common.Interfaces;

namespace AutoComplete.Common.Models
{
    /// <summary>
    ///     Should be a singleton service, holds the query details of the db table object
    /// </summary>
    public class QueryService : IQueryService
    {
        private readonly Dictionary<string, List<string>> _mToSearchFields;

        public QueryService()
        {
            _mToSearchFields = new Dictionary<string, List<string>>();
        }

        /// <summary>
        ///     register table scheme to the service, should be called on setup
        /// </summary>
        public void RegisterNewTableScheme(Type schemeType)
        {
            var tableName = schemeType.Name.ToLower();
            _mToSearchFields[tableName] = CreateSearchFieldsForType(schemeType);
        }

        public Query CreateAutoCompleteQuery(string tableName, QueryOperation searchOperation,
            string substring, int limit)
        {
            // create query filter for each search field
            // create the 'OR' query with the list of the queries
            var queryFilter = new QueryFilter()
            {
                Operation = QueryOperation.Or,
                Value = _mToSearchFields[tableName]
                    .Select(searchField => new QueryFilter
                    {
                        Name = searchField,
                        Operation = searchOperation,
                        Value = substring
                    })
                    .ToList()
            };
            var query = CreateQueryFromFilter(tableName, queryFilter);
            query.Limit = limit;
            return query;
        }

        public Query CreateQueryFromFilter(string tableName, QueryFilter queryFilter)
        {
            return new Query
            {
                Table = tableName,
                Filter = queryFilter
            };
        }

        private static List<string> CreateSearchFieldsForType(Type type)
        {
            var searchFields = type.GetProperties()
                .Select(field => new
                {
                    field, attr = field.GetCustomAttributes<SearchFieldAttribute>().FirstOrDefault()
                })
                .Where(fieldAndAttr => fieldAndAttr.attr != null)
                .Select(fieldAndAttr => fieldAndAttr.field.Name.ToLower())
                .ToList();
            return searchFields;
        }
    }
}