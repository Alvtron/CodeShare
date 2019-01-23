using CodeShare.Model;
using CodeShare.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace CodeShare.WebApi.Controllers
{
    public class UsersController : BaseController, IController<User>
    {
        public IQueryable<User> Entities => Context.Users
            .Include(e => e.Friends.Select(f => f.ProfilePictures))
            .Include(e => e.Logs.Select(f => f.Content))
            .Include(e => e.Logs.Select(f => f.Actor))
            .Include(e => e.Codes.Select(f => f.Banners))
            .Include(e => e.Ratings)
            .Include(e => e.Comments.Select(comment => comment.User.ProfilePictures))
            .Include(e => e.Comments.Select(comment => comment.User.Banners))
            .Include(e => e.Comments.Select(comment => comment.Ratings))
            .Include(e => e.ProfilePictures)
            .Include(e => e.Banners)
            .Include(e => e.Videos);

        /// <summary>
        /// Gets all entities from database.
        /// </summary>
        /// <returns></returns>
        [Route("api/users")]
        public IQueryable<User> Get()
        {
            if (!IsDatabaseOnline) return null;

            var entities = Context.Users
                .Include(c => c.ProfilePictures)
                .Include(a => a.Banners);

            return entities;
        }

        /// <summary>
        /// Gets the entity bound to the specified uid from the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        [ResponseType(typeof(User)), Route("api/users/{uid}")]
        public IHttpActionResult Get(Guid uid)
        {
            if (!IsDatabaseOnline) return InternalServerError();

            var entity = Entities.FirstOrDefault(u => u.Uid == uid);

            if (entity == null)
            {
                Debug.WriteLine($"The provided {uid.GetType().Name} parameter did not match any items in the database.");
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
        [ResponseType(typeof(void)), Route("api/users/{uid}")]
        public IHttpActionResult Put(Guid uid, [FromBody] User entity)
        {
            if (!IsDatabaseOnline) return InternalServerError();

            if (uid == Guid.Empty)
            {
                Debug.WriteLine($"The provided {uid.GetType().Name} parameter is empty! Put denied.");
                return BadRequest($"The provided {uid.GetType().Name} parameter is empty! Put denied.");
            }

            if (entity == null)
            {
                Debug.WriteLine($"The provided {entity.GetType().Name} object is empty! Put denied.");
                return BadRequest($"The provided {entity.GetType().Name} object is empty! Put denied.");
            }

            if (uid != entity.Uid)
            {
                Debug.WriteLine($"The provided {entity.GetType().Name} object does not match the provided key {uid}. Put denied!");
                return BadRequest($"The provided {entity.GetType().Name} object does not match the provided key {uid}. Put denied!");
            }

            var existingEntity = Entities.FirstOrDefault(u => u.Uid.Equals(uid));

            if (existingEntity == null)
            {
                Debug.WriteLine($"The provided {entity.GetType().Name} object with UID {uid} does not exist in the database. Attempting to add it...");
                return Post(entity);
            }

            try
            {
                UpdateEntity(entity, existingEntity);
                UpdateEntities(entity.Friends, existingEntity.Friends);
                UpdateEntities(entity.Comments, existingEntity.Comments);
                UpdateEntities(entity.Ratings, existingEntity.Ratings);
                UpdateEntities(entity.Logs, existingEntity.Logs);
                UpdateEntities(entity.ProfilePictures, existingEntity.ProfilePictures);
                UpdateEntities(entity.Files, existingEntity.Files);
                UpdateEntities(entity.Banners, existingEntity.Banners);
                UpdateEntities(entity.Screenshots, existingEntity.Screenshots);
                UpdateEntities(entity.Videos, existingEntity.Videos);

                Context.SaveChanges();
            }
            catch (Exception exception)
            {
#if DEBUG
                Debug.WriteLine($"The provided {entity.GetType().Name} object could not be updated. {exception.Source}: {exception.Message}");
                throw;
#else
                return BadRequest($"The provided {entity.GetType().Name} object could not be updated. {exception.Source}: {exception.Message}");
#endif
            }

            return Ok(true);
        }

        /// <summary>
        /// Adds the specified entity to the database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        [ResponseType(typeof(User)), Route("api/users")]
        public IHttpActionResult Post(User entity)
        {
            if (!IsDatabaseOnline)
                return InternalServerError();
            if (entity == null)
                return BadRequest($"The provided {entity.GetType().Name} object is empty! Post denied.");
            if (Exist(entity.Uid))
                return BadRequest($"The provided {entity.GetType().Name} object already exists! Post denied.");

            try
            {
                Context.Users.Add(entity);
                Context.SaveChanges();

                return Ok(User);
            }
#if DEBUG
            catch (DbEntityValidationException exception)
            {
                foreach (var result in exception.EntityValidationErrors)
                {
                    Debug.WriteLine($"Entity of type '{result.Entry.Entity.GetType().Name}' in state '{result.Entry.State}' has the following validation errors:");
                    foreach (var _result in result.ValidationErrors)
                    {
                        Debug.WriteLine($"- Property: '{_result.PropertyName}', Error: '{_result.ErrorMessage}'");
                    }
                }
                throw;
            }
#endif
            catch (Exception exception)
            {
#if DEBUG
                Debug.WriteLine($"The provided {entity.GetType().Name} object could not be added. {exception.Source}: {exception.Message}");
                throw;
#else
                return BadRequest($"The provided {entity.GetType().Name} object could not be added. {exception.Source}: {exception.Message}");
#endif
            }
        }

        /// <summary>
        /// Deletes the entity bound to the specified uid from the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        [ResponseType(typeof(User)), Route("api/users/{uid}")]
        public IHttpActionResult Delete(Guid uid)
        {
            if (!IsDatabaseOnline) return InternalServerError();
            if (uid == null) return BadRequest($"The provided {uid.GetType().Name} is empty! Delete denied.");
            if (!Exist(uid)) return BadRequest($"The provided {uid.GetType().Name} does not exist in the database! Delete denied.");

            try
            {
                Context.Database.ExecuteSqlCommand("DELETE FROM dbo.Friendships WHERE Friend_A = @uid OR Friend_B = @uid", new SqlParameter("uid", uid));
                Context.Database.ExecuteSqlCommand("DELETE FROM dbo.Entities WHERE Uid = @uid", new SqlParameter("uid", uid));

                Context.SaveChanges();

                return Ok($"User {uid} was successfully deleted.");
            }
            catch (Exception exception)
            {
#if DEBUG
                Debug.WriteLine($"The provided {uid.GetType().Name} could not be deleted! {exception.Source}: {exception.Message}");
                throw;
#else
                return BadRequest($"The provided {uid.GetType().Name} could not be deleted! {exception.Source}: {exception.Message}");
#endif
            }
        }

        /// <summary>
        /// Checks if the entity specified by uid exists in the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        public bool Exist(Guid uid)
        {
            return IsDatabaseOnline && Entities.Any(e => e.Uid == uid);
        }
    }
}