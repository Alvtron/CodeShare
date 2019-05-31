// ***********************************************************************
// Assembly         : CodeShare.RestApi
// Author           : Thomas Angeland
// Created          : 01-31-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="AuthDataSource.cs" company="CodeShare.RestApi">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using CodeShare.Model;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CodeShare.Utilities;

namespace CodeShare.RestApi
{
    /// <summary>
    /// Class AuthDataSource.
    /// </summary>
    public static class AuthDataSource
    {
        /// <summary>
        /// The base URI
        /// </summary>
        private const string BaseUri = @"http://localhost:50214/api/";

        /// <summary>
        /// The client
        /// </summary>
        private static HttpClient _client;
        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>The client.</value>
        public static HttpClient Client
        {
            get
            {
                if (_client != null)
                {
                    return _client;
                }

                _client = new HttpClient
                {
                    BaseAddress = new Uri(BaseUri)
                };
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return _client;
            }
        }

        /// <summary>
        /// The controller
        /// </summary>
        private const string Controller = "auth";

        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <returns>Task&lt;System.Nullable&lt;RSAParameters&gt;&gt;.</returns>
        private static async Task<RSAParameters?> GetPublicKey()
        {
            Logger.WriteLine("Retrieving public key from the Auth Rest Api...");

            try
            {
                var json = await Client.GetStringAsync($"{Controller}/publickey").ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json))
                {
                    Logger.WriteLine("Returned empty RSA parameters from REST API.");
                    return null;
                }

                return JsonConvert.DeserializeObject<RSAParameters>(json);
            }
            catch (HttpRequestException)
            {
                Logger.WriteLine("Failed to retrieve public key from the REST API.");
                return null;
            }
        }

        /// <summary>
        /// Signs the in.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;User&gt;.</returns>
        public static async Task<User> SignIn(string userName, string password)
        {
            Logger.WriteLine("Signing in user to database...");

            if (userName == null)
            {
                Logger.WriteLine("Can't sign in user. Provided user name was empty.");
                return null;
            }

            if (password == null)
            {
                Logger.WriteLine($"Can't sign in user {userName}. Provided password was empty.");
                return null;
            }

            var parameters = await GetPublicKey();

            if (!parameters.HasValue)
            {
                Logger.WriteLine($"Can't sign in user {userName}. The client is unable to fetch public key from REST API.");
                return null;
            }

            var encryptedCredential = new EncryptedCredential(userName, password, parameters.Value);

            var postBody = JsonConvert.SerializeObject(encryptedCredential, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            try
            {
                Logger.WriteLine("Sending user sign in credentials (encrypted) to REST API.");
                var response = await Client.PostAsync($"{Controller}/signin", new StringContent(postBody, Encoding.UTF8, "application/json")).ConfigureAwait(false);

                if (response == null)
                {
                    Logger.WriteLine("The sign up was unsuccessful. No response was given.");
                    return null;
                }
                if (!response.IsSuccessStatusCode)
                {
                    Logger.WriteLine($"The sign up was unsuccessful (Response code: {response.StatusCode}).");
                    return null;
                }

                Logger.WriteLine($"The sign in was successful (Response code: {response.StatusCode}).");
                return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException e)
            {
                Logger.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Signs up.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;User&gt;.</returns>
        /// <exception cref="ArgumentException">
        /// Provided user name was invalid. - userName
        /// or
        /// Provided email was invalid. - email
        /// or
        /// Provided password was invalid. - password
        /// </exception>
        public static async Task<User> SignUp(string userName, string email, string password)
        {
            Logger.WriteLine("Signing up user to database...");

            if (Validate.UserName(userName) == ValidationResponse.Invalid)
            {
                Logger.WriteLine("Can't sign up user. Provided user name was invalid.");
                throw new ArgumentException("Provided user name was invalid.", nameof(userName));
            }

            if (Validate.Email(email) == ValidationResponse.Invalid)
            {
                Logger.WriteLine("Can't sign up user. Provided email was invalid.");
                throw new ArgumentException("Provided email was invalid.", nameof(email));
            }

            if (Validate.Password(password) == ValidationResponse.Invalid)
            {
                Logger.WriteLine("Can't sign up user. Provided password was invalid.");
                throw new ArgumentException("Provided password was invalid.", nameof(password));
            }

            User user;

            try
            {
                user = new User(userName, email, password);
            }
            catch(ArgumentException)
            {
                Logger.WriteLine("Can't sign up user. Provided parameters are invalid.");
                throw;
            }

            Logger.WriteLine("Serializing new user to JSON string.");

            var postBody = JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            try
            {
                Logger.WriteLine("Saving new user to database with REST API.");
                var response = await Client.PostAsync($"{Controller}/signup", new StringContent(postBody, Encoding.UTF8, "application/json")).ConfigureAwait(false);

                if (response == null)
                {
                    Logger.WriteLine("The sign up was unsuccessful. No response was given.");
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    Logger.WriteLine($"The sign up was unsuccessful. Response code: {response.StatusCode}.");
                    return null;
                }

                Logger.WriteLine($"The sign up was successful (Response code: {response.StatusCode}).");
                return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
            }
            catch(HttpRequestException e)
            {
                Logger.WriteLine(e.Message);
                return null;
            }
        }
    }
}
