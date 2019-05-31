// ***********************************************************************
// Assembly         : CodeShare.RestApi
// Author           : Thomas Angeland
// Created          : 01-31-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-27-2019
// ***********************************************************************
// <copyright file="RestApiService.cs" company="CodeShare.RestApi">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using CodeShare.Model;
using System.Net.Http.Headers;
using CodeShare.Extensions;
using CodeShare.Utilities;

namespace CodeShare.RestApi
{
    /// <summary>
    /// Class RestApiService.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class RestApiService<T> where T : class, IEntity
    {
        /// <summary>
        /// The base URI
        /// </summary>
        private const string BaseUri = @"http://localhost:50214/api/";

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <returns>HttpClient.</returns>
        public static HttpClient CreateClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(BaseUri)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        private static string Type => typeof(T).Name.ToLower();
        /// <summary>
        /// Gets the controller.
        /// </summary>
        /// <value>The controller.</value>
        private static string Controller => $"{Type}s";

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>Task&lt;T[]&gt;.</returns>
        public static async Task<T[]> Get()
        {
            Logger.WriteLine($"Retrieving all {Type}s from the database...");
            
            try
            {
                using (var client = CreateClient())
                {
                    var json = await client.GetStringAsync(Controller).ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<T[]>(json);
                }
            }
            catch (HttpRequestException httpException)
            {
                Logger.WriteLine($"Could not retrieve {Type}s from the REST API.");
                Logger.WriteLine(httpException.Message);
                return default;
            }
            catch(JsonSerializationException jsonException)
            {
                Logger.WriteLine($"Could not retrieve {Type}s from the REST API.");
                Logger.WriteLine(jsonException.Message);
                return default;
            }
        }

        /// <summary>
        /// Gets the specified uid.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public static async Task<T> Get(Guid uid)
        {
            Logger.WriteLine($"Retrieving {Type} {uid} from the database...");

            if (uid == Guid.Empty)
            {
                Logger.WriteLine($"Provided uid was empty.");
                return default;
            }

            try
            {
                using (var client = CreateClient())
                {
                    var json = await client.GetStringAsync($"{Controller}/{uid}").ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (HttpRequestException exception)
            {
                Logger.WriteLine($"Could not retrieve {Type} {uid} from the REST API.");
                Logger.WriteLine(exception.Message);
                return default;
            }
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="uid">The uid.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task<bool> Update(T entity, Guid uid)
        {
            Logger.WriteLine($"Updating {Type} in the database...");

            if (entity == null)
            {
                Logger.WriteLine($"Provided {Type} entity was empty.");
                return false;
            }
            if (uid == Guid.Empty)
            {
                Logger.WriteLine("Provided uid was empty.");
                return false;
            }

            Logger.WriteLine($"Serializing {Type} to Json string.");

            var postBody = JsonConvert.SerializeObject(entity, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            using (var client = CreateClient())
            {
                var response = await client
                    .PutAsync($"{Controller}/{uid}", new StringContent(postBody, Encoding.UTF8, "application/json"))
                    .ConfigureAwait(false);

                Logger.WriteLine(response.IsSuccessStatusCode
                    ? $"{Type} was successfully updated in the database. Response code: {response.StatusCode}."
                    : $"{Type} was not updated in the database. Response code: {response.StatusCode}.");

                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task<bool> Update(T entity, string propertyName)
        {
            Logger.WriteLine($"Updating {Type} in the database...");

            if (entity == null)
            {
                Logger.WriteLine($"Provided {Type} entity was empty.");
                return false;
            }
            if (entity.Uid == Guid.Empty)
            {
                Logger.WriteLine($"Provided {nameof(entity.Uid)} was empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                Logger.WriteLine($"Provided {nameof(propertyName)} was empty.");
                return false;
            }
            var patchObject = new PatchObject(entity, propertyName);

            Logger.WriteLine($"Serializing {patchObject.GetType().Name} to Json string.");

            var postBody = JsonConvert.SerializeObject(patchObject, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            using (var client = CreateClient())
            {
                var response = await client
                    .PatchAsync(Controller, new StringContent(postBody, Encoding.UTF8, "application/json"))
                    .ConfigureAwait(false);

                Logger.WriteLine(response.IsSuccessStatusCode
                    ? $"{Type} was successfully updated in the database. Response code: {response.StatusCode}."
                    : $"{Type} was not updated in the database. Response code: {response.StatusCode}.");

                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task<bool> Add(T item)
        {
            if (item == null)
            {
                Logger.WriteLine($"Provided {Type} entity was empty.");
                return false;
            }

            Logger.WriteLine($"Serializing {Type} to Json string.");

            var postBody = JsonConvert.SerializeObject(item, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            Logger.WriteLine($"Adding {Type} to the database...");
            using (var client = CreateClient())
            {
                var response = await client
                    .PostAsync(Controller, new StringContent(postBody, Encoding.UTF8, "application/json"))
                    .ConfigureAwait(false);

                Logger.WriteLine(response.IsSuccessStatusCode
                    ? $"{Type} was successfully added to the database. Response code: {response.StatusCode}."
                    : $"{Type} was not added to the database. Response code: {response.StatusCode}.");

                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Deletes the specified uid.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task<bool> Delete(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                Logger.WriteLine("Provided uid was empty.");
                return false;
            }

            Logger.WriteLine($"Deleting {Type} from the database...");
            using (var client = CreateClient())
            {
                var response = await client.DeleteAsync($"{Controller}/{uid}").ConfigureAwait(false);

                Logger.WriteLine(response.IsSuccessStatusCode
                    ? $"{Type} was successfully deleted from the database. Response code: {response.StatusCode}."
                    : $"{Type} was not deleted from the database. Response code: {response.StatusCode}.");

                return response.IsSuccessStatusCode;
            }
        }
    }
}