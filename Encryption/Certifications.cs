using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Encryption
{
    public class Certificates
    {
        public void SignData() => new CertificatesExample().SignData();
    }
    public class CertificatesExample
    {
        public void SignData()
        {
            ASCIIEncoding converter = new ASCIIEncoding();

            X509Store store = new X509Store("demoCertStore", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2 certificate = store.Certificates[0];
            RSACryptoServiceProvider encryptProvider = certificate.PrivateKey as RSACryptoServiceProvider;
            string messageToSign = "This is the message I want to sign";
            byte[] messageToSignBytes = converter.GetBytes(messageToSign);
            // need to calculate a hash for this message - this will go into the
            // signature and be used to verify the message
            // Create an implementation of the hashing algorithm we are going to use
            // should be wrapped in using for production code
            HashAlgorithm hasher = new SHA1Managed();
            byte[] hash = hasher.ComputeHash(messageToSignBytes);
            // Now sign the hash to create a signature
            byte[] signature = encryptProvider.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
            // We can send the signature along with the message to authenticate it
            // Create a decryptor that uses the public key
            // should be wrapped in using for production code
            RSACryptoServiceProvider decryptProvider = certificate.PublicKey.Key as RSACryptoServiceProvider;
            // Now use the signature to perform a successful validation of the message
            bool validSignature = decryptProvider.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
            Console.WriteLine("Correct signature validated OK: {0}", validSignature);
            // Change one byte of the signature
            signature[0] = 99;
            // Now try the using the incorrect signature to validate the message
            bool invalidSignature = decryptProvider.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
            Console.WriteLine("Incorrect signature validated OK: {0}", invalidSignature);
        }
    }
}
