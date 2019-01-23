using System;
using System.Linq;
using System.Web.Http;

namespace CodeShare.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IController<T>
    {
        /// <summary>
        /// Gets all entities from database.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Get();
        /// <summary>
        /// Gets the entity bound to the specified uid from the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        IHttpActionResult Get(Guid uid);
        /// <summary>
        /// Updates the entity to equal entity bound to the specified uid.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        IHttpActionResult Put(Guid uid, T entity);
        /// <summary>
        /// Adds the specified entity comment to the database.
        /// </summary>
        /// <param name="entity">The entity comment.</param>
        /// <returns></returns>
        IHttpActionResult Post(T entity);
        /// <summary>
        /// Deletes the entity bound to the specified uid from the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        IHttpActionResult Delete(Guid uid);
        /// <summary>
        /// Exists the specified uid.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        bool Exist(Guid uid);
    }
}