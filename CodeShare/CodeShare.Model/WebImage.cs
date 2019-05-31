// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="WebImage.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CodeShare.Model
{
    /// <summary>
    /// Class WebImage.
    /// Implements the <see cref="CodeShare.Model.WebFile" />
    /// Implements the <see cref="CodeShare.Model.ICroppableImage" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.WebFile" />
    /// <seealso cref="CodeShare.Model.ICroppableImage" />
    public abstract class WebImage : WebFile, ICroppableImage
    {
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }
        /// <summary>
        /// The crop
        /// </summary>
        private Crop _crop;
        /// <summary>
        /// Gets or sets the crop.
        /// </summary>
        /// <value>The crop.</value>
        public virtual Crop Crop
        {
            get => _crop;
            set => SetField(ref _crop, value);
        }
        /// <summary>
        /// Gets the aspect ratio.
        /// </summary>
        /// <value>The aspect ratio.</value>
        [NotMapped, JsonIgnore] public double AspectRatio => Width / (double)Height;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebImage"/> class.
        /// </summary>
        protected WebImage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebImage"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="cropAspectRatio">The crop aspect ratio.</param>
        /// <param name="fileInBytes">The file in bytes.</param>
        /// <param name="extension">The extension.</param>
        protected WebImage(int width, int height, double cropAspectRatio, byte[] fileInBytes, string extension)
        : this()
        {
            Extension = extension;

            if (!Exist)
            {
                Upload(fileInBytes);
            }

            Width = width;
            Height = height;

            CreateCrop(cropAspectRatio);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebImage"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="fileInBytes">The file in bytes.</param>
        /// <param name="extension">The extension.</param>
        protected WebImage(int width, int height, byte[] fileInBytes, string extension)
            : this(width, height, width / (double)height, fileInBytes, extension)
        {
        }

        /// <summary>
        /// Creates the crop.
        /// </summary>
        /// <param name="aspectRatio">The aspect ratio.</param>
        public void CreateCrop(double aspectRatio)
        {
            var croppedWidth = Width * 0.9;
            var croppedHeight = Height * 0.9;

            var suggestedHeight = croppedWidth / aspectRatio;
            var suggestedWidth = croppedHeight * aspectRatio;

            if (suggestedHeight <= croppedHeight)
            {
                Crop = new Crop(Width / 2, Height / 2, (int)croppedWidth, (int)suggestedHeight, aspectRatio);
            }
            else
            {
                Crop = new Crop(Width / 2, Height / 2, (int)suggestedWidth, (int)croppedHeight, aspectRatio);
            }
        }
    }
}