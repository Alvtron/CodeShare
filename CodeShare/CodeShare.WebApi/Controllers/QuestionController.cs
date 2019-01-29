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
    public class QuestionController : BaseController, IController<Question>
    {
        private IQueryable<Question> Entities => Context.Questions
            .Include(q => q.User.ProfilePictures)
            .Include(q => q.User.Banners)
            .Include(e => e.Logs)
            .Include(q => q.Solution.User.ProfilePictures)
            .Include(q => q.Solution.User.Banners)
            .Include(q => q.CodeLanguage)
            .Include(q => q.Ratings)
            .Include(q => q.Replies)
            .Include(q => q.Replies.Select(comment => comment.User.ProfilePictures))
            .Include(q => q.Replies.Select(comment => comment.User.Banners));

        /// <summary>
        /// Gets all entities from database.
        /// </summary>
        /// <returns></returns>
        [Route("api/questions")]
        public IQueryable<Question> Get()
        {
            if (!IsDatabaseOnline) return null;

            var entities = Context.Questions
                .Include(a => a.Banners)
                .Include(q => q.User.ProfilePictures)
                .Include(q => q.User.Banners)
                .Include(q => q.CodeLanguage);

            return entities;
        }

        /// <summary>
        /// Gets the entity bound to the specified uid from the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        [ResponseType(typeof(Question)), Route("api/questions/{uid}")]
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
        [ResponseType(typeof(void)), Route("api/questions/{uid}")]
        public IHttpActionResult Put(Guid uid, [FromBody] Question entity)
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
                UpdateEntity(entity, existingEntity);
                UpdateEntities(entity.Replies, existingEntity.Replies);
                UpdateEntities(entity.Ratings, existingEntity.Ratings);
                UpdateEntities(entity.Logs, existingEntity.Logs);
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
        [ResponseType(typeof(Question)), Route("api/questions")]
        public IHttpActionResult Post(Question entity)
        {
            if (!IsDatabaseOnline)
                return InternalServerError();
            if (entity == null)
                return BadRequest($"The provided {entity.GetType().Name} object is empty! Post denied.");
            if (Exist(entity.Uid))
                return BadRequest($"The provided {entity.GetType().Name} object already exists! Post denied.");

            try
            {
                Context.Questions.Add(entity);
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
        [ResponseType(typeof(Question)), Route("api/questions/{uid}")]
        public IHttpActionResult Delete(Guid uid)
        {
            if (!IsDatabaseOnline) return InternalServerError();
            if (uid == null) return BadRequest($"The provided {uid.GetType().Name} is empty! Delete denied.");
            if (!Exist(uid)) return BadRequest($"The provided {uid.GetType().Name} does not exist in the database! Delete denied.");

            try
            {
                var question = Context.Questions.Find(uid);

                if (question == null)
                    return BadRequest("Entities do not exist! Delete denied.");

                Context.Questions.Remove(question);
                Context.SaveChanges();

                return Ok($"Question {uid} was successfully deleted.");
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