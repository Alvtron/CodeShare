// ***********************************************************************
// Assembly         : CodeShare.WebApi
// Author           : Thomas Angeland
// Created          : 05-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="CommentController.cs" company="CodeShare.WebApi">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using CodeShare.DataAccess;

namespace CodeShare.WebApi.Controllers
{
    /// <summary>
    /// Class CommentController.
    /// Implements the <see cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.Comment}" />
    /// </summary>
    /// <seealso cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.Comment}" />
    [Route("api/comments")]
    [ApiController]
    public class CommentController : EntityController<Comment>
    {
        /// <summary>
        /// Gets the database set.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>DbSet&lt;Comment&gt;.</returns>
        protected override DbSet<Comment> GetDatabaseSet(DataContext context)
        {
            return context.Comments;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;Comment&gt;.</returns>
        protected override IQueryable<Comment> GetEntities(DbSet<Comment> set)
        {
            return set;
        }

        /// <summary>
        /// Gets the navigational entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;Comment&gt;.</returns>
        protected override IQueryable<Comment> GetNavigationalEntities(DbSet<Comment> set)
        {
            return set
                .Include(c => c.User)
                .Include(c => c.User.Avatar)
                .Include(c => c.User.Banner)
                .Include(e => e.Logs)
                .Include(c => c.RatingCollection)
                .ThenInclude(c => c.Ratings)
                .Include(c => c.Replies)
                .ThenInclude(comment => comment.User.Avatar)
                .Include(c => c.Replies)
                .ThenInclude(comment => comment.User.Banner)
                .Include(c => c.Replies)
                .ThenInclude(comment => comment.RatingCollection);
        }

        /// <summary>
        /// Called when [put].
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        /// <param name="existingEntity">The existing entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPut(Comment newEntity, Comment existingEntity, DataContext context)
        {
            UpdateEntities(newEntity.RatingCollection.Ratings, existingEntity.RatingCollection.Ratings, context);
            UpdateEntities(newEntity.Replies, existingEntity.Replies, context);
            UpdateEntities(newEntity.Logs, existingEntity.Logs, context);
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPost(Comment entity, DataContext context)
        {

        }

        /// <summary>
        /// Called when [delete].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnDelete(Comment entity, DataContext context)
        {

        }
    }
}