// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="QuestionsViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class QuestionsViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.ViewModel" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.ViewModel" />
    public class QuestionsViewModel : ViewModel
    {
        /// <summary>
        /// The unfiltered questions
        /// </summary>
        private ObservableCollection<Question> _unfilteredQuestions;
        /// <summary>
        /// Gets or sets the unfiltered questions.
        /// </summary>
        /// <value>The unfiltered questions.</value>
        public ObservableCollection<Question> UnfilteredQuestions
        {
            get => _unfilteredQuestions;
            set => SetField(ref _unfilteredQuestions, value);
        }

        /// <summary>
        /// The filtered questions
        /// </summary>
        private ObservableCollection<Question> _filteredQuestions;
        /// <summary>
        /// Gets or sets the filtered questions.
        /// </summary>
        /// <value>The filtered questions.</value>
        public ObservableCollection<Question> FilteredQuestions
        {
            get => _filteredQuestions;
            set => SetField(ref _filteredQuestions, value);
        }

        /// <summary>
        /// The search query
        /// </summary>
        private string _searchQuery;
        /// <summary>
        /// Gets or sets the search query.
        /// </summary>
        /// <value>The search query.</value>
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                SearchByQuery(value);
                SetField(ref _searchQuery, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionsViewModel"/> class.
        /// </summary>
        /// <param name="questions">The questions.</param>
        /// <exception cref="ArgumentNullException">questions</exception>
        public QuestionsViewModel(IEnumerable<Question> questions)
        {
            if (questions == null)
            {
                throw new ArgumentNullException(nameof(questions));
            }

            UnfilteredQuestions = new ObservableCollection<Question>(questions);
            FilteredQuestions = UnfilteredQuestions;

            UnfilteredQuestions.CollectionChanged += (s, e) =>
            {
                SearchByQuery(SearchQuery);
            };
        }

        /// <summary>
        /// Searches the by query.
        /// </summary>
        /// <param name="query">The query.</param>
        private void SearchByQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                FilteredQuestions = UnfilteredQuestions;
            }

            query = query?.ToLower();
            var queryResult = UnfilteredQuestions?.Where(u => u.Name.ToLower().StartsWith(query));

            if (queryResult == null)
            {
                return;
            }

            FilteredQuestions = new ObservableCollection<Question>();
        }
    }
}
