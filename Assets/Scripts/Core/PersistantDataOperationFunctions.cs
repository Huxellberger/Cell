// Copyright (C) Threetee Gang All Rights Reserved

using System;
using System.IO;
using System.Security.Cryptography;

namespace Assets.Scripts.Core
{
    public static class PersistantDataOperationFunctions 
    {
        public static byte[] EncryptFileStream(Stream stream, byte[] key, byte[] initVector)
        {
            // Check arguments. 
            if (stream == null)
            {
                throw new ArgumentNullException("Naked Bytes");
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }

            if (initVector == null || initVector.Length <= 0)
            {
                throw new ArgumentNullException("IV");
            }

            // IDisposable so should encapsulate in using call to ensure scoping
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = key;
                rijAlg.IV = initVector;
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;

                // Create an encryptor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                var encryptedStream = new MemoryStream();
                using (var cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
                {
                    int data;
                    while ((data = stream.ReadByte()) != -1)
                        cryptoStream.WriteByte((byte)data);
                }

                return encryptedStream.ToArray();
            }
        }

        public static byte[] DecryptFileStream(Stream encryptedBytes, byte[] key, byte[] initVector)
        {
            // Check arguments. 
            if (encryptedBytes == null)
            {
                throw new ArgumentNullException("Encrypted Bytes");
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }

            if (initVector == null || initVector.Length <= 0)
            {
                throw new ArgumentNullException("IV");
            }

            // IDisposable so should encapsulate in using call to ensure scoping
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = key;
                rijAlg.IV = initVector;
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;

                // Create a decryptor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // var stream = new MemoryStream(encryptedBytes);
                var decryptedStream = new MemoryStream();
                using (var cryptoStream = new CryptoStream(decryptedStream, decryptor, CryptoStreamMode.Write))
                {
                    int data;
                    while ((data = encryptedBytes.ReadByte()) != -1)
                        cryptoStream.WriteByte((byte)data);
                }

                return decryptedStream.ToArray();
            }
        }
    }
}
