using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    public class Hashing
    {
        public void CalculateCheckSum()
        {
            HashingExamples he = new HashingExamples();
            he.CalculateCheckSum("Hello World");

            HashingExamples he2 = new HashingExamples();
            he2.CalculateCheckSum("Hello World");
            he2.CalculateCheckSum("World Hello");
        }
        public void Sha256() => new HashingExamples().Sha2("Hello world");

    }
    public class HashingExamples
    {
        public void CalculateCheckSum(string source)
        {
            Console.WriteLine($"Checksum for {source} is {CheckSum(source)}.");
            Console.WriteLine($"Hash for {source} is {source.GetHashCode():X}");
        }
        private int CheckSum(string source)
        {
            return source.ToCharArray().Aggregate(0, (result, element) => result + element);
        }
        public void Sha2(string source)
        {
            Console.Write($"Hash for {source} is: ");
            byte[] hash = CalculateHash(source);
            foreach (byte b in hash)
                Console.Write("{0:X} ", b);
            Console.WriteLine();
        }
        private byte[] CalculateHash(string source)
        {
            ASCIIEncoding converter = new ASCIIEncoding();
            byte[] sourceBytes = converter.GetBytes(source);
            HashAlgorithm hasher = SHA256.Create();
            byte[] hash = hasher.ComputeHash(sourceBytes);
            return hash;
        }
    }
}
