// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="CodeSearchView.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class CodeSearchView. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CodeSearchView
    {
        /// <summary>
        /// The codes property
        /// </summary>
        public static readonly DependencyProperty CodesProperty = DependencyProperty.Register("Codes", typeof(IEnumerable<Code>), typeof(CodeSearchView), new PropertyMetadata(new List<Code>()));
        /// <summary>
        /// The header property
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(CodeSearchView), new PropertyMetadata(null));
        /// <summary>
        /// The search query property
        /// </summary>
        public static readonly DependencyProperty SearchQueryProperty = DependencyProperty.Register("SearchQuery", typeof(string), typeof(CodeSearchView), new PropertyMetadata(null));
        /// <summary>
        /// The is search box enabled property
        /// </summary>
        public static readonly DependencyProperty IsSearchBoxEnabledProperty = DependencyProperty.Register("IsSearchBoxEnabled", typeof(bool), typeof(CodeSearchView), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the filtered codes.
        /// </summary>
        /// <value>The filtered codes.</value>
        private ObservableCollection<Code> FilteredCodes { get; set; } = new ObservableCollection<Code>();

        /// <summary>
        /// Gets or sets the codes.
        /// </summary>
        /// <value>The codes.</value>
        public IEnumerable<Code> Codes
        {
            get => GetValue(CodesProperty) as IEnumerable<Code>;
            set
            {
                SetValue(CodesProperty, value);
                SearchByQuery(SearchQuery);
            }
        }
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header
        {
            get => GetValue(HeaderProperty) as string;
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Gets or sets the search query.
        /// </summary>
        /// <value>The search query.</value>
        public string SearchQuery
        {
            get => GetValue(SearchQueryProperty) as string;
            set
            {
                SearchByQuery(value);
                SetValue(SearchQueryProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is search box enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is search box enabled; otherwise, <c>false</c>.</value>
        public bool IsSearchBoxEnabled
        {
            get => (bool)GetValue(IsSearchBoxEnabledProperty);
            set => SetValue(IsSearchBoxEnabledProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeSearchView"/> class.
        /// </summary>
        public CodeSearchView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates the header.
        /// </summary>
        private void UpdateHeader()
        {
            Header = FilteredCodes.Count == 1
                ? $"{FilteredCodes.Count} result"
                : $"{FilteredCodes.Count} results";
        }

        /// <summary>
        /// Updates the ListView.
        /// </summary>
        /// <param name="codes">The codes.</param>
        private void UpdateListView(IEnumerable<Code> codes)
        {
            FilteredCodes.Clear();
            foreach (var code in codes)
            {
                FilteredCodes.Add(code);
            }
            UpdateHeader();
        }

        /// <summary>
        /// Searches the by query.
        /// </summary>
        /// <param name="query">The query.</param>
        private void SearchByQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                UpdateListView(Codes);
                return;
            }

            query = query.ToLower();

            UpdateListView(Codes?.Where(u => u.Name.ToLower().Contains(query) || u.User.Name.ToLower().StartsWith(query)));
        }
    }
}
