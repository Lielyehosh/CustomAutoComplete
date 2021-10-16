using System.Collections.Generic;

// This code is a C# copy of the JAVA implementation here - https://github.com/samgh/Byte-by-Byte-Solutions/blob/master/java/Autocomplete.java

namespace AutoComplete.Common.Utils
{
    
    internal class Node<TValue> {
        internal string Prefix { get; set; }
        internal Dictionary<char, Node<TValue>> Children { get; set; }

        internal bool IsWord { get; set; }
        
        internal TValue Value { get; set; }

        internal Node(string prefix) {
            Prefix = prefix;
            Children = new Dictionary<char, Node<TValue>>();
        }
    }
    public class AutoCompleteTrie<TValue>
    {
        private Node<TValue> Trie { get; set; }
        
        public AutoCompleteTrie(IDictionary<string, TValue> dict)
        {
            Trie = new Node<TValue>("");
            foreach (var pair in dict)
            {
                InsertNodeValue(pair.Key, pair.Value);
            }
        }
        
        private void InsertNodeValue(string key, TValue value) {
            var curr = Trie;
            for (var i = 0; i < key.Length; i++) {
                if (!curr.Children.ContainsKey(key[i])) {
                    curr.Children[key[i]] = new Node<TValue>(key[..(i+1)]);
                }
                curr = curr.Children[key[i]];
                if (i == key.Length - 1)
                {
                    curr.IsWord = true;
                    curr.Value = value;
                } 
            }
        }
        
        // Find all words in trie that start with prefix
        public IEnumerable<TValue> FindByPrefix(string pre) {
            var results = new LinkedList<TValue>();
        
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

        private void FindAllChildWords(Node<TValue> node, LinkedList<TValue> results)
        {
            if (node.IsWord) 
                results.AddLast(node.Value);
            foreach (var key in node.Children.Keys)
            {
                FindAllChildWords(node.Children[key], results);
            }
        }
    }
}