// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="FileListView.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class CodeFileListView. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CodeFileListView
    {
        /// <summary>
        /// The files property
        /// </summary>
        public static readonly DependencyProperty FilesProperty = DependencyProperty.Register("CodeFiles", typeof(SortedObservableCollection<CodeFile>), typeof(CodeFileListView), new PropertyMetadata(new SortedObservableCollection<CodeFile>()));

        /// <summary>
        /// Gets or sets the code files.
        /// </summary>
        /// <value>The code files.</value>
        public SortedObservableCollection<CodeFile> CodeFiles
        {
            get => GetValue(FilesProperty) as SortedObservableCollection<CodeFile>;
            set => SetValue(FilesProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeFileListView"/> class.
        /// </summary>
        public CodeFileListView()
        {
            InitializeComponent();
            CodeFiles.CollectionChanged += (s, e) => SelectFirstFile();
        }

        /// <summary>
        /// Selects the first file.
        /// </summary>
        public void SelectFirstFile()
        {
            if (CodeFiles == null || CodeFiles.Count == 0)
            {
                return;
            }
            try
            {
                FilesList.SelectedIndex = 0;
            }
            catch (ArgumentException e)
            {
                Logger.WriteLine(e.Message);
            }
        }
    }
}
