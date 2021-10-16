using AutoComplete.Common.Models;
using AutoComplete.Common.Services;
using AutoComplete.Models;
using NUnit.Framework;

namespace AutoComplete.Tests
{
    public class QueryServiceTests
    {
        private QueryService MQueryService { get; set; }
        
        [SetUp]
        public void Setup()
        {
            MQueryService = new QueryService();
        }

        [Test]
        [TestCase("city", "Tel",10)]
        [TestCase("city", "a",10)]
        [TestCase("city", "",10)]
        [TestCase("city", "shalom",10)]
        public void RegisterTableTest(string tableName, string substring, int limit)
        {
            MQueryService.RegisterNewTableScheme(typeof(City));
            var query = MQueryService.CreateAutoCompleteQuery(tableName, QueryOperation.Prefix, substring, limit);
            if (query.Table == tableName && query.Limit == limit && query.Filter.Operation == QueryOperation.Or)
                Assert.Pass();
            Assert.Fail();
        }

    }
}