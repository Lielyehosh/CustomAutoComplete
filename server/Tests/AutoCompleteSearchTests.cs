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
        }

        
        [Test]
        [TestCase("")]
        [TestCase("isr")]
        [TestCase("i")]
        [TestCase("a")]
        [TestCase("A")]
        [TestCase("I")]
        public async Task SearchAutoCompleteTest(string nameSubString)
        {
            nameSubString = nameSubString.ToLower();
            var cts = new CancellationTokenSource();
            // cts.CancelAfter(TimeSpan.FromSeconds(60));
            var conn = new MySqlDal("Server=127.0.0.1;Database=test_db;uid=root;pwd=root");
            var results = await conn.SearchAutoComplete<City>(new Query()
            {
                Name = "name",
                Operation = QueryOperation.Prefix,
                Value = nameSubString,
                Table = "city",
                Limit = 10
            }, cts.Token);
            if (results.Any(result => !result.Name.ToLower().StartsWith(nameSubString)))
            {
                Assert.Fail();
                return;
            }
            Assert.Pass();
        }
        
    }
}
