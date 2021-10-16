using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoComplete.Common;
using AutoComplete.Common.Models;
using AutoComplete.Common.Services;
using AutoComplete.Models;
using NUnit.Framework;

namespace AutoComplete.Tests
{
    public class AutoCompleteSearchTests
    {

        [SetUp]
        public void Setup()
        {
            MQueryService = new QueryService();
            MQueryService.RegisterNewTableScheme(typeof(City));
        }

        private QueryService MQueryService { get; set; }


        [Test]
        [TestCase("")]
        [TestCase("isr")]
        [TestCase("i")]
        [TestCase("a")]
        [TestCase("A")]
        [TestCase("I")]
        [TestCase(null)]
        public async Task PrefixQueryTest(string nameSubString)
        {
            nameSubString = nameSubString.ToLower();
            var cts = new CancellationTokenSource();
            // cts.CancelAfter(TimeSpan.FromSeconds(60));
            var conn = new MySqlDal("Server=127.0.0.1;Database=test_db;uid=root;pwd=root");
            var results = await conn.FindAutoCompleteAsync<City>(new Query()
            {
                Filter = new QueryFilter()
                {
                    Name = "name",
                    Operation = QueryOperation.Prefix,
                    Value = nameSubString,
                },
                Table = "city",
                Limit = 10
            }, cts.Token);
            if (results.Any(result => !result.Label.ToLower().StartsWith(nameSubString)))
            {
                Assert.Fail();
                return;
            }
            Assert.Pass();
        }
        
        [Test]
        [TestCase("12")]
        [TestCase("Tel")]
        [TestCase("tel")]
        public async Task AndQueryTest(string nameSubString)
        {
            nameSubString = nameSubString.ToLower();
            var cts = new CancellationTokenSource();
            // cts.CancelAfter(TimeSpan.FromSeconds(60));
            var conn = new MySqlDal("Server=127.0.0.1;Database=test_db;uid=root;pwd=root");
            var results = await conn.FindAutoCompleteAsync<City>(new Query()
            {
                Filter = new QueryFilter()
                {
                    Operation = QueryOperation.Or,
                    Value = new List<QueryFilter>()
                    {
                        new QueryFilter()
                        {
                            Name = "name",
                            Operation = QueryOperation.Prefix,
                            Value = nameSubString,
                        },
                        new QueryFilter()
                        {
                            Name = "geonameid",
                            Operation = QueryOperation.Prefix,
                            Value = nameSubString,
                        }
                    },
                },
                Table = "city",
                Limit = 10
            }, cts.Token);
            if (results.Any(result => !result.Label.ToLower().StartsWith(nameSubString)))
            {
                Assert.Fail();
                return;
            }
            Assert.Pass();
        }

    }
}
