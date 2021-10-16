using System;
using System.Collections.Generic;

namespace AutoComplete.Common.Models
{
    public class QueryFilter
    {
        public string Name { get; set; }
        public QueryOperation Operation { get; set; }
        /// <summary>
        /// JSON value of filter
        /// </summary>
        public object Value { get; set; }

        private string ToSqlMultipleConditions(string op)
        {
            var cond = " ( ";
            var queries = (List<QueryFilter>) Value;
            for (var i = 0; i < queries.Count; i++)
            {
                var query = queries[i];
                var queryCond = query.ToSqlCondition();
                cond += $" {queryCond} ";
                if (i != queries.Count - 1)
                    cond += $" {op} ";
            }
            return cond + " ) ";
        }
        
        public string ToSqlCondition()
        {
            // TODO - LY - replace this logic with existing libraries (linq for example)
            return Operation switch
            {
                QueryOperation.Prefix => $" {Name} LIKE \'{Value}%\' ",
                QueryOperation.Suffix => $" {Name} LIKE \'%{Value}\' ",
                QueryOperation.SubString => $" {Name} LIKE \'%{Value}%\' ",
                QueryOperation.Equal => $" {Name} LIKE \'{Value}\' ",
                QueryOperation.And => ToSqlMultipleConditions("AND"),
                QueryOperation.Or => ToSqlMultipleConditions("OR"),
                _ => throw new IndexOutOfRangeException("Unknown query operation")
            };
        }
    }
}