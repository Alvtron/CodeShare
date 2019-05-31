using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CodeShare.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class UserListViewItem : UserControl
    {
        /// <summary>
        /// The user property
        /// </summary>
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register("User", typeof(User), typeof(UserListViewItem), new PropertyMetadata(new User()));

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public User User
        {
            get => GetValue(UserProperty) as User;
            set => SetValue(UserProperty, value);
        }

        public UserListViewItem()
        {
            this.InitializeComponent();
        }
    }
}
