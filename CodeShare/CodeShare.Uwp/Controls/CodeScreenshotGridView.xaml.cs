// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 05-29-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="CodeScreenshotGridView.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class CodeScreenshotGridView. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CodeScreenshotGridView
    {
        /// <summary>
        /// The screenshots property
        /// </summary>
        public static readonly DependencyProperty ScreenshotsProperty = DependencyProperty.Register("Screenshots", typeof(ICollection<CodeScreenshot>), typeof(CodeScreenshotGridView), new PropertyMetadata(default(ICollection<CodeScreenshot>)));
        /// <summary>
        /// Gets or sets the screenshots.
        /// </summary>
        /// <value>The screenshots.</value>
        public ICollection<CodeScreenshot> Screenshots
        {
            get => GetValue(ScreenshotsProperty) as ICollection<CodeScreenshot>;
            set => SetValue(ScreenshotsProperty, value);
        }

        /// <summary>
        /// The delete screenshot command
        /// </summary>
        private RelayCommand<CodeScreenshot> _deleteScreenshotCommand;
        /// <summary>
        /// Gets the delete screenshot command.
        /// </summary>
        /// <value>The delete screenshot command.</value>
        public ICommand DeleteScreenshotCommand => _deleteScreenshotCommand = _deleteScreenshotCommand ?? new RelayCommand<CodeScreenshot>(DeleteScreenshot);

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeScreenshotGridView"/> class.
        /// </summary>
        public CodeScreenshotGridView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Deletes the screenshot.
        /// </summary>
        /// <param name="screenshot">The screenshot.</param>
        private void DeleteScreenshot(CodeScreenshot screenshot)
        {
            Screenshots.Remove(screenshot);
        }
    }
}
