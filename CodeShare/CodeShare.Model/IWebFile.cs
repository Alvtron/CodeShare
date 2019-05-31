// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="IWebFile.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Threading.Tasks;

namespace CodeShare.Model
{
    /// <summary>
    /// Interface IWebFile
    /// </summary>
    public interface IWebFile
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }
        /// <summary>
        /// Gets a value indicating whether this <see cref="IWebFile"/> is exist.
        /// </summary>
        /// <value><c>true</c> if exist; otherwise, <c>false</c>.</value>
        bool Exist { get; }
        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>The extension.</value>
        string Extension { get; set; }
        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        string Path { get; }
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        Uri Url { get; }
        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Delete();
        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> DeleteAsync();
        /// <summary>
        /// Downloads this instance.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        byte[] Download();
        /// <summary>
        /// Downloads the asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        Task<byte[]> DownloadAsync();
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        bool Equals(object obj);
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        int GetHashCode();
        /// <summary>
        /// Uploads the specified file in bytes.
        /// </summary>
        /// <param name="fileInBytes">The file in bytes.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Upload(byte[] fileInBytes);
        /// <summary>
        /// Uploads the asynchronous.
        /// </summary>
        /// <param name="fileInBytes">The file in bytes.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> UploadAsync(byte[] fileInBytes);
    }
}