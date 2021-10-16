using System.Data;
using AutoComplete.Common.Attributes;
using AutoComplete.Common.Models;

namespace AutoComplete.Models
{
    public class City : DbObject
    {
        [SearchField]
        public string Name { get; set; }
        public string Country { get; set; }
        public string SubCountry { get; set; }
        public string GeoNameId { get; set; }

        public override string ToLabel()
        {
            return Name;
        }

        public override bool UpdateScheme(IDataRecord record)
        {
            Name = (string) record[0];
            Country = (string) record[1];
            SubCountry = (string) record[2];
            GeoNameId = (string) record[3];
            // case there is no primary key for the table
            Id = $"{Name}_{Country}_{GeoNameId}";
            return true;
        }

        public override DbRef ToFieldChoice()
        {
            return new DbRef()
            {
                Id = Id,
                Label = ToLabel()
            };
        }
    }
}