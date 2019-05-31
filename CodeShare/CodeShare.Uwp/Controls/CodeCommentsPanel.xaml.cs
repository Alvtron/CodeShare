// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-29-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="CodeCommentsPanel.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Windows.UI.Xaml;
using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using System.Windows.Input;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;
using CodeShare.RestApi;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class CodeCommentsPanel. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CodeCommentsPanel
    {
        /// <summary>
        /// The view model property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CodeViewModel), typeof(CodeCommentsPanel), new PropertyMetadata(default(CodeViewModel)));

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
        /// Initializes a new instance of the <see cref="CodeCommentsPanel"/> class.
        /// </summary>
        public CodeCommentsPanel()
        {
            InitializeComponent();
        }
    }
}
