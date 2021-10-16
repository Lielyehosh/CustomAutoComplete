using System.Collections.Generic;
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
        [SearchField]
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
            Id = $"{Name},{Country},{GeoNameId}";
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

        public new static QueryFilter GetIdQueryFilter(string id)
        {
            var keys = id.Split(',');
            return new QueryFilter()
            {
                Operation = QueryOperation.And,
                Value = new List<QueryFilter>()
                {
                    new QueryFilter()
                    {
                        Name = "name",
                        Operation = QueryOperation.Equal,
                        Value = keys[0]
                    },
                    new QueryFilter()
                    {
                        Name = "country",
                        Operation = QueryOperation.Equal,
                        Value = keys[1]
                    },
                    new QueryFilter()
                    {
                        Name = "geonameid",
                        Operation = QueryOperation.Equal,
                        Value = keys[2]
                    }
                }
            };
        }
    }
}