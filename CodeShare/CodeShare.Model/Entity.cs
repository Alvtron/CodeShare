// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-28-2019
// ***********************************************************************
// <copyright file="Entity.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class Entity.
    /// Implements the <see cref="CodeShare.Model.ObservableObject" />
    /// Implements the <see cref="CodeShare.Model.IEntity" />
    /// Implements the <see cref="System.IEquatable{CodeShare.Model.IEntity}" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.ObservableObject" />
    /// <seealso cref="CodeShare.Model.IEntity" />
    /// <seealso cref="System.IEquatable{CodeShare.Model.IEntity}" />
    public abstract class Entity : ObservableObject, IEntity, IEquatable<IEntity>
    {
        /// <summary>
        /// Gets or sets the uid.
        /// </summary>
        /// <value>The uid.</value>
        public Guid Uid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; } = DateTime.Now;

        /// <summary>
        /// The updated
        /// </summary>
        private DateTime _updated = DateTime.Now;
        /// <summary>
        /// Gets or sets the updated.
        /// </summary>
        /// <value>The updated.</value>
        public DateTime Updated
        {
            get => _updated;
            set => SetField(ref _updated, value);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(IEntity other)
        {
            return Uid.Equals(other?.Uid);
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int CompareTo(object obj)
        {
            if (obj is IEntity entity)
            {
                return Uid.CompareTo(entity.Uid);
            } 
            else if (obj is ITimeRecord timeRecord)
            {
                return Created.CompareTo(timeRecord.Created);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
