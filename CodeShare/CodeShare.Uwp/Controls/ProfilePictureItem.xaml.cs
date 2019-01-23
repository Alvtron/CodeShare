﻿using CodeShare.Model;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class ProfilePictureItem : UserControl
    {
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register("User", typeof(User), typeof(ProfilePictureItem), new PropertyMetadata(new User()));
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(int), typeof(ProfilePictureItem), new PropertyMetadata(50));
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ProfilePictureItem), new PropertyMetadata(Orientation.Vertical));
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(int), typeof(ProfilePictureItem), new PropertyMetadata(4));

        public User User
        {
            get => GetValue(UserProperty) as User;
            set => SetValue(UserProperty, value);
        }
        public int Size
        {
            get => (int)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        public int StrokeThickness
        {
            get => (int)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public ProfilePictureItem()
        {
            this.InitializeComponent();
        }
    }
}
