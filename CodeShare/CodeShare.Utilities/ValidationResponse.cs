// ***********************************************************************
// Assembly         : CodeShare.Utilities
// Author           : Thomas Angeland
// Created          : 01-31-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 01-31-2019
// ***********************************************************************
// <copyright file="ValidationResponse.cs" company="CodeShare.Utilities">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace CodeShare.Utilities
{
    /// <summary>
    /// Enum ValidationResponse
    /// </summary>
    public enum ValidationResponse
    {
        /// <summary>
        /// The empty
        /// </summary>
        Empty,
        /// <summary>
        /// The valid
        /// </summary>
        Valid,
        /// <summary>
        /// The invalid
        /// </summary>
        Invalid,
        /// <summary>
        /// The too short
        /// </summary>
        TooShort,
        /// <summary>
        /// The too long
        /// </summary>
        TooLong,
        /// <summary>
        /// The contains illegal characters
        /// </summary>
        ContainsIllegalCharacters,
        /// <summary>
        /// The unavailable
        /// </summary>
        Unavailable,
        /// <summary>
        /// The no symbol
        /// </summary>
        NoSymbol,
        /// <summary>
        /// The no number
        /// </summary>
        NoNumber,
        /// <summary>
        /// The no lower case
        /// </summary>
        NoLowerCase,
        /// <summary>
        /// The no upper case
        /// </summary>
        NoUpperCase,
    };
}
