// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="WebFile.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using CodeShare.Services;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using CodeShare.Utilities;

namespace CodeShare.Model
{
    /// <summary>
    /// Class WebFile.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// Implements the <see cref="CodeShare.Model.IWebFile" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    /// <seealso cref="CodeShare.Model.IWebFile" />
    public abstract class WebFile : Entity, IWebFile
    {
        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>The extension.</value>
        public string Extension { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [NotMapped, JsonIgnore]
        public Uri Url => new Uri($@"{FtpService.RootDirectoryHttp}/{Path}");

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        [NotMapped, JsonIgnore]
        public string Path => $"{Uid.ToString()}{Extension}";

        /// <summary>
        /// Gets a value indicating whether this <see cref="WebFile"/> is exist.
        /// </summary>
        /// <value><c>true</c> if exist; otherwise, <c>false</c>.</value>
        [NotMapped, JsonIgnore]
        public bool Exist => FtpService.Exist(Path);

        /// <summary>
        /// Uploads the specified file in bytes.
        /// </summary>
        /// <param name="fileInBytes">The file in bytes.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Upload(byte[] fileInBytes)
        {
            Logger.WriteLine($"Uploading {fileInBytes.Length} bytes at path '{Path}' to FTP web server...");

            if (fileInBytes.Length == 0)
            {
                Logger.WriteLine($"Failed to upload file: {Path}. No bytes were supplied.");
                return false;
            }
            if (!FtpService.Upload(fileInBytes, Path))
            {
                Logger.WriteLine($"Failed to upload file: {Path}. Bad server response.");
                return false;
            }

            Logger.WriteLine($"The bytes was successfully uploaded as '{Path}'!");

            Updated = DateTime.Now;
            return true;
        }

        /// <summary>
        /// upload as an asynchronous operation.
        /// </summary>
        /// <param name="fileInBytes">The file in bytes.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> UploadAsync(byte[] fileInBytes)
        {
            Logger.WriteLine($"Uploading {fileInBytes.Length} bytes at path '{Path}' to FTP web server...");

            if (fileInBytes.Length == 0)
            {
                Logger.WriteLine($"Failed to upload file: {Path}. No bytes were supplied.");
                return false;
            }
            if (await FtpService.UploadAsync(fileInBytes, Path) == false)
            {
                Logger.WriteLine($"Failed to upload file: {Path}. Bad server response.");
                return false;
            }

            Logger.WriteLine($"The bytes was successfully uploaded as '{Path}'!");

            Updated = DateTime.Now;
            return true;
        }

        /// <summary>
        /// Downloads this instance.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] Download() => FtpService.Download(Path);

        /// <summary>
        /// download as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public async Task<byte[]> DownloadAsync() => await FtpService.DownloadAsync(Path);

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Delete() => FtpService.Delete(Path);

        /// <summary>
        /// delete as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> DeleteAsync() => await FtpService.DeleteAsync(Path);

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => obj is WebFile file && file.Uid.Equals(Uid);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
