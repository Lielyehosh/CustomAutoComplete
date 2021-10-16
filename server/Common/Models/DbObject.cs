using System.Data;

namespace AutoComplete.Common.Models
{
    public abstract class DbObject
    {
        public string Id { get; set; }
        public abstract string ToLabel();
        public abstract bool UpdateScheme(IDataRecord record);
        public abstract DbRef ToDbRef();

        /// <summary>
        /// some of the tables doesn't have an Id field in db (primary key is a combination between multiple keys)
        /// </summary>
        /// <returns></returns>
        public static QueryFilter GetIdQueryFilter(string id)
        {
            return new QueryFilter()
            {
                Name = "id",
                Operation = QueryOperation.Equal,
                Value = id
            };
        }
    }
}