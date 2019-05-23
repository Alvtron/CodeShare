using System.Linq;
using System.Web.Http;
using CodeShare.DataAccess;
using CodeShare.Model;
using System.Collections.Generic;
using System;
using CodeShare.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CodeShare.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public abstract class EntityController<T> : ControllerBase, IController<T> where T : class, IEntity
    {
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        protected DataContext Context { get; }
        protected abstract DbSet<T> Entities { get; }
        protected abstract IQueryable<T> QueryableEntities { get; }
        protected abstract IQueryable<T> QueryableEntitiesMinimal { get; }
        /// <summary>
        /// Gets a value indicating whether the database is online.
        /// </summary>
        /// <value>
        ///   <c>true</c> if database is online; otherwise, <c>false</c>.
        /// </value>
        public bool IsDatabaseOnline
        {
            get
            {
                // Not working
                //if (!Context.Database.GetService<IRelationalDatabaseCreator>().Exists())
                //{
                //    Logger.WriteLine($"The database from context {Context.GetType().Name} does not exist.");
                //    return false;
                //}

                return true;
            }
        }

        protected EntityController()
        {
            Context = new DataContext();
        }

        protected abstract void OnPost(T entity);

        protected abstract void OnPut(T entity, T existingEntity);

        protected abstract void OnDelete(T entity);

        public ActionResult<IEnumerable<T>> Get()
        {
            if (!IsDatabaseOnline)
            {
                return null;
            }

            return Ok(QueryableEntitiesMinimal);
        }

        public ActionResult<T> Get(Guid uid)
        {
            if (!IsDatabaseOnline)
            {
                return BadRequest("The server is not available.");
            }

            var entity = QueryableEntities.FirstOrDefault(u => u.Uid == uid);

            if (entity == null)
            {
                Logger.WriteLine($"The provided {uid.GetType().Name} parameter did not match any items in the database.");
                return NotFound();
            }

            return Ok(entity);
        }

        /// <summary>
        /// Updates the entity to equal entity bound to the specified uid.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public ActionResult<T> Put(Guid uid, T entity)
        {
            if (!IsDatabaseOnline)
            {
                return BadRequest("The server is not available.");
            }

            if (CheckUpdateParameters(uid, entity) is BadRequestResult badRequest)
            {
                return badRequest;
            }

            var existingEntity = QueryableEntities.FirstOrDefault(e => e.Uid.Equals(uid));

            if (existingEntity == null)
            {
                Logger.WriteLine($"The provided {entity.GetType().Name} object with UID {uid} does not exist in the database. Attempting to add it...");
                return Post(entity);
            }

            try
            {
                UpdateEntity(entity, existingEntity);

                OnPut(entity, existingEntity);

                Context.SaveChanges();
            }
            catch (Exception exception)
            {
#if DEBUG
                Logger.WriteLine($"The provided {entity.GetType().Name} object could not be updated. {exception.Message}");
                throw;
#else
                return BadRequest($"The provided {entity.GetType().Name} object could not be updated. {exception.Message}");
#endif
            }

            return Ok(true);
        }

        /// <summary>
        /// Adds the specified entity to the database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public ActionResult<T> Post(T entity)
        {
            if (!IsDatabaseOnline)
            {
                return BadRequest("The server is not available.");
            }
            if (entity == null)
            {
                return BadRequest($"The provided {typeof(T).GetType().Name} object is empty! Post denied.");
            }
            if (Exist(entity.Uid))
            {
                return BadRequest($"The provided {typeof(T).GetType().Name} object already exists! Post denied.");
            }

            try
            {
                Entities.Add(entity);

                OnPost(entity);

                Context.SaveChanges();

                return Ok(entity);
            }
            catch (Exception exception)
            {
                Logger.WriteLine($"The provided {typeof(T).GetType().Name} object could not be added. {exception.Message}");
#if DEBUG
                throw;
#else
                return BadRequest($"The provided {typeof(T).GetType().Name} object could not be added. {exception.Message}");
#endif
            }
        }

        /// <summary>
        /// Deletes the entity bound to the specified uid from the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        public IActionResult Delete(Guid uid)
        {
            if (!IsDatabaseOnline)
            {
                return BadRequest("The server is not available.");
            }
            if (uid == null || uid == Guid.Empty)
            {
                return BadRequest($"The provided {uid.GetType().Name} is empty! Deletion denied.");
            }
            if (!Exist(uid))
            {
                return BadRequest($"The provided {uid.GetType().Name} does not exist in the database! Deletion denied.");
            }

            var entity = QueryableEntities.FirstOrDefault(e => e.Uid.Equals(uid));

            if (entity == null)
            {
                Logger.WriteLine($"The provided UID {uid} does not exist in the database.");
                return BadRequest($"The provided UID {uid} does not exist in the database.");
            }

            try
            {
                Entities.Remove(entity);

                OnDelete(entity);

                Context.SaveChanges();
            }
            catch (Exception exception)
            {
                Logger.WriteLine($"The provided {uid.GetType().Name} could not be deleted! {exception.Message}");
#if DEBUG
                throw;
#else
                return BadRequest($"The provided {uid.GetType().Name} could not be deleted! {exception.Message}");
#endif
            }

            return Ok($"Code {uid} was successfully deleted.");
        }

        /// <summary>
        /// Checks if the entity specified by uid exists in the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        protected bool Exist(Guid uid)
        {
            return IsDatabaseOnline && Entities.Any(e => e.Uid == uid);
        }

        /// <summary>
        /// Updates the properties of the provided entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newEntity">The new entity.</param>
        /// <param name="existingEntity">The existing entity.</param>
        protected void UpdateEntity(T newEntity, T existingEntity)
        {
            if (newEntity == null || existingEntity == null)
            {
                return;
            }

            if (newEntity.Uid == null || newEntity.Uid == Guid.Empty || existingEntity.Uid == null || existingEntity.Uid == Guid.Empty || !newEntity.Uid.Equals(existingEntity.Uid))
            {
                return;
            }

            Context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Updates each property of each entity in the provided entity list.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="newEntities">The new entities.</param>
        /// <param name="existingEntities">The existing entities.</param>
        protected void UpdateEntities<E>(ICollection<E> newEntities, ICollection<E> existingEntities) where E : class, IEntity
        {
            if (newEntities == null || existingEntities == null)
            {
                return;
            }

            foreach (var newEntity in newEntities)
            {
                var existingEntity = existingEntities.FirstOrDefault(p => p.Uid.Equals(newEntity.Uid));

                if (existingEntity == null)
                {
                    existingEntities.Add(newEntity);
                    continue;
                }

                Context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
            }
        }

        protected IActionResult CheckUpdateParameters(Guid uid, T entity)
        {
            if (uid == null || uid == Guid.Empty)
            {
                Logger.WriteLine($"The provided {uid.GetType().Name} parameter is empty! Put denied.");
                return BadRequest($"The provided {uid.GetType().Name} parameter is empty! Put denied.");
            }

            if (entity == null)
            {
                Logger.WriteLine($"The provided {entity.GetType().Name} object is empty! Put denied.");
                return BadRequest($"The provided {entity.GetType().Name} object is empty! Put denied.");
            }

            if (uid != entity.Uid)
            {
                Logger.WriteLine($"The provided {entity.GetType().Name} object does not match the provided key {uid}. Put denied!");
                return BadRequest($"The provided {entity.GetType().Name} object does not match the provided key {uid}. Put denied!");
            }

            else return Ok();
        }
    }
}