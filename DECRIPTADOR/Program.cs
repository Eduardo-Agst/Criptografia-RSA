using System;
using System.IO;
using System.Security.Cryptography;

namespace DECRIPTADOR
{
    class Program
    {
        static CspParameters _cspp = new CspParameters();
        static RSACryptoServiceProvider _rsa;

        const string DecrFile = @"c:\encrypt\texto.enc";
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
            {
                Console.WriteLine("CHAVE NÃO ENCONTRADA");
            }
            else
            {
                {
                    if (DecrFile != null)
                    {
                        DecryptFile(new FileInfo(DecrFile));
                        Console.WriteLine("Arquivo Decriptado");
                    }
                }
            }
            static void DecryptFile(FileInfo file)
            {
                Aes aes = Aes.Create();
                byte[] LenK = new byte[4];
                byte[] LenIV = new byte[4];
                string outFile = @"C:/encrypt/texto_decriptado.txt";

                using (var inFs = new FileStream(file.FullName, FileMode.Open))
                {
                    inFs.Seek(0, SeekOrigin.Begin);
                    inFs.Read(LenK, 0, 3);
                    inFs.Seek(4, SeekOrigin.Begin);
                    inFs.Read(LenIV, 0, 3);

                    int lenK = BitConverter.ToInt32(LenK, 0);
                    int lenIV = BitConverter.ToInt32(LenIV, 0);
                    int startC = lenK + lenIV + 8;
                    int lenC = (int)inFs.Length - startC;

                    byte[] KeyEncrypted = new byte[lenK];
                    byte[] IV = new byte[lenIV];

                    inFs.Seek(8, SeekOrigin.Begin);
                    inFs.Read(KeyEncrypted, 0, lenK);
                    inFs.Seek(8 + lenK, SeekOrigin.Begin);
                    inFs.Read(IV, 0, lenIV);

                    byte[] KeyDecrypted = _rsa.Decrypt(KeyEncrypted, false);

                    ICryptoTransform transform = aes.CreateDecryptor(KeyDecrypted, IV);

                    using (var outFs = new FileStream(outFile, FileMode.Create))
                    {
                        int count = 0;
                        int offset = 0;

                        int blockSizeBytes = aes.BlockSize / 8;
                        byte[] data = new byte[blockSizeBytes];

                        inFs.Seek(startC, SeekOrigin.Begin);
                        using (var outStreamDecrypted =
                            new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                        {
                            do
                            {
                                count = inFs.Read(data, 0, blockSizeBytes);
                                offset += count;
                                outStreamDecrypted.Write(data, 0, count);
                            } while (count > 0);

                            outStreamDecrypted.FlushFinalBlock();
                        }
                    }
                }
            }
        }
    }
}
