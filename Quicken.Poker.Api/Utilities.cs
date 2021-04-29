using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;


namespace Quicken.Poker.Api.Utilities
{
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Returns an individual HTTP Header value
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetHeader(this Microsoft.AspNetCore.Http.HttpRequest request, string key)
        {
             
            if (request.Headers[key].Any())
                return null;

            return request.Headers[key];
        }
    }
    public static class TokenFunctions
	{


        public static string GetUserLoginFromToken(string token)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(token)) return null;

				//Token is in format {userLogin}|{iso8601date}
				string[] decoded = EncryptionFunctions.Decrypt(token).Split('|');

				string userLogin = decoded[0];
				string date = decoded[1];

				//Fail check if time difference is over an hour
				TimeSpan dateDiff = (DateTime.UtcNow - DateTime.Parse(date));
				if (Math.Abs(dateDiff.Minutes) > 120) throw new SecurityException("Invalid authorization token.");

				return userLogin;
			}
			catch (Exception e)
			{
				return null;
			}
		}
	}

    public static class EncryptionFunctions
    {
        private static readonly int iterations = 1000;
        private const string DefaultKey = "~tQ!dp3!ky4QJdX]";

        public static string Encrypt(string input, string password = DefaultKey)
        {
            byte[] encrypted;
            byte[] IV;
            byte[] Salt = GetSalt();
            byte[] Key = CreateKey(password, Salt);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC;

                aesAlg.GenerateIV();
                IV = aesAlg.IV;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            byte[] combinedIvSaltCt = new byte[Salt.Length + IV.Length + encrypted.Length];
            Array.Copy(Salt, 0, combinedIvSaltCt, 0, Salt.Length);
            Array.Copy(IV, 0, combinedIvSaltCt, Salt.Length, IV.Length);
            Array.Copy(encrypted, 0, combinedIvSaltCt, Salt.Length + IV.Length, encrypted.Length);

            return Convert.ToBase64String(combinedIvSaltCt.ToArray());
        }

        public static string Decrypt(string input, string password = DefaultKey)
        {
            byte[] inputAsByteArray;
            string plaintext = null;
            try
            {
                inputAsByteArray = Convert.FromBase64String(input);

                byte[] Salt = new byte[32];
                byte[] IV = new byte[16];
                byte[] Encoded = new byte[inputAsByteArray.Length - Salt.Length - IV.Length];

                Array.Copy(inputAsByteArray, 0, Salt, 0, Salt.Length);
                Array.Copy(inputAsByteArray, Salt.Length, IV, 0, IV.Length);
                Array.Copy(inputAsByteArray, Salt.Length + IV.Length, Encoded, 0, Encoded.Length);

                byte[] Key = CreateKey(password, Salt);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (var msDecrypt = new MemoryStream(Encoded))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }

                return plaintext;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static byte[] CreateKey(string password, byte[] salt)
        {
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
                return rfc2898DeriveBytes.GetBytes(32);
        }

        private static byte[] GetSalt()
        {
            var salt = new byte[32];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        private static string FixBase64Length(string input)
        {
            //Remove extra spacers at end if string is too long
            while (input.EndsWith("=") && input.Length % 4 != 0)
            {
                input = input.Substring(0, input.Length - 1);
            }

            while (input.Length % 4 != 0)
            {
                input = input + "=";
            }

            return input;
        }
    }
     




}
