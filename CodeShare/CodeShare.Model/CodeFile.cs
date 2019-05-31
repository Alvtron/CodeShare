// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-28-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="CodeFile.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class CodeFile.
    /// Implements the <see cref="CodeShare.Model.File" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.File" />
    public class CodeFile : File
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public virtual Code Code { get; set; }
        /// <summary>
        /// Gets or sets the code uid.
        /// </summary>
        /// <value>The code uid.</value>
        public Guid? CodeUid { get; set; }
        /// <summary>
        /// Gets or sets the code language.
        /// </summary>
        /// <value>The code language.</value>
        public virtual CodeLanguage CodeLanguage { get; set; }
        /// <summary>
        /// Gets or sets the code language uid.
        /// </summary>
        /// <value>The code language uid.</value>
        public Guid? CodeLanguageUid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeFile"/> class.
        /// </summary>
        public CodeFile() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeFile"/> class.
        /// </summary>
        /// <param name="codeLanguage">The code language.</param>
        /// <param name="data">The data.</param>
        /// <param name="name">The name.</param>
        /// <param name="extension">The extension.</param>
        public CodeFile(IEntity codeLanguage, string data, string name, string extension)
            : base(data, name, extension)
        {
            CodeLanguageUid = codeLanguage.Uid;
        }
    }
}
