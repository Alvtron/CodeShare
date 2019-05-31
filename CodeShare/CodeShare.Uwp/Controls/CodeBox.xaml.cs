// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-26-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-27-2019
// ***********************************************************************
// <copyright file="CodeBox.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Uwp.Utilities;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class CodeBox. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CodeBox
    {
        /// <summary>
        /// The code property
        /// </summary>
        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(string), typeof(CodeBlock), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code
        {
            get => (string) GetValue(CodeProperty);
            set => SetValue(CodeProperty, value);
        }

        /// <summary>
        /// The copy code command
        /// </summary>
        private RelayCommand _copyCodeCommand;
        /// <summary>
        /// Gets the copy code command.
        /// </summary>
        /// <value>The copy code command.</value>
        public ICommand CopyCodeCommand => _copyCodeCommand = _copyCodeCommand ?? new RelayCommand(parameter => CopyCode());

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeBox"/> class.
        /// </summary>
        public CodeBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Copies the code.
        /// </summary>
        private void CopyCode()
        {
            var dataPackage = new DataPackage { RequestedOperation = DataPackageOperation.Copy };
            dataPackage.SetText(Code);
            Clipboard.SetContent(dataPackage);
        }

        /// <summary>
        /// Updates the line numbers.
        /// </summary>
        private void UpdateLineNumbers()
        {
            LineNumbers.Text = "";

            if (string.IsNullOrWhiteSpace(Code))
            {
                return;
            }

            LineNumbers.Text = GenerateLineNumbers(Code.Split('\n').Length);
        }

        /// <summary>
        /// Generates the line numbers.
        /// </summary>
        /// <param name="numberOfLines">The number of lines.</param>
        /// <returns>System.String.</returns>
        private static string GenerateLineNumbers(int numberOfLines)
        {
            var lineNumbers = "";

            for (var i = 1; i <= numberOfLines; i++)
            {
                lineNumbers += $"{i}\n";
            }

            return lineNumbers;
        }

        /// <summary>
        /// Handles the TextChanged event of the CodeTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void CodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateLineNumbers();
        }
    }
}
