// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="BindableRichEditBox.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Xaml
{
    /// <summary>
    /// Class BindableRichEditBox.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.RichEditBox" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.RichEditBox" />
    public class BindableRichEditBox : RichEditBox
    {
        /// <summary>
        /// The RTF property
        /// </summary>
        public static readonly DependencyProperty RtfProperty = DependencyProperty.Register("Rtf", typeof(string), typeof(BindableRichEditBox), new PropertyMetadata(default(string), RtfPropertyChanged));

        /// <summary>
        /// The editable property
        /// </summary>
        public static readonly DependencyProperty EditableProperty = DependencyProperty.Register("Editable", typeof(bool), typeof(BindableRichEditBox), new PropertyMetadata(default(bool), EditablePropertyChanged));

        /// <summary>
        /// The lock change execution
        /// </summary>
        private bool _lockChangeExecution;

        /// <summary>
        /// Gets or sets the RTF.
        /// </summary>
        /// <value>The RTF.</value>
        public string Rtf
        {
            get => (string)GetValue(RtfProperty);
            set => SetValue(RtfProperty, value);
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                Document.GetText(TextGetOptions.None, out string text);
                return text;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BindableRichEditBox"/> is editable.
        /// </summary>
        /// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
        public bool Editable
        {
            get => (bool)GetValue(EditableProperty);
            set => SetValue(EditableProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableRichEditBox"/> class.
        /// </summary>
        public BindableRichEditBox()
        {
            TextChanged += BindableRichEditBox_TextChanged;
        }

        /// <summary>
        /// Handles the TextChanged event of the BindableRichEditBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BindableRichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!_lockChangeExecution)
            {
                _lockChangeExecution = true;
                Document.GetText(TextGetOptions.None, out var text);
                if (string.IsNullOrWhiteSpace(text))
                {
                    Rtf = "";
                }
                else
                {
                    Document.GetText(TextGetOptions.FormatRtf, out text);
                    Rtf = text;
                }
                _lockChangeExecution = false;
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            IsReadOnly = false;
            Document.SetText(TextSetOptions.None, "");
            IsReadOnly = true;
        }

        /// <summary>
        /// RTFs the property changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void RtfPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!(dependencyObject is BindableRichEditBox rtb) || rtb._lockChangeExecution)
            {
                return;
            }
            rtb.IsReadOnly = false;
            rtb.Document.SetText(TextSetOptions.FormatRtf, (string) dependencyPropertyChangedEventArgs.NewValue);
            rtb.IsReadOnly = !rtb.Editable;
        }

        /// <summary>
        /// Editables the property changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void EditablePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!(dependencyObject is BindableRichEditBox rtb)) return;
            if (!rtb._lockChangeExecution)
            {
                rtb.IsReadOnly = !rtb.Editable;
            }
        }
    }
}
