﻿using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using CodeShare.Model;
using System.Net.Http.Headers;
using CodeShare.Utilities;
using System.Collections.Generic;

namespace CodeShare.RestApi
{
    /// <summary>
    /// 
    /// </summary>
    public static class RestApiService<T> where T : IEntity
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

        private static string Type => typeof(T).Name.ToLower();
        private static string Controller => $"{Type}s";

        public static async Task<T[]> Get()
        {
            Logger.WriteLine($"Retrieving all {Type}s from the database...");
            
            try
            {
                var json = await Client.GetStringAsync(Controller).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T[]>(json);
            }
            catch (HttpRequestException exception)
            {
                Logger.WriteLine($"Could not retrieve {Type}s from the REST API.");
                Logger.WriteLine(exception.Message);
                return null;
            }
        }

        public static async Task<T> Get(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                Logger.WriteLine($"Provided uid was empty.");
                return default(T);
            }

            Logger.WriteLine($"Retrieving {Type} {uid} from the database...");

            try
            {
                var json = await Client.GetStringAsync($"{Controller}/{uid}").ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (HttpRequestException exception)
            {
                Logger.WriteLine($"Could not retrieve {Type} {uid} from the REST API.");
                Logger.WriteLine(exception.Message);
                return default(T);
            }
        }

        public static async Task<bool> Update(T item, Guid uid)
        {
            if (item == null)
            {
                Logger.WriteLine($"Provided {Type} item was empty.");
                return false;
            }

            if (uid == Guid.Empty)
            {
                Logger.WriteLine("Provided uid was empty.");
                return false;
            }

            Logger.WriteLine($"Serializing {Type} to Json string.");

            var postBody = JsonConvert.SerializeObject(item, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            Logger.WriteLine($"Updating {Type} in the database...");

            var response = await Client.PutAsync($"{Controller}/{uid}", new StringContent(postBody, Encoding.UTF8, "application/json")).ConfigureAwait(false);

            Logger.WriteLine(response.IsSuccessStatusCode
                ? $"{Type} was successfully updated in the database. Response code: {response.StatusCode}."
                : $"{Type} was not updated in the database. Response code: {response.StatusCode}.");

            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> Add(T item)
        {
            if (item == null)
            {
                Logger.WriteLine($"Provided {Type} item was empty.");
                return false;
            }

            Logger.WriteLine($"Serializing {Type} to Json string.");

            var postBody = JsonConvert.SerializeObject(item, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            Logger.WriteLine($"Adding {Type} to the database...");

            var response = await Client.PostAsync(Controller, new StringContent(postBody, Encoding.UTF8, "application/json")).ConfigureAwait(false);

            Logger.WriteLine(response.IsSuccessStatusCode
                ? $"{Type} was successfully added to the database. Response code: {response.StatusCode}."
                : $"{Type} was not added to the database. Response code: {response.StatusCode}.");

            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> Delete(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                Logger.WriteLine("Provided uid was empty.");
                return false;
            }

            Logger.WriteLine($"Deleting {Type} from the database...");

            var response = await Client.DeleteAsync($"{Controller}/{uid}").ConfigureAwait(false);

            Logger.WriteLine(response.IsSuccessStatusCode
                ? $"{Type} was successfully deleted from the database. Response code: {response.StatusCode}."
                : $"{Type} was not deleted from the database. Response code: {response.StatusCode}.");

            return response.IsSuccessStatusCode;
        }
    }
}