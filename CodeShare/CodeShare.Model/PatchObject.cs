// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-26-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-27-2019
// ***********************************************************************
// <copyright file="PatchObject.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Reflection;

namespace CodeShare.Model
{
    /// <summary>
    /// Class PatchObject.
    /// </summary>
    public class PatchObject
    {
        /// <summary>
        /// Gets or sets the entity uid.
        /// </summary>
        /// <value>The entity uid.</value>
        public Guid EntityUid { get; set; }
        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        public Type EntityType { get; set; }
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <value>The property value.</value>
        public object PropertyValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchObject"/> class.
        /// </summary>
        public PatchObject()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchObject"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        private PatchObject(IEntity entity)
        {
            EntityUid = entity.Uid;
            EntityType = entity.GetType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchObject"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <exception cref="ArgumentNullException">propertyName</exception>
        public PatchObject(IEntity entity, string propertyName)
            : this(entity)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyValue = GetValue(entity, propertyName);
        }

        /// <summary>
        /// Applies the changes.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="InvalidOperationException">
        /// Could not determine property info for {PropertyName} in {entity.GetType().Name}
        /// or
        /// Could not determine target type for {PropertyName} in {entity.GetType().Name}
        /// </exception>
        public void ApplyChanges(IEntity entity)
        {
            var type = entity.GetType();
            var propertyInfo = type.GetProperty(PropertyName);

            if (propertyInfo == null)
            {
                throw new InvalidOperationException($"Could not determine property info for {PropertyName} in {entity.GetType().Name}.");
            }

            var propertyType = propertyInfo.PropertyType;
            var targetType = IsNullableType(propertyType) ? Nullable.GetUnderlyingType(propertyType) : propertyType;

            if (targetType == null)
            {
                throw new InvalidOperationException($"Could not determine target type for {PropertyName} in {entity.GetType().Name}.");
            }

            PropertyValue = Convert.ChangeType(PropertyValue, targetType);
            propertyInfo.SetValue(entity, PropertyValue, null);
        }

        /// <summary>
        /// Determines whether [is nullable type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is nullable type] [the specified type]; otherwise, <c>false</c>.</returns>
        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="InvalidOperationException">
        /// Could not determine property info for {propertyName} in {value.GetType().Name}
        /// or
        /// Failed to find matching property for {propertyName} in {value.GetType().Name}.\nError message: {e.Message}
        /// </exception>
        public static object GetValue(object value, string propertyName)
        {
            try
            {
                var propertyInfo = value.GetType().GetProperty(propertyName);

                if (propertyInfo == null)
                {
                    throw new InvalidOperationException($"Could not determine property info for {propertyName} in {value.GetType().Name}.");
                }

                return propertyInfo.GetValue(value, null);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Failed to find matching property for {propertyName} in {value.GetType().Name}.\nError message: {e.Message}.");
            }
        }
    }
}
