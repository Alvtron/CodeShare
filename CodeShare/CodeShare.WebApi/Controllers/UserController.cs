// ***********************************************************************
// Assembly         : CodeShare.WebApi
// Author           : Thomas Angeland
// Created          : 05-15-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="UserController.cs" company="CodeShare.WebApi">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using System;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CodeShare.DataAccess;

namespace CodeShare.WebApi.Controllers
{
    /// <summary>
    /// Class UsersController.
    /// Implements the <see cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.User}" />
    /// </summary>
    /// <seealso cref="CodeShare.WebApi.Controllers.EntityController{CodeShare.Model.User}" />
    [Route("api/users")]
    [ApiController]
    public class UsersController : EntityController<User>
    {
        /// <summary>
        /// Gets the database set.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>DbSet&lt;TEntity&gt;.</returns>
        protected override DbSet<User> GetDatabaseSet(DataContext context)
        {
            return context.Users;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected override IQueryable<User> GetEntities(DbSet<User> set)
        {
            return set
                .Include(c => c.Avatar)
                .Include(a => a.Banner)
                .Include(a => a.Codes)
                .Include(a => a.Questions);
        }

        /// <summary>
        /// Gets the navigational entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected override IQueryable<User> GetNavigationalEntities(DbSet<User> set)
        {
            return set
                .Include(e => e.Email)
                .Include(e => e.Color)
                .Include(e => e.Ratings)
                .Include(e => e.SentFriendRequests)
                .ThenInclude(e => e.Confirmer.Avatar)
                .Include(e => e.SentFriendRequests)
                .ThenInclude(e => e.Confirmer.Banner)
                .Include(e => e.ReceivedFriendRequests)
                .ThenInclude(e => e.Requester.Avatar)
                .Include(e => e.ReceivedFriendRequests)
                .ThenInclude(e => e.Requester.Banner)
                .Include(e => e.Codes)
                .ThenInclude(f => f.Banner)
                .Include(e => e.Codes)
                .ThenInclude(f => f.User)
                .Include(e => e.Questions)
                .ThenInclude(f => f.User)
                .Include(e => e.Questions)
                .ThenInclude(f => f.CodeLanguage)
                .Include(e => e.Logs)
                .Include(e => e.Avatar)
                .Include(e => e.Avatars)
                .Include(e => e.Banner)
                .Include(e => e.Banners);
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPost(User entity, DataContext context)
        {
            
        }

        /// <summary>
        /// Called when [put].
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        /// <param name="existingEntity">The existing entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnPut(User newEntity, User existingEntity, DataContext context)
        {
            newEntity.Avatar = null;
            newEntity.Banner = null;
            UpdateEntities(newEntity.Avatars, existingEntity.Avatars, context);
            UpdateEntities(newEntity.Banners, existingEntity.Banners, context);
            UpdateEntities(newEntity.SentFriendRequests, existingEntity.SentFriendRequests, context);
            UpdateEntities(newEntity.ReceivedFriendRequests, existingEntity.ReceivedFriendRequests, context);
            UpdateEntities(newEntity.Logs, existingEntity.Logs, context);
        }

        /// <summary>
        /// Called when [delete].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected override void OnDelete(User entity, DataContext context)
        {
            context.Database.ExecuteSqlCommand("DELETE FROM dbo.Friendships WHERE Friend_A = @uid OR Friend_B = @uid", new SqlParameter("uid", entity.Uid));
        }
    }
}