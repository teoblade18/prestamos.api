using System.Security.Cryptography;
using System.Text;

namespace prestamos.api.Util
{
    public interface IEncrypter
    {
        string Encrypt(string password);
        string Decrypt(string cipherText, string key);
    }

    public class Encrypter: IEncrypter
    {
        public string Encrypt(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string Decrypt(string cipherText, string key)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(PadRight(key, 16));
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.IV = cipherBytes.Take(16).ToArray(); // Extract IV from cipher bytes

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new System.IO.MemoryStream(cipherBytes.Skip(16).ToArray()))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        string PadRight(string text, int length)
        {
            if (text.Length >= length)
            {
                return text.Substring(0, length);
            }
            else
            {
                return text.PadRight(length, '\0');
            }
        }
    }
}
