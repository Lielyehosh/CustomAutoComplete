namespace AutoComplete.Common.Models
{
    public class Query
    {
        public string Table { get; set; }
        public string Name { get; set; }
        public QueryOperation Operation { get; set; }
        public string Value { get; set; }

        public int Limit { get; set; } = 0;

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
                case QueryOperation.And:
                case QueryOperation.Or:
                default:
                    // TBD
                    break;
            }

            if (Limit > 0)
            {
                query += $" LIMIT {Limit}";
            }

            return $"{query};";
        }
    }
}