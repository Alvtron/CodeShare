using Newtonsoft.Json;
using CodeShare.Model;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CodeShare.Uwp.DataSource
{
    public static class AuthDataSource
    {
        private const string BaseUri = @"http://localhost:58669/api/";

        private static HttpClient _client;
        public static HttpClient Client
        {
            get
            {
                if (_client != null) return _client;

                _client = new HttpClient();
                _client.BaseAddress = new Uri(BaseUri);
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return _client;
            }
        }

        /// <summary>
        /// The controller
        /// </summary>
        private const string Controller = "auth";

        private static async Task<RSAParameters?> GetPublicKey()
        {
            Debug.WriteLine("Retrieving public key from the Auth Rest Api...");

            try
            {
                var json = await Client.GetStringAsync($"{Controller}/publickey").ConfigureAwait(false);
                return JsonConvert.DeserializeObject<RSAParameters>(json);
            }
            catch (HttpRequestException)
            {
                Debug.WriteLine($"Could not retrieve public key from the REST API.");
                return null;
            }
        }

        public static async Task<User> SignIn(string userName, string password)
        {
            if (userName == null)
            {
                Debug.WriteLine("Can't sign in user. Provided user name was empty.");
                return null;
            }

            if (password == null)
            {
                Debug.WriteLine($"Can't sign in user {userName}. Provided password was empty.");
                return null;
            }

            Debug.WriteLine("Signing in user to database...");

            var parameters = await GetPublicKey();

            if (!parameters.HasValue)
            {
                Debug.WriteLine($"Can't sign in user {userName}. The client is unable to fetch public key from REST API.");
                return null;
            }

            var encryptedCredential = new EncryptedCredential(userName, password, parameters.Value);

            var postBody = JsonConvert.SerializeObject(encryptedCredential, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            Debug.WriteLine("Sending user sign in credentials (encrypted) to REST API.");
            var response = await Client.PostAsync($"{Controller}/signin", new StringContent(postBody, Encoding.UTF8, "application/json")).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"The sign in was unsuccessful. Response code: {response.StatusCode}.");
                return null;
            }

            Debug.WriteLine($"The sign in was successful. Response code: {response.StatusCode}.");

            return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<User> SignUp(User user)
        {
            if (user == null)
            {
                Debug.WriteLine("Can't sign up user. Provided user was empty.");
                return null;
            }

            if (!user.Valid)
            {
                Debug.WriteLine("Can't sign up user. Provided user was invalid.");
                return null;
            }

            Debug.WriteLine("Signing up user to database...");

            var postBody = JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            Debug.WriteLine("Sending user to REST API.");
            var response = await Client.PostAsync($"{Controller}/signup", new StringContent(postBody, Encoding.UTF8, "application/json")).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"The sign up was unsuccessful. Response code: {response.StatusCode}.");
                return null;
            }

            Debug.WriteLine($"The sign up was successful. Response code: {response.StatusCode}.");

            return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
        }
    }
}
