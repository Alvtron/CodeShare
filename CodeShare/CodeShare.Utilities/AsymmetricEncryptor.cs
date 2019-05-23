using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace CodeShare.Utilities
{
    // Windows guide on encryption and decryption: https://docs.microsoft.com/en-us/dotnet/standard/security/walkthrough-creating-a-cryptographic-application

    public static class RSAExtension
    {
        public static void ImportFromXmlString(this RSACryptoServiceProvider rsa, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                        case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                        case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                        case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                        case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                        case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }

        public static string ExportToXmlString(this RSACryptoServiceProvider rsa)
        {
            RSAParameters parameters = rsa.ExportParameters(true);

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                Convert.ToBase64String(parameters.Modulus),
                Convert.ToBase64String(parameters.Exponent),
                Convert.ToBase64String(parameters.P),
                Convert.ToBase64String(parameters.Q),
                Convert.ToBase64String(parameters.DP),
                Convert.ToBase64String(parameters.DQ),
                Convert.ToBase64String(parameters.InverseQ),
                Convert.ToBase64String(parameters.D));
        }
    }

    public class AsymmetricEncryptor
    {
        private const int KeySize = 2048;

        private readonly string _privateKey;

        public RSAParameters PublicParameters { get; private set; }

        public AsymmetricEncryptor()
        {
            using (var rsa = new RSACryptoServiceProvider(KeySize))
            {
                _privateKey = rsa.ExportToXmlString();
                PublicParameters = rsa.ExportParameters(false);
            }
        }

        public string Decrypt(string data)
        {
            var dataInBytes = Convert.FromBase64String(data);

            using (var rsa = new RSACryptoServiceProvider(KeySize))
            {
                rsa.ImportFromXmlString(_privateKey);

                var decryptedBytes = rsa.Decrypt(dataInBytes, false);

                return Base64Decode(Convert.ToBase64String(decryptedBytes));
            }
        }

        public static string Encrypt(string data, RSAParameters parameters)
        {
            var dataInBytes = Convert.FromBase64String(Base64Encode(data));

            using (var rsa = new RSACryptoServiceProvider(KeySize))
            {
                rsa.ImportParameters(parameters);
                var encryptedBytes = rsa.Encrypt(dataInBytes, false);
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}