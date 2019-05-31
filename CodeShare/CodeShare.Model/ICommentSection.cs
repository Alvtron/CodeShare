// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-23-2019
// ***********************************************************************
// <copyright file="ICommentSection.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace CodeShare.Model
{
    /// <summary>
    /// Interface ICommentSection
    /// </summary>
    public interface ICommentSection
    {
        /// <summary>
        /// Gets or sets the replies.
        /// </summary>
        /// <value>The replies.</value>
        SortedObservableCollection<Comment> Replies { get; set; }
    }
}