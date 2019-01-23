﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class BindableRichTextBlock : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BindableRichTextBlock), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(BindableRichTextBlock), new PropertyMetadata(default(string)));

        public string Text
        {
            get
            {
                var text = string.Empty;
                foreach (var block in RichTextBlock.Blocks)
                {
                    text += block.ToString();
                }
                return text;
            }
            set
            {
                RichTextBlock.Blocks.Clear();
                Run run = new Run { Text = value as string };
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(run);
                RichTextBlock.Blocks.Add(paragraph);
            }
        }

        public string Header
        {
            get => HeaderBox.Text;
            set => HeaderBox.Text = value;
        }

        public BindableRichTextBlock()
        {
            this.InitializeComponent();
        }
    }
}
