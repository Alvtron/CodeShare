// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="IWebImage.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace CodeShare.Model
{
    /// <summary>
    /// Interface IWebImage
    /// Implements the <see cref="CodeShare.Model.IWebFile" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.IWebFile" />
    public interface IWebImage : IWebFile
    {
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        int Height { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        int Width { get; set; }
        /// <summary>
        /// Gets the aspect ratio.
        /// </summary>
        /// <value>The aspect ratio.</value>
        double AspectRatio { get; }
    }
}