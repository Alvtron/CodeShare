// ***********************************************************************
// Assembly         : CodeShare.Services
// Author           : Thomas Angeland
// Created          : 01-30-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="FTPClient.cs" company="CodeShare.Services">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CodeShare.Services
{

    /// <summary>
    /// Class FtpClient.
    /// </summary>
    internal class FtpClient
    {
        /// <summary>
        /// The root directory
        /// </summary>
        public readonly string RootDirectory;
        /// <summary>
        /// The credential
        /// </summary>
        private readonly NetworkCredential _credential;

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpClient"/> class.
        /// </summary>
        /// <param name="rootDirectory">The root directory.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="ArgumentNullException">rootDirectory</exception>
        public FtpClient(string rootDirectory, string userName, string password)
        {
            RootDirectory = rootDirectory ?? throw new ArgumentNullException(nameof(rootDirectory));

            _credential = new NetworkCredential(userName, password);
        }

        /// <summary>
        /// Requests the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>FtpWebRequest.</returns>
        private FtpWebRequest Request(string path) => WebRequest.Create(new Uri($"{RootDirectory}/{path}")) as FtpWebRequest;

        /// <summary>
        /// download as an asynchronous operation.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public async Task<byte[]> DownloadAsync(string path)
        {
            var request = Request(path);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = _credential;

            try
            {
                using (var response = await request.GetResponseAsync() as FtpWebResponse)
                {
                    using (var responseStream = response?.GetResponseStream())
                    {
                        if (responseStream == null)
                            return null;

                        Logger.WriteLine($"Download Complete. Status: {response.StatusDescription}");

                        using (var memoryStream = new MemoryStream())
                        {
                            await responseStream.CopyToAsync(memoryStream);
                            return memoryStream.ToArray();
                        }
                    }
                }
            }
            catch(WebException)
            {
                return null;
            }
        }

        /// <summary>
        /// Downloads the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] Download(string path)
        {
            var request = Request(path);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = _credential;

            try
            {
                using (var response = request.GetResponse() as FtpWebResponse)
                {
                    using (var responseStream = response?.GetResponseStream())
                    {
                        if (responseStream == null)
                            return null;

                        Logger.WriteLine($"Download Complete. Status: {response.StatusDescription}");

                        using (var memoryStream = new MemoryStream())
                        {
                            responseStream.CopyTo(memoryStream);
                            return memoryStream.ToArray();
                        }
                    }
                }
            }
            catch(WebException)
            {
                return null;
            }
        }

        /// <summary>
        /// upload as an asynchronous operation.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="path">The path.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> UploadAsync(byte[] file, string path)
        {
            var request = Request(path);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = _credential;
            request.ContentLength = file.Length;

            using (var requestStream = await request.GetRequestStreamAsync())
            {
                await requestStream.WriteAsync(file, 0, file.Length);
            }

            try
            {
                using (var response = await request.GetResponseAsync() as FtpWebResponse)
                {
                    Logger.WriteLine($"Upload File Complete. Status: {response?.StatusDescription}");
                    return true;
                }
            }
            catch (WebException)
            {
                Logger.WriteLine($"Upload file failed.");
                return false;
            }
}

        /// <summary>
        /// Uploads the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Upload(byte[] file, string path)
        {
            var request = Request(path);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = _credential;
            request.ContentLength = file.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(file, 0, file.Length);
            }

            try
            {
                using (var response = request.GetResponse() as FtpWebResponse)
                {
                    Logger.WriteLine($"Upload File Complete. Status: {response?.StatusDescription}");
                    return true;
                }
            }
            catch (WebException)
            {
                Logger.WriteLine($"Upload file failed.");
                return false;
            }
        }

        /// <summary>
        /// Deletes the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Delete(string path)
        {
            var request = Request(path);
            request.Credentials = _credential;
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            try
            {
                using (var response = request.GetResponse() as FtpWebResponse)
                {
                    Logger.WriteLine($"Delete File Complete. Status: {response?.StatusDescription}");
                    return true;
                }
            }
            catch (WebException)
            {
                Logger.WriteLine($"Deletion of file failed.");
                return false;
            }
        }

        /// <summary>
        /// delete as an asynchronous operation.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> DeleteAsync(string path)
        {
            var request = Request(path);
            request.Credentials = _credential;
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            try
            {
                using (var response = await request.GetResponseAsync() as FtpWebResponse)
                {
                    Logger.WriteLine($"Delete File Complete. Status: {response?.StatusDescription}");
                    return true;
                }
            }
            catch (WebException)
            {
                Logger.WriteLine($"Deletion of file failed.");
                return false;
            }
        }

        /// <summary>
        /// Deletes all.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DeleteAll()
        {
            var files = ListFiles();

            if (files.Count == 0)
                return true;

            bool success = false;

            foreach (var file in files)
                success = Delete(file);

            return success;
        }

        /// <summary>
        /// delete all as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> DeleteAllAsync()
        {
            var files = await ListFilesAsync();

            if (files.Count == 0)
                return true;

            bool success = false;

            foreach (var file in files)
                success = await DeleteAsync(file);

            return success;
        }

        public bool CreateDirectory(string path)
        {
            var request = Request(path);

            try
            {
                using (request.GetResponse() as FtpWebResponse)
                {
                    Logger.WriteLine($"Directory {request.RequestUri} already exists.");
                    return true;
                }
            }
            catch (WebException)
            {
                try
                {
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;
                    using (request.GetResponse() as FtpWebResponse)
                    {
                        Logger.WriteLine($"Directory {request.RequestUri} was successfully created.");
                        return true;
                    }
                }
                catch (WebException)
                {
                    Logger.WriteLine($"Something wen't wrong when creating directory {request.RequestUri}.");
                    return false;
                }
            }
        }

        public async Task<bool> CreateDirectoryAsync(string path)
        {
            var request = Request(path);

            try
            {
                using (await request.GetResponseAsync() as FtpWebResponse)
                {
                    Logger.WriteLine($"Directory {request.RequestUri} already exists.");
                    return true;
                }
            }
            catch (WebException)
            {
                try
                {
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;
                    using (await request.GetResponseAsync() as FtpWebResponse)
                    {
                        Logger.WriteLine($"Directory {request.RequestUri} was successfully created.");
                        return true;
                    }
                }
                catch (WebException)
                {
                    Logger.WriteLine($"Something wen't wrong when creating directory {request.RequestUri}.");
                    return false;
                }
            }
        }

        public List<string> ListFiles(string path = "")
        {
            var request = Request(path);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = _credential;

            using (var response = request.GetResponse() as FtpWebResponse)
            {
                if (response == null)
                {
                    return null;
                }

                using (var stream = response.GetResponseStream())
                {
                    if (stream == null)
                    {
                        return null;
                    }

                    using (var reader = new StreamReader(stream ))
                    {
                        var files = new List<string>();
                        var line = reader.ReadLine();
                        while (!string.IsNullOrEmpty(line))
                        {
                            files.Add(line);
                            line = reader.ReadLine();
                        }

                        Logger.WriteLine($"Directory List Complete, status {response.StatusDescription}");

                        return files;
                    }
                }
                
            }
        }

        public async Task<List<string>> ListFilesAsync(string path = "")
        {
            var request = Request(path);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = _credential;

            using (var response = await request.GetResponseAsync() as FtpWebResponse)
            {
                if (response == null)
                {
                    return null;
                }

                using (var stream = response.GetResponseStream())
                {
                    if (stream == null)
                    {
                        return null;
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        var files = new List<string>();
                        var line = await reader.ReadLineAsync();
                        while (!string.IsNullOrEmpty(line))
                        {
                            files.Add(line);
                            line = await reader.ReadLineAsync();
                        }

                        Logger.WriteLine($"Directory List Complete, status {response.StatusDescription}");

                        return files;
                    }
                }
            }
        }

        public bool FileExists(string path)
        {
            var request = Request(path);
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            request.Credentials = _credential;

            try
            {
                using (request.GetResponse() as FtpWebResponse)
                {
                    return true;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }

        public async Task<bool> FileExistsAsync(string path)
        {
            var request = Request(path);
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            request.Credentials = _credential;

            try
            {
                using (await request.GetResponseAsync() as FtpWebResponse)
                {
                    return true;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }
    }
}
