// ***********************************************************************
// Assembly         : CodeShare.WebApi
// Author           : Thomas Angeland
// Created          : 05-31-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="FriendshipController.cs" company="CodeShare.WebApi">
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
    /// Class FriendshipController.
    /// Implements the <see cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.Friendship}" />
    /// </summary>
    /// <seealso cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.Friendship}" />
    [Route("api/friendships")]
    [ApiController]
    public class FriendshipController : EntityController<Friendship>
    {
        /// <summary>
        /// Gets the database set.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>DbSet&lt;TEntity&gt;.</returns>
        protected override DbSet<Friendship> GetDatabaseSet(DataContext context)
        {
            return context.Friendships;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected override IQueryable<Friendship> GetEntities(DbSet<Friendship> set)
        {
            return set;
        }

        /// <summary>
        /// Gets the navigational entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected override IQueryable<Friendship> GetNavigationalEntities(DbSet<Friendship> set)
        {
            return set
                .Include(e => e.Requester)
                .Include(e => e.Confirmer);
        }

        /// <summary>
        /// Called when [put].
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        /// <param name="existingEntity">The existing entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPut(Friendship newEntity, Friendship existingEntity, DataContext context)
        {

        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPost(Friendship entity, DataContext context)
        {

        }

        /// <summary>
        /// Called when [delete].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnDelete(Friendship entity, DataContext context)
        {

        }
    }
}