using System.IO;
using System.Security.Cryptography;

namespace SpeechAnalyzer
{
    public static class Scrambler
    {
        static byte[] key = new byte[16] { 0x12, 0xAF, 0x34, 0xAC, 0x00, 0x01, 0x02, 0x05, 0x32, 0x00, 0x00, 0xDD, 0x00, 0xFF, 0x00, 0x00 };
        static byte[] IV = new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        static byte[] encryptedBytes = null;
        static byte[] clearBytes = null;

        public static byte[] AESEncryptBytes(byte[] clearBytes)
        {
            using (Aes aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.Key = key;
                aes.IV = IV;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(),
          CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            return encryptedBytes;
        }

        public static byte[] AESDecryptBytes(byte[] cryptBytes)
        {
            using (Aes aes = new AesManaged())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cryptBytes, 0, cryptBytes.Length);
                        cs.Close();
                    }
                    clearBytes = ms.ToArray();
                }
            }
            return clearBytes;
        }
    }
}
