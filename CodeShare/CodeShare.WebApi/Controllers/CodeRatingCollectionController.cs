// ***********************************************************************
// Assembly         : CodeShare.WebApi
// Author           : Thomas Angeland
// Created          : 05-27-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="CodeRatingCollectionController.cs" company="CodeShare.WebApi">
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
    /// Class CodeRatingsController.
    /// Implements the <see cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.CodeRatingCollection}" />
    /// </summary>
    /// <seealso cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.CodeRatingCollection}" />
    [Route("api/coderatingcollections")]
    [ApiController]
    public class CodeRatingsController : EntityController<CodeRatingCollection>
    {
        /// <summary>
        /// Gets the database set.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>DbSet&lt;CodeRatingCollection&gt;.</returns>
        protected override DbSet<CodeRatingCollection> GetDatabaseSet(DataContext context)
        {
            return context.CodeRatings;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;CodeRatingCollection&gt;.</returns>
        protected override IQueryable<CodeRatingCollection> GetEntities(DbSet<CodeRatingCollection> set)
        {
            return set;
        }

        /// <summary>
        /// Gets the navigational entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;CodeRatingCollection&gt;.</returns>
        protected override IQueryable<CodeRatingCollection> GetNavigationalEntities(DbSet<CodeRatingCollection> set)
        {
            return set
                .Include(c => c.Code)
                .Include(c => c.Ratings);
        }

        /// <summary>
        /// Called when [put].
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        /// <param name="existingEntity">The existing entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPut(CodeRatingCollection newEntity, CodeRatingCollection existingEntity, DataContext context)
        {
            UpdateEntities(newEntity.Ratings, existingEntity.Ratings, context);
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPost(CodeRatingCollection entity, DataContext context)
        {

        }

        /// <summary>
        /// Called when [delete].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnDelete(CodeRatingCollection entity, DataContext context)
        {

        }
    }
}