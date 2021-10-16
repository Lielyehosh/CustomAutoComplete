using System.Collections.Generic;
using System.Linq;
using AutoComplete.Common.Utils;
using NUnit.Framework;

namespace AutoComplete.Tests
{
    public class AutoCompleteTrieTests
    {
        [SetUp]
        public void Setup()
        {
            NormalList = new string[] {"abc", "acd", "bcd", "def", "a", "aba"};
            EmptyList = new string[] {};
        }

        private string[] EmptyList { get; set; }
        private string[] NormalList { get; set; }

        [Test]
        [TestCase(new[] {"abc"}, "abc")]
        [TestCase(new[] {"abc", "acd", "bcd", "def", "a", "aba"}, "")]
        [TestCase(new[] {"def"}, "def")]
        [TestCase(new string[] {}, "defa")]
        public void Tests(string[] expectedResults, string prefix) {
            var autoCompleteTrie = new AutoCompleteTrie(NormalList);
            var results = autoCompleteTrie.GetWordsForPrefix(prefix);
            Assert.IsTrue(CompareArrays(results,expectedResults));
        }

        public bool CompareArrays(IEnumerable<string> A, IEnumerable<string> B)
        {
            if (A.Count() != B.Count()) 
                return false;

            var aOrdered = A.OrderBy(s => s).ToList();
            var bOrdered = B.OrderBy(s => s).ToList();
        
        
            for (int i = 0; i < aOrdered.Count(); i++) {
                if (aOrdered[i] != bOrdered[i]) 
                    return false;
            }
            return true;
        }
    }
}