// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="IEntity.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;

namespace CodeShare.Model
{
    /// <summary>
    /// Interface IEntity
    /// Implements the <see cref="CodeShare.Model.ITimeRecord" />
    /// Implements the <see cref="System.IComparable" />
    /// Implements the <see cref="System.ComponentModel.INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.ITimeRecord" />
    /// <seealso cref="System.IComparable" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public interface IEntity : ITimeRecord, IComparable, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the uid.
        /// </summary>
        /// <value>The uid.</value>
        Guid Uid { get; set; }
    }
}
