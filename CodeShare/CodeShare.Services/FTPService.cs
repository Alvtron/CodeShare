// ***********************************************************************
// Assembly         : CodeShare.Services
// Author           : Thomas Angeland
// Created          : 01-30-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 01-31-2019
// ***********************************************************************
// <copyright file="FTPService.cs" company="CodeShare.Services">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Threading.Tasks;

namespace CodeShare.Services
{
    /// <summary>
    /// Class FtpService.
    /// </summary>
    public static class FtpService
    {
        /// <summary>
        /// The client
        /// </summary>
        private static readonly FtpClient Client = new FtpClient(RootDirectoryFtp, "r3dcraft.net", "#Alvtron1");

        /// <summary>
        /// The root directory HTTP
        /// </summary>
        public const string RootDirectoryHttp = @"https://r3dcraft.net/CodeShare";

        /// <summary>
        /// The root directory FTP
        /// </summary>
        public const string RootDirectoryFtp = @"ftp://ftp.r3dcraft.net/CodeShare";

        /// <summary>
        /// Uploads the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Upload(byte[] file, string path) => Client.Upload(file, path);

        /// <summary>
        /// upload as an asynchronous operation.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="path">The path.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task<bool> UploadAsync(byte[] file, string path) => await Client.UploadAsync(file, path);

        /// <summary>
        /// Downloads the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] Download(string path) => Client.Download(path);

        /// <summary>
        /// download as an asynchronous operation.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public static async Task<byte[]> DownloadAsync(string path) => await Client.DownloadAsync(path);

        /// <summary>
        /// Deletes the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Delete(string path) => Client.Delete(path);

        /// <summary>
        /// delete as an asynchronous operation.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task<bool> DeleteAsync(string path) => await Client.DeleteAsync(path);

        /// <summary>
        /// Deletes all.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool DeleteAll() => Client.DeleteAll();

        /// <summary>
        /// delete all as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task<bool> DeleteAllAsync() => await Client.DeleteAllAsync();

        /// <summary>
        /// Exists the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Exist(string path) => Client.FileExists(path);

        /// <summary>
        /// exist as an asynchronous operation.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task<bool> ExistAsync(string path) => await Client.FileExistsAsync(path);
    }
}
