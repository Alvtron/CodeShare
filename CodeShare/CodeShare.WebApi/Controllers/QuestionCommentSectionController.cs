// ***********************************************************************
// Assembly         : CodeShare.WebApi
// Author           : Thomas Angeland
// Created          : 05-27-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="QuestionCommentSectionController.cs" company="CodeShare.WebApi">
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
    /// Class QuestionCommentSectionController.
    /// Implements the <see cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.QuestionCommentSection}" />
    /// </summary>
    /// <seealso cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.QuestionCommentSection}" />
    [Route("api/questioncommentsections")]
    [ApiController]
    public class QuestionCommentSectionController : EntityController<QuestionCommentSection>
    {
        /// <summary>
        /// Gets the database set.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>DbSet&lt;TEntity&gt;.</returns>
        protected override DbSet<QuestionCommentSection> GetDatabaseSet(DataContext context)
        {
            return context.QuestionCommentSections;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected override IQueryable<QuestionCommentSection> GetEntities(DbSet<QuestionCommentSection> set)
        {
            return set;
        }

        /// <summary>
        /// Gets the navigational entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected override IQueryable<QuestionCommentSection> GetNavigationalEntities(DbSet<QuestionCommentSection> set)
        {
            return set
                .Include(c => c.Question)
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
        protected override void OnPut(QuestionCommentSection newEntity, QuestionCommentSection existingEntity, DataContext context)
        {
            UpdateEntities(newEntity.Replies, existingEntity.Replies, context);
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPost(QuestionCommentSection entity, DataContext context)
        {

        }

        /// <summary>
        /// Called when [delete].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnDelete(QuestionCommentSection entity, DataContext context)
        {

        }
    }
}