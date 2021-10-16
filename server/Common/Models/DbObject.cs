using System.Data;

namespace AutoComplete.Common.Models
{
    public abstract class DbObject
    {
        public string Id { get; set; }

        public abstract string ToLabel();
        public abstract bool UpdateScheme(IDataRecord record);

        public abstract DbRef ToFieldChoice();
    }
}