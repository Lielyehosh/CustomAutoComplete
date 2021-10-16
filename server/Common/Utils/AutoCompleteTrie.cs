using System.Collections.Generic;

// This code is a C# copy of the JAVA implementation here - https://github.com/samgh/Byte-by-Byte-Solutions/blob/master/java/Autocomplete.java

namespace AutoComplete.Common.Utils
{
    public class AutoCompleteTrie
    {
        // Trie node class
        internal class Node {
            internal string Prefix { get; set; }
            internal Dictionary<char, Node> Children { get; set; }

            internal bool IsWord { get; set; }

            internal Node(string prefix) {
                Prefix = prefix;
                Children = new Dictionary<char, Node>();
            }
        }

        // The trie
        private Node Trie { get; set; }
        
        // Construct the trie from the dictionary
        public AutoCompleteTrie(IEnumerable<string> dict)
        {
            Trie = new Node("");
            foreach (var s in dict)
            {
                InsertWord(s);
            }
        }
        
        private void InsertWord(string str) {
            var curr = Trie;
            for (var i = 0; i < str.Length; i++) {
                if (!curr.Children.ContainsKey(str[i])) {
                    curr.Children[str[i]] = new Node(str[..(i+1)]);
                }
                curr = curr.Children[str[i]];
                if (i == str.Length - 1) 
                    curr.IsWord = true;
            }
        }
        
        
        // Find all words in trie that start with prefix
        public IEnumerable<string> GetWordsForPrefix(string pre) {
            var results = new LinkedList<string>();
        
            // Iterate to the end of the prefix
            var curr = Trie;
            foreach (var ch in pre.ToCharArray())
            {
                if (curr.Children.ContainsKey(ch)) {
                    curr = curr.Children[ch];
                } else {
                    return results;
                }
            }
        
            // At the end of the prefix, find all child words
            FindAllChildWords(curr, results);
            return results;
        }

        private void FindAllChildWords(Node node, LinkedList<string> results)
        {
            if (node.IsWord) 
                results.AddLast(node.Prefix);
            foreach (var key in node.Children.Keys)
            {
                FindAllChildWords(node.Children[key], results);
            }
        }
    }
}