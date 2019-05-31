// ***********************************************************************
// Assembly         : CodeShare.WebApi
// Author           : Thomas Angeland
// Created          : 05-15-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="CodeController.cs" company="CodeShare.WebApi">
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
using Microsoft.AspNetCore.Hosting.Internal;

namespace CodeShare.WebApi.Controllers
{
    /// <summary>
    /// Class CodeController.
    /// Implements the <see cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.Code}" />
    /// </summary>
    /// <seealso cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.Code}" />
    [Route("api/codes")]
    [ApiController]
    public class CodeController : EntityController<Code>
    {
        /// <summary>
        /// Gets the database set.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>DbSet&lt;Code&gt;.</returns>
        protected override DbSet<Code> GetDatabaseSet(DataContext context)
        {
            return context.Codes;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;Code&gt;.</returns>
        protected override IQueryable<Code> GetEntities(DbSet<Code> set)
        {
            return set
                .Include(e => e.Banner)
                .Include(e => e.User.Avatar);
        }

        /// <summary>
        /// Gets the navigational entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;Code&gt;.</returns>
        protected override IQueryable<Code> GetNavigationalEntities(DbSet<Code> set)
        {
            return set
                .Include(e => e.Banner)
                .Include(e => e.Banners)
                .Include(e => e.Screenshots)
                .Include(e => e.Videos)
                .Include(e => e.User)
                .Include(e => e.User.Avatar)
                .Include(e => e.User.Banner)
                .Include(e => e.Files)
                .ThenInclude(f => f.CodeLanguage)
                .Include(e => e.Logs)
                .Include(e => e.RatingCollection)
                .ThenInclude(e => e.Ratings)
                .Include(e => e.CommentSection)
                .ThenInclude(e => e.Replies);
        }

        /// <summary>
        /// Called when [put].
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        /// <param name="existingEntity">The existing entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPut(Code newEntity, Code existingEntity, DataContext context)
        {
            newEntity.Banner = null;
            UpdateEntities(newEntity.Banners, existingEntity.Banners, context);
            UpdateEntities(newEntity.Screenshots, existingEntity.Screenshots, context);
            UpdateEntities(newEntity.Logs, existingEntity.Logs, context);
            UpdateEntities(newEntity.Files, existingEntity.Files, context);
            UpdateEntities(newEntity.Videos, existingEntity.Videos, context);
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPost(Code entity, DataContext context)
        {

        }

        /// <summary>
        /// Called when [delete].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnDelete(Code entity, DataContext context)
        {

        }
    }
}