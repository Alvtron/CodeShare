// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-25-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="EditFileDialog.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Dialogs
{
    /// <summary>
    /// Class EditFileDialog. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class EditFileDialog
    {
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>The file.</value>
        private File File { get; }
        /// <summary>
        /// Gets or sets the original file data.
        /// </summary>
        /// <value>The original file data.</value>
        private string OriginalFileData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditFileDialog"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public EditFileDialog(File file)
        {
            File = file;
            OriginalFileData = file.Data;
            Title = $"Edit {file}";
            InitializeComponent();
        }

        /// <summary>
        /// Discards the close button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ContentDialogButtonClickEventArgs"/> instance containing the event data.</param>
        private void Discard_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            File.Data = OriginalFileData;
        }

        /// <summary>
        /// Saves the primary button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ContentDialogButtonClickEventArgs"/> instance containing the event data.</param>
        private void Save_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
