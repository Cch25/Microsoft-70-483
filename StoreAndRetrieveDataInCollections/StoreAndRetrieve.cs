using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace StoreAndRetrieveDataInCollections
{
    public class StoreAndRetrieve
    {
        public void BasicCollections() => new CollectionsExamples().BasicCollections();
        public void CountLetterOccurencies() => new CollectionsExamples().CountLettersOccurencies();
        public void HashSets() => new CollectionsExamples().HashSets();
        public void StacksAndQueues() => new CollectionsExamples().QueuesAndStacks();

    }

    public class CollectionsExamples
    {
        public void BasicCollections()
        {
            int[] arr = new int[2];
            arr[0] = 5;
            arr[1] = 2;

            int[,] matrix = new int[2, 3]
            {
                {2,3,4 },
                {2,3,4 }
            };

            int[][][] jaggedArray = new int[][][]
            {
                new int[][]{
                    new int[] { 2,2,1 },
                    new int[] { 1,1,1,1 },
                    new int[] { 3 }
                },
                new int[][]{
                    new int[] { 1,5 },
                    new int[] { 2,5,5,5,5,5 } },
                new int[][]{
                    new int[] { 3,1,0 },
                    new int[] { 2,0,0,0,0,0,1 },
                    new int[] { 1,0,0,1,1 },
                    new int[] { 4,2,3,4,5,1 } }
            };

            ArrayList arrayList = new ArrayList();
            arrayList.Add(5);
            arrayList.Add(2);
            arrayList.Add(3);

            List<string> myList = new List<string>()
            {
                "A","B","C"
            };

            Dictionary<int, BankAccount> keyValues = new Dictionary<int, BankAccount>();

            BankAccount ba = new BankAccount() { AccountNo = 1, Name = "Bear" };
            BankAccount ba1 = new BankAccount() { AccountNo = 2, Name = "Bearison" };

            keyValues.Add(ba.AccountNo, ba);
            keyValues.Add(ba1.AccountNo, ba1);

            Console.WriteLine(keyValues[ba.AccountNo].ToString());
        }
        public void CountLettersOccurencies()
        {
            Dictionary<char, int> occurencies = new Dictionary<char, int>();

            string text = File.ReadAllText("TextFile.txt");

            foreach (char c in Regex.Replace(text, @"\s+", "").ToCharArray())
            {
                if (occurencies.ContainsKey(c))
                {
                    occurencies[c] = occurencies[c] + 1;
                    continue;
                }
                occurencies.Add(c, 1);
            }
            Console.WriteLine("We've got: ");
            foreach (KeyValuePair<char, int> item in occurencies)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
            Console.Write($"Most used letter {occurencies.OrderByDescending(x => x.Value).Select(x => x.Key).First()}");
        }
        public void HashSets()
        {
            HashSet<string> hashOne = new HashSet<string> { "A", "B", "C" };

            HashSet<string> hashTwo = new HashSet<string> {"D","E","C"};//note that is the same as the first hash 

            HashSet<string> search = new HashSet<string>() { "C", "A" };

            if (search.IsSubsetOf(hashOne))
            {
                Console.WriteLine($"All that you find in search is also in hashOne");
            }

            if (search.IsSubsetOf(hashTwo))
            {
                Console.WriteLine($"All that you find in search is also in hashTwo");
            }
        }
        public void QueuesAndStacks()
        {
            //FIFO structure
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);

            //LIFO structure
            Stack<int> stacks = new Stack<int>();
            stacks.Push(1);
            stacks.Push(2);

        }

    }

    public class BankAccount
    {
        public int AccountNo { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Account {AccountNo} is held by {Name}";
        }
    }
}
