// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 05-30-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="QuestionScreenshotGridView.xaml.cs" company="CodeShare">
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
    /// Class QuestionScreenshotGridView. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class QuestionScreenshotGridView
    {
        /// <summary>
        /// The screenshots property
        /// </summary>
        public static readonly DependencyProperty ScreenshotsProperty = DependencyProperty.Register("Screenshots", typeof(ICollection<QuestionScreenshot>), typeof(QuestionScreenshotGridView), new PropertyMetadata(default(ICollection<QuestionScreenshot>)));
        /// <summary>
        /// Gets or sets the screenshots.
        /// </summary>
        /// <value>The screenshots.</value>
        public ICollection<QuestionScreenshot> Screenshots
        {
            get => GetValue(ScreenshotsProperty) as ICollection<QuestionScreenshot>;
            set => SetValue(ScreenshotsProperty, value);
        }

        /// <summary>
        /// The delete screenshot command
        /// </summary>
        private RelayCommand<QuestionScreenshot> _deleteScreenshotCommand;
        /// <summary>
        /// Gets the delete screenshot command.
        /// </summary>
        /// <value>The delete screenshot command.</value>
        public ICommand DeleteScreenshotCommand => _deleteScreenshotCommand = _deleteScreenshotCommand ?? new RelayCommand<QuestionScreenshot>(DeleteScreenshot);

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionScreenshotGridView"/> class.
        /// </summary>
        public QuestionScreenshotGridView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Deletes the screenshot.
        /// </summary>
        /// <param name="screenshot">The screenshot.</param>
        private void DeleteScreenshot(QuestionScreenshot screenshot)
        {
            Screenshots.Remove(screenshot);
        }
    }
}
