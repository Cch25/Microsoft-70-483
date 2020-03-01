using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    public class SymmetricAndAsymmetricEncryption
    {
        public void AesEncryption() => new AesEncryption().AesEncryptionTest();
        public void AesDecryption() => new AesEncryption().DecryptAes(null, null, null);
        public void RSAEncryptionDecryption() => new RSA().RSAEncryptDecrypt();
        public void KeyStorageLocal() => new KeyStorage().LocalStorage();
        public void KeyStorageMachine() => new KeyStorage().MachineStorage();
        public void DoubleEncryption() => new EncryptStreamExample().EncryptStream();
    }

    #region [ Aes ]
    public class AesEncryption
    {
        public void AesEncryptionTest()
        {
            string plainText = "This is my text to encrypt";
            //byte[] to hold the encrypted data
            byte[] cypherText;
            //byte[] to hold the key that was used to encrypt
            byte[] key;
            //byte[] that was used to the initialization vector that was used for encryption
            byte[] initializationVector;

            //Create AES instance. This will create a random key and initialization vector

            using (Aes aes = Aes.Create())
            {
                //copy the key and initialization vector
                key = aes.Key;
                initializationVector = aes.IV;

                //create an encryptor to encrypt some data (use using in prod)
                ICryptoTransform encryptor = aes.CreateEncryptor();

                //create memory stream to receive the encrypted data

                using (MemoryStream ms = new MemoryStream())
                {
                    //create a CryptoStream, tell it the stream to write to and the encryptor to use. Also set the mode
                    using (CryptoStream encryptCryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        //make a stream writer from the cryptoStream
                        using (StreamWriter sw = new StreamWriter(encryptCryptoStream))
                        {
                            sw.Write(plainText);
                        }
                        //get the encrypted message from the stream
                        cypherText = ms.ToArray();
                    }
                }
                //Display data
                Console.WriteLine($"String to encrypt {plainText}");
                DumpBytes("key: ", key);
                DumpBytes("Initialization vector: ", initializationVector);
                DumpBytes("Encrypted: ", cypherText);

            }
        }

        public void DecryptAes(byte[] encryptedText, byte[] key, byte[] IV)
        {
            string decryptedText = string.Empty;
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = IV;

                ICryptoTransform cryptoTransform = aes.CreateDecryptor();

                using (MemoryStream ms = new MemoryStream(encryptedText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            decryptedText = sr.ReadToEnd();
                        }
                    }
                }
            }
            Console.WriteLine($"Decripted text: \n{decryptedText}");
        }

        private void DumpBytes(string title, byte[] bytes)
        {
            Console.WriteLine(title);
            foreach (byte b in bytes)
            {
                Console.Write($"{b:X}");
            }
            Console.WriteLine();
        }
    }
    #endregion

    #region [ RSA ]
    public class RSA
    {
        public void RSAEncryptDecrypt()
        {
            string text = "This is my decoded text";
            // RSA works on byte arrays, not strings of text
            // This will convert our input string into bytes and back
            ASCIIEncoding converter = new ASCIIEncoding();

            byte[] plainBytes = converter.GetBytes(text);
            byte[] encryptedBytes;
            byte[] decryptedBytes;
            // Create a new RSA to encrypt the data should be wrapped in using for production code

            RSACryptoServiceProvider rsaEncrypt = new RSACryptoServiceProvider();
            // get the keys out of the encryptor
            string publicKey = rsaEncrypt.ToXmlString(includePrivateParameters: false);
            string privateKey = rsaEncrypt.ToXmlString(includePrivateParameters: true);

            // Now tell the encyryptor to use the public key to encrypt the data
            rsaEncrypt.FromXmlString(publicKey);

            // Use the encryptor to encrypt the data. The fOAEP parameter
            // specifies how the output is "padded" with extra bytes
            // For maximum compatibility with receiving systems, set this as false
            encryptedBytes = rsaEncrypt.Encrypt(plainBytes, fOAEP: false);
            Console.WriteLine($"Encrypted text: \"{converter.GetString(encryptedBytes)}\"");
            // Now do the decode - use the private key for this
            // We have sent someone our public key and they
            // have used this to encrypt data that they are sending to us

            RSACryptoServiceProvider rsaDecrypt = new RSACryptoServiceProvider();
            rsaDecrypt.FromXmlString(privateKey);
            decryptedBytes = rsaDecrypt.Decrypt(encryptedBytes, fOAEP: false);
            Console.WriteLine($"Decrypted text: \"{converter.GetString(decryptedBytes)}\"");
        }
    }

    #endregion

    #region [ Key storage ]
    public class KeyStorage
    {
        public void LocalStorage()
        {
            string containerName = "MyKeyStore";
            CspParameters csp = new CspParameters();
            csp.KeyContainerName = containerName;
            RSACryptoServiceProvider rsaStore = new RSACryptoServiceProvider(csp);
            Console.WriteLine($"Stored keys {rsaStore.ToXmlString(true)}");
            RSACryptoServiceProvider rsaLoad = new RSACryptoServiceProvider(csp);
            Console.WriteLine($"Loaded keys {rsaLoad.ToXmlString(true)}");

            rsaStore.PersistKeyInCsp = false;
            rsaStore.Clear();
        }
        public void MachineStorage()
        {
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = "Machine Level Key";
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParams);
            Console.WriteLine(rsa.ToXmlString(includePrivateParameters: false));
            // Make sure that it is persisting keys
            rsa.PersistKeyInCsp = true;
            // Clear the provider to make sure it saves the key
            rsa.Clear();
        }
    }
    #endregion

    #region [ Encrypt stream ]
    public class EncryptStreamExample
    {
        public void EncryptStream()
        {
            string plainText = "This is my plain text";
            //byte[] to hold the encrypted message;
            byte[] encryptedText;
            // byte arrays to hold the key that was used for encryption
            byte[] key1;
            byte[] key2;

            // byte array to hold the initialization vector that was used for encryption
            byte[] iv1;
            byte[] iv2;

            using Aes aes1 = Aes.Create();
            // copy the key and the initialization vector
            key1 = aes1.Key;
            iv1 = aes1.IV;

            ICryptoTransform encryptor1 = aes1.CreateEncryptor();
            // Create a new memory stream to receive the encrypted data.
            using (MemoryStream ms = new MemoryStream())
            {
                // create a CryptoStream, tell it the stream to write to and the encryptor to use. Also set the mode
                using CryptoStream cs1 = new CryptoStream(ms, encryptor1, CryptoStreamMode.Write);
                // Add another layer of encryption
                using Aes aes2 = Aes.Create();
                // copy the key and the initialization vector
                key2 = aes2.Key;
                iv2 = aes2.IV;
                ICryptoTransform encryptor2 = aes2.CreateEncryptor();
                using CryptoStream cs2 = new CryptoStream(cs1, encryptor2, CryptoStreamMode.Write);
                using (StreamWriter sw = new StreamWriter(cs2))
                {
                    sw.Write(plainText);
                }
                // get the encrypted message from the stream
                encryptedText = ms.ToArray();
            }

            // Now do the decryption
            string decryptedText;
            using Aes aesd1 = Aes.Create();
            // Configure the aes instances with the key and initialization vector to use for the decryption
            aesd1.Key = key1;
            aesd1.IV = iv1;
            // Create a decryptor from aes1
            ICryptoTransform decryptor1 = aesd1.CreateDecryptor();
            using (MemoryStream decryptStream = new MemoryStream(encryptedText))
            {
                using CryptoStream decryptCryptoStream1 = new CryptoStream(decryptStream, decryptor1, CryptoStreamMode.Read);
                using Aes aesd2 = Aes.Create();
                // Configure the aes instances with the key and initialization vector to use for the decryption
                aesd2.Key = key2;
                aesd2.IV = iv2;
                // Create a decryptor from aes2
                ICryptoTransform decryptor2 = aesd2.CreateDecryptor();
                using CryptoStream decryptCryptoStream2 = new CryptoStream(decryptCryptoStream1, decryptor2, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new StreamReader(decryptCryptoStream2);
                decryptedText = srDecrypt.ReadToEnd();
            }
            Console.WriteLine("Decrypted string: {0}", decryptedText);
        }
    }
    #endregion
}
