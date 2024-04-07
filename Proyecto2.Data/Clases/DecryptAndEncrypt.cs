﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.Data.Clases
{
    public class DecryptAndEncrypt
    {
        private static readonly string ENCRYPTION_KEY = "1234567891234567";

        public static string EncryptStringAES(string clearText)
        {
            string ciphertext = string.Empty;
            try
            {
                var keybytes = Encoding.UTF8.GetBytes(ENCRYPTION_KEY);
                var iv = Encoding.UTF8.GetBytes(ENCRYPTION_KEY);
                //c# encription
                var encryptStringToBytes = EncryptStringToBytes(clearText, keybytes, iv);
                // Decrypt the bytes to a string.
                var encryptedToJavascript = EncryptStringToBytes(clearText, keybytes, iv);
                ciphertext = Convert.ToBase64String(encryptedToJavascript);
            }
            catch
            {
                ciphertext = string.Empty;
            }
            return ciphertext;
        }

        public static string DecryptStringAES(string ciphredText)
        {
            string deciphertext = string.Empty;
            try
            {
                var keybytes = Encoding.UTF8.GetBytes(ENCRYPTION_KEY);
                var iv = Encoding.UTF8.GetBytes(ENCRYPTION_KEY);
                //c# encription
                var encryptStringToBytes = EncryptStringToBytes(ciphredText, keybytes, iv);
                //Decrypt the bytes to a string.
                var roundtrip = DecryptStringFromBytes(encryptStringToBytes, keybytes, iv);
                //DECRYPT FROM CRIPTOJS
                var encrypted = Convert.FromBase64String(ciphredText);
                var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
                deciphertext = decriptedFromJavascript.ToString();
            }
            catch
            {
                deciphertext = string.Empty;
            }
            return deciphertext;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            //Declare the string used to hold
            //the decrypted text.
            string plaintext = null;
            //Create an RijndaelManaged object
            //with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;
                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string. 
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("iv");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object 
            // with the specified key and IV. 
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;
                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
    }
}
