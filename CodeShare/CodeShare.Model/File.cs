// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="File.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    /// <summary>
    /// Class File.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// Implements the <see cref="CodeShare.Model.IFile" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    /// <seealso cref="CodeShare.Model.IFile" />
    public class File : Entity, IFile
    {
        /// <summary>
        /// The data
        /// </summary>
        private string _data;
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string Data
        {
            get => _data;
            set => SetField(ref _data, value);
        }

        /// <summary>
        /// The name
        /// </summary>
        private string _name;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        /// <summary>
        /// The extension
        /// </summary>
        private string _extension;
        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>The extension.</value>
        public string Extension
        {
            get => _extension;
            set => SetField(ref _extension, value);
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        [NotMapped, JsonIgnore]
        public string FullName => $"{Name}{Extension}";
        /// <summary>
        /// Gets the lines.
        /// </summary>
        /// <value>The lines.</value>
        [NotMapped, JsonIgnore]
        public int Lines => Data?.Split('\n').Length ?? 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        public File() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="name">The name.</param>
        /// <param name="extension">The extension.</param>
        /// <exception cref="ArgumentNullException">
        /// data
        /// or
        /// name
        /// or
        /// extension
        /// </exception>
        public File(string data, string name, string extension)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Extension = extension ?? throw new ArgumentNullException(nameof(extension));
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() => FullName;
    }
}
