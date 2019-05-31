// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="CodeAuthorBlock.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Views;
using System.Windows.Input;
using Windows.UI.Xaml;
using CodeShare.Uwp.ViewModels;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class CodeAuthorBlock. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CodeAuthorBlock
    {
        /// <summary>
        /// The view model property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CodeViewModel), typeof(CodeAuthorBlock), new PropertyMetadata(default(CodeViewModel)));

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public CodeViewModel ViewModel
        {
            get => GetValue(ViewModelProperty) as CodeViewModel;
            set => SetValue(ViewModelProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeAuthorBlock"/> class.
        /// </summary>
        public CodeAuthorBlock()
        {
            InitializeComponent();
        }
    }
}
