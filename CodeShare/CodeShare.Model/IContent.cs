// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-22-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="IContent.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace CodeShare.Model
{
    /// <summary>
    /// Interface IContent
    /// Implements the <see cref="CodeShare.Model.IEntity" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.IEntity" />
    public interface IContent : IEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the views.
        /// </summary>
        /// <value>The views.</value>
        int Views { get; set; }
        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        string Type { get; }
    }
}
