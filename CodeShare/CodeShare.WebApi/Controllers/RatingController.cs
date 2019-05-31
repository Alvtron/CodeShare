// ***********************************************************************
// Assembly         : CodeShare.WebApi
// Author           : Thomas Angeland
// Created          : 05-24-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="RatingController.cs" company="CodeShare.WebApi">
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
    /// Class RatingController.
    /// Implements the <see cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.Rating}" />
    /// </summary>
    /// <seealso cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.Rating}" />
    [Route("api/ratings")]
    [ApiController]
    public class RatingController : EntityController<Rating>
    {
        /// <summary>
        /// Gets the database set.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>DbSet&lt;TEntity&gt;.</returns>
        protected override DbSet<Rating> GetDatabaseSet(DataContext context)
        {
            return context.Ratings;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected override IQueryable<Rating> GetEntities(DbSet<Rating> set)
        {
            return set;
        }

        /// <summary>
        /// Gets the navigational entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected override IQueryable<Rating> GetNavigationalEntities(DbSet<Rating> set)
        {
            return set
                .Include(c => c.User)
                .ThenInclude(c => c.Avatar)
                .Include(c => c.User)
                .ThenInclude(c => c.Banner);
        }

        /// <summary>
        /// Called when [put].
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        /// <param name="existingEntity">The existing entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPut(Rating newEntity, Rating existingEntity, DataContext context)
        {

        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPost(Rating entity, DataContext context)
        {

        }

        /// <summary>
        /// Called when [delete].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnDelete(Rating entity, DataContext context)
        {

        }
    }
}