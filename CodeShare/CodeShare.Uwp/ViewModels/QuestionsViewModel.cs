using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeShare.Uwp.ViewModels
{
    public class QuestionsViewModel : BaseViewModel
    {
        private ObservableCollection<Question> _unfilteredQuestions = new ObservableCollection<Question>();
        public ObservableCollection<Question> UnfilteredQuestions
        {
            get => _unfilteredQuestions;
            set => SetField(ref _unfilteredQuestions, value);
        }

        private ObservableCollection<Question> _filteredQuestions = new ObservableCollection<Question>();
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

        public QuestionsViewModel()
        {
            UnfilteredQuestions.CollectionChanged += (s, e) =>
            {
                SearchByQuery(SearchQuery);
            };

            var allQuestions = RestApiService<Question>.Get().Result;

            if (allQuestions == null)
                return;

            UnfilteredQuestions = new ObservableCollection<Question>(allQuestions);
            FilteredQuestions = UnfilteredQuestions;
        }

        private void SearchByQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
                FilteredQuestions = UnfilteredQuestions;

            query = query?.ToLower();

            FilteredQuestions = new ObservableCollection<Question>(UnfilteredQuestions?.Where(u => u.Title.ToLower().StartsWith(query)));
        }
    }
}
