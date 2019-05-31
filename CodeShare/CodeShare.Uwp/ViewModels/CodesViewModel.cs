// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="CodesViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class CodesViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.ViewModel" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.ViewModel" />
    public class CodesViewModel : ViewModel
    {
        /// <summary>
        /// The codes
        /// </summary>
        private ObservableCollection<Code> _codes;
        /// <summary>
        /// Gets or sets the codes.
        /// </summary>
        /// <value>The codes.</value>
        public ObservableCollection<Code> Codes
        {
            get => _codes;
            set => SetField(ref _codes, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodesViewModel"/> class.
        /// </summary>
        /// <param name="codes">The codes.</param>
        /// <exception cref="ArgumentNullException">codes</exception>
        public CodesViewModel(IEnumerable<Code> codes)
        {
            if (codes == null)
            {
                throw new ArgumentNullException(nameof(codes));
            }

            Codes = new ObservableCollection<Code>(codes);
        }
    }
}
