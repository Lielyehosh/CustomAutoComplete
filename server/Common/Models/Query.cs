using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoComplete.Common.Attributes;

namespace AutoComplete.Common.Models
{
    public class Query
    {
        public string Table { get; set; }
        
        public QueryFilter Filter { get; set; }

        /// <summary>
        /// the limit of results from the query.
        /// if 0 then no limit is required
        /// </summary>
        public int Limit { get; set; }

        
        // TODO - make it generic template and make sure its not execute on each request
        public static List<string> GetSearchFields<T>()
        {
            var searchFields = typeof(T).GetProperties()
                .Select(field => new
                    {field, attr = field.GetCustomAttributes<SearchFieldAttribute>().FirstOrDefault()})
                .Where(fieldAndAttr => fieldAndAttr.attr != null)
                .Select(fieldAndAttr => fieldAndAttr.field.Name.ToLower())
                .ToList();
            return searchFields;
        }
        
        public static Query CreateAutoCompleteQuery(string substring, QueryOperation searchOperation, string tableName, int limit)
        {
            return new Query
            {
                Filter = new QueryFilter()
                {
                    Operation = QueryOperation.Or,
                    Value = new List<QueryFilter>()
                    {
                        new QueryFilter()
                        {
                            Name = "Name",
                            Operation = searchOperation,
                            Value = substring
                        }, new QueryFilter()
                        {
                            Name = "GeoNameId",
                            Operation = searchOperation,
                            Value = substring
                        }
                    },
                },
                Table = tableName,
                Limit = limit
            };
        }

        public static Query CreateSearchByIdQuery(string id, string tableName)
        {
            return new Query
            {
                Filter = new QueryFilter()
                {
                    Name = "Id",
                    Operation = QueryOperation.Equal,
                    Value = id,
                },
                Table = tableName,
                Limit = 0
            };
        }

        public string GetSelectSqlQuery()
        {
            return $"SELECT * FROM {Table} ";
        }
        
        public string ToSqlQuery()
        {
            var selectQuery = GetSelectSqlQuery();
            var whereCond = Filter.ToSqlCondition();
            var sqlQuery = $"{selectQuery} WHERE {whereCond} ";
            if (Limit > 0)
                sqlQuery += $" LIMIT {Limit} ";
            return sqlQuery;
        }
    }
}