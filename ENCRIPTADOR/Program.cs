using System;
using System.IO;
using System.Security.Cryptography;

namespace ENCRIPTADOR
{
    class Program
    {
        static CspParameters _cspp = new CspParameters();
        static RSACryptoServiceProvider _rsa;

        const string EncrFile = @"c:\encrypt\texto.txt";
        const string PubKeyFile = @"c:\encrypt\rsaPublicKey.txt";
        const string KeyName = "Key01";

        static void Main(string[] args)
        {
            _cspp.KeyContainerName = KeyName;
            _rsa = new RSACryptoServiceProvider(_cspp)
            {
                PersistKeyInCsp = true
            };

            if (_rsa is null)
                Console.WriteLine("CHAVE NÃO ENCONTRADA");
            else
            {
                if (EncrFile != null)
                {
                    EncryptFile(new FileInfo(EncrFile));
                    Console.WriteLine("Arquivo Encriptado");
                }
            }
        }

        static void EncryptFile(FileInfo file)
        {
            Aes aes = Aes.Create();
            ICryptoTransform transform = aes.CreateEncryptor();

            byte[] keyEncrypted = _rsa.Encrypt(aes.Key, false);
            int lKey = keyEncrypted.Length;
            byte[] LenK = BitConverter.GetBytes(lKey);
            int lIV = aes.IV.Length;
            byte[] LenIV = BitConverter.GetBytes(lIV);

            string outFile =
                Path.Combine(file.DirectoryName, Path.ChangeExtension(file.Name, ".enc"));

            using (var outFs = new FileStream(outFile, FileMode.Create))
            {
                outFs.Write(LenK, 0, 4);
                outFs.Write(LenIV, 0, 4);
                outFs.Write(keyEncrypted, 0, lKey);
                outFs.Write(aes.IV, 0, lIV);

                using (var outStreamEncrypted =
                    new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                {
                    int count = 0;
                    int offset = 0;

                    int blockSizeBytes = aes.BlockSize / 8;
                    byte[] data = new byte[blockSizeBytes];
                    int bytesRead = 0;

                    using (var inFs = new FileStream(file.FullName, FileMode.Open))
                    {
                        do
                        {
                            count = inFs.Read(data, 0, blockSizeBytes);
                            offset += count;
                            outStreamEncrypted.Write(data, 0, count);
                            bytesRead += blockSizeBytes;
                        } while (count > 0);
                    }
                    outStreamEncrypted.FlushFinalBlock();
                }
            }
        }
    }
}
