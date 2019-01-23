using CodeShare.Model;
using CodeShare.WebApi.Controllers;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace CodeShare.WebApi.Controllers
{
    public class CodeController : BaseController, IController<Code>
    {
        private IQueryable<Code> Entities => Context.Codes
            .Include(c => c.User)
            .Include(c => c.User.ProfilePictures)
            .Include(c => c.User.Banners)
            .Include(c => c.User.Screenshots)
            .Include(e => e.Logs.Select(f => f.Content))
            .Include(e => e.Logs.Select(f => f.Actor))
            .Include(c => c.Files)
            .Include(c => c.Ratings)
            .Include(c => c.CodeLanguage)
            .Include(c => c.Comments.Select(comment => comment.User.ProfilePictures))
            .Include(c => c.Comments.Select(comment => comment.User.Banners))
            .Include(c => c.Comments.Select(comment => comment.Ratings))
            .Include(a => a.Banners)
            .Include(c => c.Screenshots)
            .Include(c => c.Videos);

        /// <summary>
        /// Gets all entities from database.
        /// </summary>
        /// <returns></returns>
        [Route("api/codes")]
        public IQueryable<Code> Get()
        {
            if (!IsDatabaseOnline) return null;

            var entities = Context.Codes
                .Include(a => a.Banners)
                .Include(c => c.User)
                .Include(c => c.CodeLanguage);

            return entities;
        }

        /// <summary>
        /// Gets the entity bound to the specified uid from the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        [ResponseType(typeof(Code)), Route("api/codes/{uid}")]
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
        [ResponseType(typeof(void)), Route("api/codes/{uid}")]
        public IHttpActionResult Put(Guid uid, [FromBody] Code entity)
        {
            if (!IsDatabaseOnline) return InternalServerError();

            if (CheckUpdateParameters(uid, entity) is BadRequestResult badRequest)
                return badRequest;

            var existingEntity = Entities.FirstOrDefault(u => u.Uid.Equals(uid));

            if (existingEntity == null)
            {
                Debug.WriteLine($"The provided {entity.GetType().Name} object with UID {uid} does not exist in the database. Attempting to add it...");
                return Post(entity);
            }

            try
            {
                Context.Entry(existingEntity).CurrentValues.SetValues(entity);
                UpdateEntities(entity.Comments, existingEntity.Comments);
                UpdateEntities(entity.Ratings, existingEntity.Ratings);
                UpdateEntities(entity.Logs, existingEntity.Logs);
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
        [ResponseType(typeof(Code)), Route("api/codes")]
        public IHttpActionResult Post(Code entity)
        {
            if (!IsDatabaseOnline)
                return InternalServerError();
            if (entity == null)
                return BadRequest($"The provided {entity.GetType().Name} object is empty! Post denied.");
            if (Exist(entity.Uid))
                return BadRequest($"The provided {entity.GetType().Name} object already exists! Post denied.");

            try
            {
                Context.Codes.Add(entity);
                Context.SaveChanges();

                return Ok(entity);
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
        [ResponseType(typeof(Code)), Route("api/codes/{uid}")]
        public IHttpActionResult Delete(Guid uid)
        {
            if (!IsDatabaseOnline) return InternalServerError();
            if (uid == null) return BadRequest($"The provided {uid.GetType().Name} is empty! Delete denied.");
            if (!Exist(uid)) return BadRequest($"The provided {uid.GetType().Name} does not exist in the database! Delete denied.");

            try
            {
                Context.SaveChanges();

                return Ok($"Code {uid} was successfully deleted.");
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