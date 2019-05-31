// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="CodeItem.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using Windows.UI.Xaml;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class CodeItem. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CodeItem
    {
        /// <summary>
        /// The code property
        /// </summary>
        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(Code), typeof(CodeItem), new PropertyMetadata(new Code()));

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public Code Code
        {
            get => GetValue(CodeProperty) as Code;
            set => SetValue(CodeProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItem"/> class.
        /// </summary>
        public CodeItem()
        {
            InitializeComponent();
        }
    }
}
