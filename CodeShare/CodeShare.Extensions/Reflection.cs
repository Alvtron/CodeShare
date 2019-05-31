// ***********************************************************************
// Assembly         : CodeShare.Extensions
// Author           : Thomas Angeland
// Created          : 01-30-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="Reflection.cs" company="CodeShare.Extensions">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace CodeShare.Extensions
{
    /// <summary>
    /// Class Reflection.
    /// </summary>
    public static class Reflection
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            if (source == null)
            {
                return default(T);
            }

            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }

        /// <summary>
        /// Extension for 'Object' that copies the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The destination.</param>
        /// <exception cref="ArgumentNullException">
        /// source
        /// or
        /// target
        /// </exception>
        public static void CopyProperties(this object source, object target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            var typeTarget = target.GetType();
            var typeSource = source.GetType();
            
            var sourceProperties = typeSource.GetProperties();
            foreach (var sourceProperty in sourceProperties)
            {
                if (!sourceProperty.CanRead)
                {
                    continue;
                }

                var targetProperty = typeTarget.GetProperty(sourceProperty.Name);

                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                {
                    continue;
                }
                targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
            }
        }
    }
}
