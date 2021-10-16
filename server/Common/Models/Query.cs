namespace AutoComplete.Common.Models
{
    public class Query
    {
        public string Table { get; set; }
        public string Name { get; set; }
        public QueryOperation Operation { get; set; }
        public string Value { get; set; }
        /// <summary>
        /// the limit of results from the query.
        /// if 0 then no limit is required
        /// </summary>
        public int Limit { get; set; }

        public static Query CreateAutoCompleteQuery(string substring, QueryOperation searchOperation, string tableName, int limit)
        {
            return new Query
            {
                Name = "Name",
                Operation = searchOperation,
                Value = substring,
                Table = tableName,
                Limit = limit
            };
        }

        public static Query CreateSearchByIdQuery(string id, string tableName)
        {
            return new Query
            {
                Name = "Id",
                Operation = QueryOperation.Equal,
                Value = id,
                Table = tableName,
                Limit = 0
            };
        }
        
        public string ToStringQuery()
        {
            var query = $"SELECT * FROM {Table}";
            switch (Operation)
            {
                case QueryOperation.Prefix:
                    query = $"SELECT * FROM {Table} WHERE {Name} LIKE \'{Value}%\'";
                    break;
                case QueryOperation.Suffix:
                    query = $"SELECT * FROM {Table} WHERE {Name} LIKE \'%{Value}\'";
                    break;
                case QueryOperation.SubString:
                    query = $"SELECT * FROM {Table} WHERE {Name} LIKE \'%{Value}%\'";
                    break;
                case QueryOperation.Equal:
                    query = $"SELECT * FROM {Table} WHERE {Name} = \'{Value}\'";
                    break;
                case QueryOperation.And:
                    // TBD
                case QueryOperation.Or:
                    // TBD
                default:
                    // TBD
                    break;
            }

            if (Limit > 0) query += $" LIMIT {Limit}";

            return $"{query};";
        }
    }
}