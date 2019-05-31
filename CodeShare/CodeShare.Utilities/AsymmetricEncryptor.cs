// ***********************************************************************
// Assembly         : CodeShare.Utilities
// Author           : Thomas Angeland
// Created          : 01-31-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="AsymmetricEncryptor.cs" company="CodeShare.Utilities">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace CodeShare.Utilities
{
    // Windows guide on encryption and decryption: https://docs.microsoft.com/en-us/dotnet/standard/security/walkthrough-creating-a-cryptographic-application

    /// <summary>
    /// Class RsaExtension.
    /// </summary>
    public static class RsaExtension
    {
        /// <summary>
        /// Imports from XML string.
        /// </summary>
        /// <param name="rsa">The RSA.</param>
        /// <param name="xmlString">The XML string.</param>
        /// <exception cref="Exception">Invalid XML RSA key.</exception>
        public static void ImportFromXmlString(this RSACryptoServiceProvider rsa, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement != null && xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
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

        /// <summary>
        /// Exports to XML string.
        /// </summary>
        /// <param name="rsa">The RSA.</param>
        /// <returns>System.String.</returns>
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

    /// <summary>
    /// Class AsymmetricEncryptor.
    /// </summary>
    public class AsymmetricEncryptor
    {
        /// <summary>
        /// The key size
        /// </summary>
        private const int KeySize = 2048;

        /// <summary>
        /// The private key
        /// </summary>
        private readonly string _privateKey;

        /// <summary>
        /// Gets the public parameters.
        /// </summary>
        /// <value>The public parameters.</value>
        public RSAParameters PublicParameters { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricEncryptor"/> class.
        /// </summary>
        public AsymmetricEncryptor()
        {
            using (var rsa = new RSACryptoServiceProvider(KeySize))
            {
                _privateKey = rsa.ExportToXmlString();
                PublicParameters = rsa.ExportParameters(false);
            }
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Base64s the encode.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>System.String.</returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Base64s the decode.
        /// </summary>
        /// <param name="base64EncodedData">The base64 encoded data.</param>
        /// <returns>System.String.</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}