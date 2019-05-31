// ***********************************************************************
// Assembly         : CodeShare.WebApi
// Author           : Thomas Angeland
// Created          : 05-15-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="EntityController.cs" company="CodeShare.WebApi">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq;
using CodeShare.DataAccess;
using CodeShare.Model;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using CodeShare.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CodeShare.WebApi.Controllers
{
    /// <summary>
    /// Class EntityController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// Implements the <see cref="CodeShare.WebApi.Controllers.IController{TEntity}" />
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// <seealso cref="CodeShare.WebApi.Controllers.IController{TEntity}" />
    public abstract class EntityController<TEntity> : ControllerBase, IController<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Gets the database set.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>DbSet&lt;TEntity&gt;.</returns>
        protected abstract DbSet<TEntity> GetDatabaseSet(DataContext context);
        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected abstract IQueryable<TEntity> GetEntities(DbSet<TEntity> set);
        /// <summary>
        /// Gets the navigational entities.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        protected abstract IQueryable<TEntity> GetNavigationalEntities(DbSet<TEntity> set);
        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected abstract void OnPost(TEntity entity, DataContext context);
        /// <summary>
        /// Called when [put].
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        /// <param name="existingEntity">The existing entity.</param>
        /// <param name="context">The context.</param>
        protected abstract void OnPut(TEntity newEntity, TEntity existingEntity, DataContext context);
        /// <summary>
        /// Called when [delete].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        protected abstract void OnDelete(TEntity entity, DataContext context);

        /// <summary>
        /// Gets all entities from database.
        /// </summary>
        /// <returns>ActionResult&lt;IEnumerable&lt;TEntity&gt;&gt;.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<TEntity>> Get()
        {
            using (var context = new DataContext())
            {
                try
                {
                    var entities = GetEntities(GetDatabaseSet(context)).ToList();
                    return Ok(entities);
                }
                catch (SqlException sqlException)
                {
                    return StatusCode(500, sqlException.Message);
                }
            }
        }

        /// <summary>
        /// Gets the entity bound to the specified uid from the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns>ActionResult&lt;TEntity&gt;.</returns>
        [HttpGet("{uid}")]
        public ActionResult<TEntity> Get(Guid uid)
        {
            using (var context = new DataContext())
            {
                try
                {
                    var set = GetDatabaseSet(context);
                    var entities = GetNavigationalEntities(set);
                    var entity = entities.FirstOrDefault(u => u.Uid == uid);

                    if (entity == null)
                    {
                        var message =
                            $"The provided {uid.GetType().Name} parameter did not match any items in the database.";
                        Logger.WriteLine(message);
                        return NotFound(message);
                    }

                    return Ok(entity);
                }
                catch (SqlException sqlException)
                {
                    return StatusCode(500, sqlException.Message);
                }
            }
        }


        /// <summary>
        /// Updates the newEntity to equal newEntity bound to the specified uid.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <param name="entity">The newEntity.</param>
        /// <returns>ActionResult&lt;TEntity&gt;.</returns>
        [HttpPut("{uid}")]
        public ActionResult<TEntity> Put(Guid uid, [FromBody] TEntity entity)
        {
            if (uid == Guid.Empty)
            {
                var message = $"The provided {uid.GetType().Name} parameter is empty! Put denied.";
                Logger.WriteLine(message);
                return BadRequest(message);
            }

            if (entity == null)
            {
                var message = $"The provided {typeof(TEntity).Name} object is empty! Put denied.";
                Logger.WriteLine(message);
                return BadRequest(message);
            }

            if (uid != entity.Uid)
            {
                var message = $"The provided {typeof(TEntity).Name} object does not match the provided key {uid}. Put denied!";
                Logger.WriteLine(message);
                return BadRequest(message);
            }

            using (var context = new DataContext())
            {
                try
                {
                    var set = GetDatabaseSet(context);
                    var entities = GetNavigationalEntities(set);
                    var existingEntity = entities.FirstOrDefault(e => e.Uid.Equals(uid));

                    if (existingEntity == null)
                    {
                        Logger.WriteLine(
                            $"The provided {entity.GetType().Name} object with UID {uid} does not exist in the database. Attempting to add it...");
                        return Post(entity);
                    }
                    UpdateEntity(entity, existingEntity, context);
                    OnPut(entity, existingEntity, context);
                    context.SaveChanges();
                    return Ok(existingEntity);
                }
                catch (DbUpdateException dbUpdateException)
                {
                    Logger.WriteLine(
                        $"The provided {typeof(TEntity).Name} object could not be updated. {dbUpdateException.Message}");
                    return BadRequest(
                        $"The provided {typeof(TEntity).Name} object could not be updated. {dbUpdateException.Message}");
                }
                catch (SqlException sqlException)
                {
                    return StatusCode(500, sqlException.Message);
                }
            }
        }

        /// <summary>
        /// Adds the specified newEntity to the database.
        /// </summary>
        /// <param name="entity">The newEntity.</param>
        /// <returns>ActionResult&lt;TEntity&gt;.</returns>
        [HttpPost]
        public ActionResult<TEntity> Post(TEntity entity)
        {
            if (entity == null)
            {
                return BadRequest($"The provided {typeof(TEntity).Name} object is empty! Post denied.");
            }
            if (Exist(entity.Uid))
            {
                return BadRequest($"The provided {typeof(TEntity).Name} object already exists! Post denied.");
            }

            using (var context = new DataContext())
            {
                try
                {
                    var set = GetDatabaseSet(context);
                    set.Add(entity);
                    OnPost(entity, context);
                    context.SaveChanges();
                }
                catch (DbUpdateException dbUpdateException)
                {
                    Logger.WriteLine(
                        $"The provided {typeof(TEntity).Name} object could not be added. {dbUpdateException.Message}");
                    return BadRequest(
                        $"The provided {typeof(TEntity).Name} object could not be added. {dbUpdateException.Message}");
                }
                catch (SqlException sqlException)
                {
                    return StatusCode(500, sqlException.Message);
                }
                return Ok(entity);
            }
        }

        /// <summary>
        /// Deletes the newEntity bound to the specified uid from the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns>IActionResult.</returns>
        [HttpDelete("{uid}")]
        public IActionResult Delete(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return BadRequest($"The provided {uid.GetType().Name} is empty! Deletion denied.");
            }
            if (!Exist(uid))
            {
                return BadRequest($"The provided {uid.GetType().Name} does not exist in the database! Deletion denied.");
            }

            try
            {
                using (var context = new DataContext())
                {
                    var set = GetDatabaseSet(context);
                    var entities = GetNavigationalEntities(set);
                    var entity = entities.FirstOrDefault(e => e.Uid.Equals(uid));

                    if (entity == null)
                    {
                        Logger.WriteLine($"The provided UID {uid} does not exist in the database.");
                        return BadRequest($"The provided UID {uid} does not exist in the database.");
                    }

                    try
                    {
                        set.Remove(entity);
                        OnDelete(entity, context);

                        context.SaveChanges();
                    }
                    catch (Exception exception)
                    {
                        Logger.WriteLine($"The provided {uid.GetType().Name} could not be deleted! {exception.Message}");
                        return BadRequest($"The provided {uid.GetType().Name} could not be deleted! {exception.Message}");
                    }
                }

                return Ok();
            }
            catch (SqlException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Patches the specified patch object.
        /// </summary>
        /// <param name="patchObject">The patch object.</param>
        /// <returns>ActionResult&lt;TEntity&gt;.</returns>
        [HttpPatch]
        public ActionResult<TEntity> Patch([FromBody] PatchObject patchObject)
        {
            if (patchObject.EntityUid == Guid.Empty)
            {
                Logger.WriteLine($"The provided {patchObject.EntityUid.GetType().Name} is empty! Patch denied.");
                return BadRequest($"The provided {patchObject.EntityUid.GetType().Name} is empty! Patch denied.");
            }
            if (!Exist(patchObject.EntityUid))
            {
                Logger.WriteLine($"The provided {patchObject.EntityUid.GetType().Name} does not exist in the database! Patch denied.");
                return BadRequest($"The provided {patchObject.EntityUid.GetType().Name} does not exist in the database! Patch denied.");
            }

            try
            {
                using (var context = new DataContext())
                {
                    var set = GetDatabaseSet(context);
                    var entities = GetNavigationalEntities(set);
                    var entity = entities.FirstOrDefault(e => e.Uid.Equals(patchObject.EntityUid));

                    if (entity == null)
                    {
                        Logger.WriteLine($"The provided UID {patchObject.EntityUid} does not exist in the database.");
                        return BadRequest($"The provided UID {patchObject.EntityUid} does not exist in the database.");
                    }

                    try
                    {
                        patchObject.ApplyChanges(entity);
                        context.SaveChanges();
                    }
                    catch (InvalidOperationException e)
                    {
                        Logger.WriteLine($"Patch failed. {e.Message}.");
                        return BadRequest($"Patch failed. {e.Message}.");
                    }

                    return Ok(entity);
                }
            }
            catch (SqlException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Checks if the newEntity specified by uid exists in the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool Exist(Guid uid)
        {
            using (var context = new DataContext())
            {
                var set = GetDatabaseSet(context);
                return set.Any(e => e.Uid == uid);
            }
        }

        /// <summary>
        /// Updates the properties of the provided newEntity.
        /// </summary>
        /// <typeparam name="TEntityProperty">The type of the t entity property.</typeparam>
        /// <param name="newEntity">The new newEntity.</param>
        /// <param name="existingEntity">The existing newEntity.</param>
        /// <param name="context">The current context</param>
        /// <returns>IActionResult.</returns>
        protected IActionResult UpdateEntity<TEntityProperty>(TEntityProperty newEntity, TEntityProperty existingEntity, DataContext context) where TEntityProperty : class, IEntity
        {
            if (newEntity == null || existingEntity == null)
            {
                return BadRequest();
            }

            if (newEntity.Uid == Guid.Empty || existingEntity.Uid == Guid.Empty || !newEntity.Uid.Equals(existingEntity.Uid))
            {
                return BadRequest();
            }

            try
            {
                context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
            }
            catch (SqlException e)
            {
                return StatusCode(500, e.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Updates each property of each newEntity in the provided newEntity list.
        /// </summary>
        /// <typeparam name="TEntityProperty">The type of the t entity property.</typeparam>
        /// <param name="newEntities">The new set.</param>
        /// <param name="existingEntities">The existing set.</param>
        /// <param name="context">The current context</param>
        /// <returns>IActionResult.</returns>
        protected IActionResult UpdateEntities<TEntityProperty>(ICollection<TEntityProperty> newEntities, ICollection<TEntityProperty> existingEntities, DataContext context) where TEntityProperty : class, IEntity
        {
            if (newEntities == null || existingEntities == null)
            {
                return BadRequest();
            }

            foreach (var newEntity in newEntities)
            {
                var existingEntity = existingEntities.FirstOrDefault(p => p.Uid.Equals(newEntity.Uid));

                if (existingEntity == null)
                {
                    existingEntities.Add(newEntity);
                    continue;
                }

                try
                {
                    context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
                }
                catch (SqlException e)
                {
                    return StatusCode(500, e.Message);
                }
            }

            return Ok();
        }
    }
}