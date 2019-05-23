using CodeShare.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeShare.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IController<T> where T : class, IEntity
    {
        /// <summary>
        /// Gets all entities from database.
        /// </summary>
        /// <returns></returns>
        ActionResult<IEnumerable<T>> Get();
        /// <summary>
        /// Gets the entity bound to the specified uid from the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        ActionResult<T> Get(Guid uid);
        /// <summary>
        /// Updates the entity to equal entity bound to the specified uid.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        ActionResult<T> Put(Guid uid, T entity);
        /// <summary>
        /// Adds the specified entity comment to the database.
        /// </summary>
        /// <param name="entity">The entity comment.</param>
        /// <returns></returns>
        ActionResult<T> Post(T entity);
        /// <summary>
        /// Deletes the entity bound to the specified uid from the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        IActionResult Delete(Guid uid);
    }
}