using System.Security.Cryptography;
using System.Text;

namespace EasySave
{
    internal static class MainPage
    {
        private static int Main(string[] args)
        {
            try
            {
                string inputFile = args[0];
                long fSize = (new FileInfo(inputFile)).Length;

                string password = args[1];
                byte[] salt = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

                byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(File.ReadAllText(inputFile));

                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    var key = new Rfc2898DeriveBytes(password, salt, 1000).GetBytes(aes.KeySize / 8);
                    aes.Key = key;
                    aes.IV = new byte[aes.BlockSize / 8];

                    using (FileStream output = new FileStream(inputFile, FileMode.Create))
                    using (CryptoStream encryptor = new CryptoStream(output, args[2] != "decrypt" ? aes.CreateEncryptor() : aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        encryptor.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                    }
                }
                new FileInfo(inputFile);
                if (fSize == (new FileInfo(inputFile)).Length)
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                return -1;
            }
            return 0;
        }
    }
}