// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="Crop.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace CodeShare.Model
{
    /// <summary>
    /// Class Crop.
    /// Implements the <see cref="CodeShare.Model.ObservableObject" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.ObservableObject" />
    public class Crop : ObservableObject
    {
        /// <summary>
        /// The x
        /// </summary>
        private int _x;
        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>The x.</value>
        public int X
        {
            get => _x;
            set => SetField(ref _x, value);
        }
        /// <summary>
        /// The y
        /// </summary>
        private int _y;
        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
        public int Y
        {
            get => _y;
            set => SetField(ref _y, value);
        }
        /// <summary>
        /// The width
        /// </summary>
        private int _width;
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get => _width;
            set => SetField(ref _width, value);
        }
        /// <summary>
        /// The height
        /// </summary>
        private int _height;
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get => _height;
            set => SetField(ref _height, value);
        }

        /// <summary>
        /// Gets or sets the aspect ratio.
        /// </summary>
        /// <value>The aspect ratio.</value>
        public double AspectRatio { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crop"/> class.
        /// </summary>
        public Crop() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crop"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="aspectRatio">The aspect ratio.</param>
        public Crop(int x, int y, int width, int height, double aspectRatio)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            AspectRatio = aspectRatio;
        }
    }
}
