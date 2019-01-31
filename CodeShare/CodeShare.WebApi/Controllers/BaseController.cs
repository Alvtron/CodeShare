using System.Linq;
using System.Web.Http;
using CodeShare.DataAccess;
using CodeShare.Model;
using System.Collections.Generic;
using System;
using CodeShare.Utilities;

namespace CodeShare.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class BaseController : ApiController
    {
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        protected DataContext Context { get; set; } = new DataContext();

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!Context.Database.Exists()) return;

            if (disposing) Context.Dispose();

            base.Dispose(disposing);
        }

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
                if (!Context.Database.Exists())
                {
                    Logger.WriteLine($"The database from context {Context.GetType().Name} does not exist. Get failed!");
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Updates the properties of the provided entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newEntity">The new entity.</param>
        /// <param name="existingEntity">The existing entity.</param>
        public void UpdateEntity<T>(T newEntity, T existingEntity) where T : class, IEntity
        {
            if (newEntity == null || existingEntity == null)
            {
                return;
            }

            Context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Updates each property of each entity in the provided entity list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newEntities">The new entities.</param>
        /// <param name="existingEntities">The existing entities.</param>
        public void UpdateEntities<T>(ICollection<T> newEntities, ICollection<T> existingEntities) where T : class, IEntity
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

        public IHttpActionResult CheckUpdateParameters(Guid uid, IEntity entity)
        {
            if (uid == Guid.Empty)
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