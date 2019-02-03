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
            Logger.WriteLine("Retrieving public key from the Auth Rest Api...");

            try
            {
                var json = await Client.GetStringAsync($"{Controller}/publickey").ConfigureAwait(false);
                return JsonConvert.DeserializeObject<RSAParameters>(json);
            }
            catch (HttpRequestException)
            {
                Logger.WriteLine($"Could not retrieve public key from the REST API.");
                return null;
            }
        }

        public static async Task<User> SignIn(string userName, string password)
        {
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

            Logger.WriteLine("Signing in user to database...");

            var parameters = await GetPublicKey();

            if (!parameters.HasValue)
            {
                Logger.WriteLine($"Can't sign in user {userName}. The client is unable to fetch public key from REST API.");
                return null;
            }

            var encryptedCredential = new EncryptedCredential(userName, password, parameters.Value);

            var postBody = JsonConvert.SerializeObject(encryptedCredential, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            Logger.WriteLine("Sending user sign in credentials (encrypted) to REST API.");
            var response = await Client.PostAsync($"{Controller}/signin", new StringContent(postBody, Encoding.UTF8, "application/json")).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                Logger.WriteLine($"The sign in was unsuccessful. Response code: {response.StatusCode}.");
                return null;
            }

            Logger.WriteLine($"The sign in was successful. Response code: {response.StatusCode}.");

            return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<User> SignUp(string userName, string email, string password)
        {
            if (Validate.UserName(userName) == ValidationResponse.Invalid)
            {
                Logger.WriteLine("Can't sign up user. Provided user name was invalid.");
                throw new ArgumentException("Provided user name was invalid.", "userName");
            }

            if (Validate.Email(email) == ValidationResponse.Invalid)
            {
                Logger.WriteLine("Can't sign up user. Provided email was invalid.");
                throw new ArgumentException("Provided email was invalid.", "email");
            }

            if (Validate.Password(password) == ValidationResponse.Invalid)
            {
                Logger.WriteLine("Can't sign up user. Provided password was invalid.");
                throw new ArgumentException("Provided password was invalid.", "password");
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

            Logger.WriteLine("Searlizing new user to JSON string.");

            var postBody = JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            Logger.WriteLine("Saving new user to database with REST API.");
            var response = await Client.PostAsync($"{Controller}/signup", new StringContent(postBody, Encoding.UTF8, "application/json")).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                Logger.WriteLine($"The sign up was unsuccessful. Response code: {response.StatusCode}.");
                return null;
            }

            Logger.WriteLine($"The sign up was successful. Response code: {response.StatusCode}.");

            return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
        }
    }
}
