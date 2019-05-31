// ***********************************************************************
// Assembly         : CodeShare.WebApi
// Author           : Thomas Angeland
// Created          : 05-27-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="QuestionRatingCollectionController.cs" company="CodeShare.WebApi">
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
    /// Class QuestionRatingsController.
    /// Implements the <see cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.QuestionRatingCollection}" />
    /// </summary>
    /// <seealso cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.QuestionRatingCollection}" />
    [Route("api/questionratingcollections")]
    [ApiController]
    public class QuestionRatingsController : EntityController<QuestionRatingCollection>
    {
        /// <summary>
        /// Gets the database set.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>DbSet&lt;TEntity&gt;.</returns>
        protected override DbSet<QuestionRatingCollection> GetDatabaseSet(DataContext context)
        {
            return context.QuestionRatings;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected override IQueryable<QuestionRatingCollection> GetEntities(DbSet<QuestionRatingCollection> set)
        {
            return set;
        }

        /// <summary>
        /// Gets the navigational entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected override IQueryable<QuestionRatingCollection> GetNavigationalEntities(DbSet<QuestionRatingCollection> set)
        {
            return set
                .Include(c => c.Question)
                .Include(c => c.Ratings);
        }

        /// <summary>
        /// Called when [put].
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        /// <param name="existingEntity">The existing entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPut(QuestionRatingCollection newEntity, QuestionRatingCollection existingEntity, DataContext context)
        {
            UpdateEntities(newEntity.Ratings, existingEntity.Ratings, context);
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPost(QuestionRatingCollection entity, DataContext context)
        {

        }

        /// <summary>
        /// Called when [delete].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnDelete(QuestionRatingCollection entity, DataContext context)
        {

        }
    }
}