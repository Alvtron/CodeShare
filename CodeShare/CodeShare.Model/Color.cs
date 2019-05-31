// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="Color.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace CodeShare.Model
{
    /// <summary>
    /// Class Color.
    /// </summary>
    public class Color
    {
        /// <summary>
        /// Gets or sets the r.
        /// </summary>
        /// <value>The r.</value>
        public byte R { get; set; } = 255;
        /// <summary>
        /// Gets or sets the g.
        /// </summary>
        /// <value>The g.</value>
        public byte G { get; set; } = 255;
        /// <summary>
        /// Gets or sets the b.
        /// </summary>
        /// <value>The b.</value>
        public byte B { get; set; } = 255;
        /// <summary>
        /// Gets or sets a.
        /// </summary>
        /// <value>a.</value>
        public byte A { get; set; } = 255;

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        public Color() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="g">The g.</param>
        /// <param name="b">The b.</param>
        /// <param name="a">a.</param>
        public Color(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"R:{R} G:{G} B:{B} A:{A}";
        }
    }
}
