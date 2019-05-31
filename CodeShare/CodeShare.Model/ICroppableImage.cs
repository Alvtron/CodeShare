// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="ICroppableImage.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace CodeShare.Model
{
    /// <summary>
    /// Interface ICroppableImage
    /// Implements the <see cref="CodeShare.Model.IWebImage" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.IWebImage" />
    public interface ICroppableImage : IWebImage
    {
        /// <summary>
        /// Gets or sets the crop.
        /// </summary>
        /// <value>The crop.</value>
        Crop Crop { get; set; }
        /// <summary>
        /// Creates the crop.
        /// </summary>
        /// <param name="aspectRatio">The aspect ratio.</param>
        void CreateCrop(double aspectRatio);
    }
}