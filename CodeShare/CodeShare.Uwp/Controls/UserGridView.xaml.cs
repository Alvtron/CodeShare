﻿using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Views;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class UserGridView : UserControl
    {
        public static readonly DependencyProperty UsersProperty = DependencyProperty.Register("Users", typeof(IEnumerable<User>), typeof(UserGridView), new PropertyMetadata(new List<User>()));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(UserGridView), new PropertyMetadata(null));

        public IEnumerable<User> Users
        {
            get => GetValue(UsersProperty) as IEnumerable<User>;
            set => SetValue(UsersProperty, value);
        }
        public string Header
        {
            get => GetValue(HeaderProperty) as string;
            set => SetValue(HeaderProperty, value);
        }

        public UserGridView()
        {
            this.InitializeComponent();
        }

        public void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is User user)
                NavigationService.Navigate(typeof(UserPage), user.Uid, user.Name + "'s profile");
        }
    }
}
