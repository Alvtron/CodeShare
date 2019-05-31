// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-28-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="SaveResetPanel.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows.Input;
using Windows.UI.Xaml;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class SaveResetPanel. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class SaveResetPanel
    {
        /// <summary>
        /// The save command property
        /// </summary>
        public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register("SaveCommand", typeof(ICommand), typeof(SaveResetPanel), new PropertyMetadata(default(ICommand)));
        /// <summary>
        /// The reset command property
        /// </summary>
        public static readonly DependencyProperty ResetCommandProperty = DependencyProperty.Register("ResetCommand", typeof(ICommand), typeof(SaveResetPanel), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the save command.
        /// </summary>
        /// <value>The save command.</value>
        public ICommand SaveCommand
        {
            get => GetValue(SaveCommandProperty) as ICommand;
            set => SetValue(SaveCommandProperty, value);
        }
        /// <summary>
        /// Gets or sets the reset command.
        /// </summary>
        /// <value>The reset command.</value>
        public ICommand ResetCommand
        {
            get => GetValue(ResetCommandProperty) as ICommand;
            set => SetValue(ResetCommandProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveResetPanel"/> class.
        /// </summary>
        public SaveResetPanel()
        {
            InitializeComponent();
        }
    }
}
