// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="Editor.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Uwp.Utilities;
using System.Windows.Input;
using Windows.UI.Text;
using Windows.UI.Xaml;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class Editor. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class Editor
    {
        /// <summary>
        /// The header property
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(Editor), new PropertyMetadata(default(string)));

        /// <summary>
        /// The editable property
        /// </summary>
        public static readonly DependencyProperty EditableProperty = DependencyProperty.Register("Editable", typeof(bool), typeof(Editor), new PropertyMetadata(true, EditablePropertyChanged));

        /// <summary>
        /// The RTF property
        /// </summary>
        public static readonly DependencyProperty RtfProperty = DependencyProperty.Register("Rtf", typeof(string), typeof(Editor), new PropertyMetadata(default(string), RtfPropertyChanged));

        /// <summary>
        /// The bold command
        /// </summary>
        private RelayCommand _boldCommand;
        /// <summary>
        /// Gets the bold command.
        /// </summary>
        /// <value>The bold command.</value>
        public ICommand BoldCommand => _boldCommand = _boldCommand ?? new RelayCommand(parameter => OnBold());

        /// <summary>
        /// The italic command
        /// </summary>
        private RelayCommand _italicCommand;
        /// <summary>
        /// Gets the italic command.
        /// </summary>
        /// <value>The italic command.</value>
        public ICommand ItalicCommand => _italicCommand = _italicCommand ?? new RelayCommand(parameter => OnItalic());

        /// <summary>
        /// The underline command
        /// </summary>
        private RelayCommand _underlineCommand;
        /// <summary>
        /// Gets the underline command.
        /// </summary>
        /// <value>The underline command.</value>
        public ICommand UnderlineCommand => _underlineCommand = _underlineCommand ?? new RelayCommand(parameter => OnUnderline());

        /// <summary>
        /// The clear command
        /// </summary>
        private RelayCommand _clearCommand;
        /// <summary>
        /// Gets the clear command.
        /// </summary>
        /// <value>The clear command.</value>
        public ICommand ClearCommand => _clearCommand = _clearCommand ?? new RelayCommand(parameter => Clear());

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Editor"/> is editable.
        /// </summary>
        /// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
        public bool Editable
        {
            get => (bool)GetValue(EditableProperty);
            set => SetValue(EditableProperty, value);
        }

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
                EditorBox.Document.GetText(TextGetOptions.None, out string text);
                return text;
            }
        }

        /// <summary>
        /// Gets the object text.
        /// </summary>
        /// <value>The object text.</value>
        public string ObjectText
        {
            get
            {
                EditorBox.Document.GetText(TextGetOptions.UseObjectText, out string text);
                return text;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [lock change execution].
        /// </summary>
        /// <value><c>true</c> if [lock change execution]; otherwise, <c>false</c>.</value>
        private bool LockChangeExecution { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Editor"/> class.
        /// </summary>
        public Editor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the TextChanged event of the EditorBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void EditorBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (LockChangeExecution)
            {
                return;
            }

            LockChangeExecution = true;

            EditorBox.Document.GetText(TextGetOptions.FormatRtf, out var rtf);
            Rtf = rtf;
                
            LockChangeExecution = false;
        }

        /// <summary>
        /// Updates the RTF text.
        /// </summary>
        /// <param name="rtf">The RTF.</param>
        private void UpdateRtfText(string rtf)
        {
            if (LockChangeExecution)
            {
                return;
            }

            EditorBox.IsReadOnly = false;
            EditorBox.Document.SetText(TextSetOptions.FormatRtf, rtf);
            EditorBox.IsReadOnly = !Editable;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            EditorBox.IsReadOnly = false;
            EditorBox.Document.SetText(TextSetOptions.None, "");
            EditorBox.IsReadOnly = !Editable;
        }

        /// <summary>
        /// RTFs the property changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void RtfPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!(dependencyObject is Editor editor))
            {
                return;
            }

            editor.UpdateRtfText(dependencyPropertyChangedEventArgs.NewValue as string);
        }

        /// <summary>
        /// Editables the property changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void EditablePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!(dependencyObject is Editor rtb)) return;
            if (!rtb.LockChangeExecution)
            {
                rtb.EditorBox.IsReadOnly = !rtb.Editable;
            }
        }

        /// <summary>
        /// Called when [bold].
        /// </summary>
        private void OnBold()
        {
            EditorBox.IsReadOnly = false;
            var selectedText = EditorBox.Document.Selection;
            if (selectedText != null)
            {
                var charFormatting = selectedText.CharacterFormat;
                charFormatting.Bold = FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
            EditorBox.IsReadOnly = !Editable;
        }

        /// <summary>
        /// Called when [italic].
        /// </summary>
        private void OnItalic()
        {
            EditorBox.IsReadOnly = false;
            var selectedText = EditorBox.Document.Selection;
            if (selectedText != null)
            {
                var charFormatting = selectedText.CharacterFormat;
                charFormatting.Italic = FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
            EditorBox.IsReadOnly = !Editable;
        }

        /// <summary>
        /// Called when [underline].
        /// </summary>
        private void OnUnderline()
        {
            EditorBox.IsReadOnly = false;
            var selectedText = EditorBox.Document.Selection;
            if (selectedText != null)
            {
                var charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == UnderlineType.None)
                {
                    charFormatting.Underline = UnderlineType.Single;
                }
                else
                {
                    charFormatting.Underline = UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
            EditorBox.IsReadOnly = !Editable;
        }
    }
}
