using CodeShare.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeShare.Uwp.ViewModels
{
    public class QuestionsViewModel : ObservableObject
    {
        private ObservableCollection<Question> _unfilteredQuestions;
        public ObservableCollection<Question> UnfilteredQuestions
        {
            get => _unfilteredQuestions;
            set => SetField(ref _unfilteredQuestions, value);
        }

        private ObservableCollection<Question> _filteredQuestions;
        public ObservableCollection<Question> FilteredQuestions
        {
            get => _filteredQuestions;
            set => SetField(ref _filteredQuestions, value);
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                SearchByQuery(value);
                SetField(ref _searchQuery, value);
            }
        }

        public QuestionsViewModel(IEnumerable<Question> Questions)
        {
            if (Questions == null)
            {
                throw new ArgumentNullException("Questions was null.");
            }

            UnfilteredQuestions = new ObservableCollection<Question>(Questions);
            FilteredQuestions = UnfilteredQuestions;

            UnfilteredQuestions.CollectionChanged += (s, e) =>
            {
                SearchByQuery(SearchQuery);
            };
        }

        private void SearchByQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
                FilteredQuestions = UnfilteredQuestions;

            query = query?.ToLower();

            FilteredQuestions = new ObservableCollection<Question>(UnfilteredQuestions?.Where(u => u.Name.ToLower().StartsWith(query)));
        }
    }
}
