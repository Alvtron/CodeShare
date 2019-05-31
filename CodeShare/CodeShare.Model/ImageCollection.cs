// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-29-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="ImageCollection.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CodeShare.Utilities;
using Newtonsoft.Json;

namespace CodeShare.Model
{
    /// <summary>
    /// Class ImageCollection.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    public abstract class ImageCollection : Entity
    {
        /// <summary>
        /// The primary
        /// </summary>
        private WebImage _primary;
        /// <summary>
        /// Gets or sets the primary.
        /// </summary>
        /// <value>The primary.</value>
        public WebImage Primary
        {
            get => _primary;
             set => SetField(ref _primary, value);
        }
        /// <summary>
        /// Gets or sets the primary uid.
        /// </summary>
        /// <value>The primary uid.</value>
        public Guid? PrimaryUid { get; set; }
        /// <summary>
        /// Gets or sets the unused.
        /// </summary>
        /// <value>The unused.</value>
        public SortedObservableCollection<WebImage> Unused { get; set; } = new SortedObservableCollection<WebImage>(b => b.Created, true);
        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <value>The images.</value>
        [NotMapped, JsonIgnore] public IEnumerable<WebImage> Images
        {
            get
            {
                if (Primary == null && Unused?.Count == 0)
                {
                    return new List<WebImage>();
                }
                if (Primary != null && Unused?.Count == 0)
                {
                    return new List<WebImage> { Primary };
                }
                if (Primary != null && Unused?.Count > 0)
                {
                    return Unused.Concat(new List<WebImage> {Primary});
                }
                return new List<WebImage>();
            }
        }
        /// <summary>
        /// Gets a value indicating whether this instance has images.
        /// </summary>
        /// <value><c>true</c> if this instance has images; otherwise, <c>false</c>.</value>
        [NotMapped, JsonIgnore] public bool HasImages => Images.Any();

        /// <summary>
        /// Finds the specified key selector.
        /// </summary>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>WebImage.</returns>
        public WebImage Find(Func<WebImage, bool> keySelector)
        {
            return keySelector == null ? null : Images.FirstOrDefault(keySelector);
        }

        /// <summary>
        /// Sets the primary.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <exception cref="ArgumentException">image</exception>
        private void SetPrimary(WebImage image)
        {
            if (image == null)
            {
                Logger.WriteLine($"Failed to set new primary image. Provided {nameof(image)} is null.");
                throw new ArgumentException(nameof(image));
            }
            if (Primary != null)
            {
                Unused.Add(Primary);
            }
            Primary = image;
            PrimaryUid = image.Uid;
        }

        /// <summary>
        /// Removes the primary.
        /// </summary>
        private void RemovePrimary()
        {
            var newPrimary = Unused.FirstOrDefault();
            Primary = newPrimary;
            PrimaryUid = newPrimary?.Uid;
        }

        /// <summary>
        /// Adds the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <exception cref="ArgumentException">image</exception>
        public void Add(WebImage image)
        {
            if (image == null)
            {
                Logger.WriteLine($"Failed to add image. Provided {nameof(image)} is null.");
                throw new ArgumentException(nameof(image));
            }
            SetPrimary(image);
        }

        /// <summary>
        /// Changes the specified key selector.
        /// </summary>
        /// <param name="keySelector">The key selector.</param>
        /// <exception cref="ArgumentException">keySelector</exception>
        public void Change(Func<WebImage, bool> keySelector)
        {
            var existingImage = Find(keySelector);
            if (existingImage == null)
            {
                Logger.WriteLine($"Failed to change image. Provided {nameof(keySelector)} does not select a image from this collection.");
                throw new ArgumentException(nameof(keySelector));
            }

            SetPrimary(existingImage);
        }

        /// <summary>
        /// Removes the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <exception cref="ArgumentException">image</exception>
        public void Remove(WebImage image)
        {
            var existingImage = Find(i => i.Equals(image));
            if (existingImage == null)
            {
                Logger.WriteLine($"Failed to remove image. Provided {nameof(image)} does not exist in this collection.");
                throw new ArgumentException(nameof(image));
            }
            if (existingImage.Equals(Primary))
            {
                RemovePrimary();
            }
            else
            {
                Unused.Remove(existingImage);
            }
        }
    }
}