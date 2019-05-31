// ***********************************************************************
// Assembly         : CodeShare.WebApi
// Author           : Thomas Angeland
// Created          : 05-15-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="IController.cs" company="CodeShare.WebApi">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CodeShare.WebApi.Controllers
{
    /// <summary>
    /// Interface IController
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IController<T> where T : class, IEntity
    {
        /// <summary>
        /// Gets all entities from database.
        /// </summary>
        /// <returns>ActionResult&lt;IEnumerable&lt;T&gt;&gt;.</returns>
        ActionResult<IEnumerable<T>> Get();
        /// <summary>
        /// Gets the entity bound to the specified uid from the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns>ActionResult&lt;T&gt;.</returns>
        ActionResult<T> Get(Guid uid);
        /// <summary>
        /// Updates the entity to equal entity bound to the specified uid.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>ActionResult&lt;T&gt;.</returns>
        ActionResult<T> Put(Guid uid, T entity);
        /// <summary>
        /// Adds the specified entity comment to the database.
        /// </summary>
        /// <param name="entity">The entity comment.</param>
        /// <returns>ActionResult&lt;T&gt;.</returns>
        ActionResult<T> Post(T entity);
        /// <summary>
        /// Deletes the entity bound to the specified uid from the database.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns>IActionResult.</returns>
        IActionResult Delete(Guid uid);
    }
}