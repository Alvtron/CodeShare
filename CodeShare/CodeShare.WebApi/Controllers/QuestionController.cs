// ***********************************************************************
// Assembly         : CodeShare.WebApi
// Author           : Thomas Angeland
// Created          : 05-15-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="QuestionController.cs" company="CodeShare.WebApi">
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
    /// Class QuestionController.
    /// Implements the <see cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.Question}" />
    /// </summary>
    /// <seealso cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.Question}" />
    [Route("api/questions")]
    [ApiController]
    public class QuestionController : EntityController<Question>
    {
        /// <summary>
        /// Gets the database set.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>DbSet&lt;TEntity&gt;.</returns>
        protected override DbSet<Question> GetDatabaseSet(DataContext context)
        {
            return context.Questions;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected override IQueryable<Question> GetEntities(DbSet<Question> set)
        {
            return set
                .Include(q => q.User.Avatar)
                .Include(q => q.User.Banner)
                .Include(q => q.CodeLanguage);
        }

        /// <summary>
        /// Gets the navigational entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected override IQueryable<Question> GetNavigationalEntities(DbSet<Question> set)
        {
            return set
                .Include(q => q.User.Avatar)
                .Include(q => q.User.Banner)
                .Include(e => e.Logs)
                .Include(q => q.Solution.User.Avatar)
                .Include(q => q.Solution.User.Banner)
                .Include(q => q.CodeLanguage)
                .Include(e => e.Screenshots)
                .Include(c => c.RatingCollection)
                .ThenInclude(c => c.Ratings)
                .Include(c => c.CommentSection)
                .ThenInclude(cs => cs.Replies);
        }

        /// <summary>
        /// Called when [put].
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        /// <param name="existingEntity">The existing entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPut(Question newEntity, Question existingEntity, DataContext context)
        {
            UpdateEntities(newEntity.Screenshots, existingEntity.Screenshots, context);
            UpdateEntities(newEntity.Logs, existingEntity.Logs, context);
            UpdateEntities(newEntity.Videos, existingEntity.Videos, context);
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPost(Question entity, DataContext context)
        {

        }

        /// <summary>
        /// Called when [delete].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnDelete(Question entity, DataContext context)
        {

        }
    }
}