// ***********************************************************************
// Assembly         : CodeShare.Extensions
// Author           : Thomas Angeland
// Created          : 05-27-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-27-2019
// ***********************************************************************
// <copyright file="HttpClientExtensions.cs" company="CodeShare.Extensions">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CodeShare.Extensions
{
    /// <summary>
    /// Class HttpClientExtensions.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Patch as an asynchronous operation.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="httpContent">HTTP content.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent httpContent)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = httpContent
            };

            var response = new HttpResponseMessage();
            try
            {
                response = await client.SendAsync(request);
            }
            catch (TaskCanceledException e)
            {
                Debug.WriteLine(e);
            }

            return response;
        }
    }
}
