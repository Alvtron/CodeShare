// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 02-01-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="ItemClickCommand.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Utilities
{
    /// <summary>
    /// Class ItemClickCommand.
    /// </summary>
    public static class ItemClickCommand
    {
        /// <summary>
        /// The command property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ItemClickCommand), new PropertyMetadata(null, OnCommandPropertyChanged));
        /// <summary>
        /// The command parameter property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(ItemClickCommand), new PropertyMetadata(null));

        /// <summary>
        /// Sets the command.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="value">The value.</param>
        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Sets the command parameter.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="parameter">The parameter.</param>
        public static void SetCommandParameter(DependencyObject d, object parameter)
        {
            d.SetValue(CommandParameterProperty, parameter);
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns>ICommand.</returns>
        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        /// <summary>
        /// Gets the command parameter.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns>System.Object.</returns>
        public static object GetCommandParameter(DependencyObject d)
        {
            return d.GetValue(CommandParameterProperty);
        }

        /// <summary>
        /// Handles the <see cref="E:CommandPropertyChanged" /> event.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListViewBase control)
            {
                control.ItemClick += OnItemClick;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ItemClick" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private static void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var control = sender as ListViewBase;
            var command = GetCommand(control);
            var parameter = GetCommandParameter(control);

            if (parameter != null)
            {
                if (command != null && command.CanExecute(parameter))
                {
                    command.Execute(parameter);
                }
            }
            else
            {
                if (command != null && command.CanExecute(e.ClickedItem))
                {
                    command.Execute(e.ClickedItem);
                }
            }
        }
    }
}
