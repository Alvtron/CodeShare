// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="RichTextBox.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace CodeShare.Uwp.Xaml
{
    /// <summary>
    /// Class RichTextBox.
    /// Implements the <see cref="CodeShare.Uwp.Xaml.BindableRichEditBox" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.Xaml.BindableRichEditBox" />
    public class RichTextBox : BindableRichEditBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextBox"/> class.
        /// </summary>
        public RichTextBox()
        {
            //IsEnabled = false;
            IsReadOnly = true;
            AllowFocusOnInteraction = false;
            AllowFocusWhenDisabled = false;
            FocusVisualPrimaryBrush = new SolidColorBrush(Colors.Transparent);
            FocusVisualSecondaryBrush = new SolidColorBrush(Colors.Transparent);
            Background = new SolidColorBrush(Colors.Transparent);
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            BorderThickness = new Thickness(0);
            Padding = new Thickness(0);
        }
    }
}