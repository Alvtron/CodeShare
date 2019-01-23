using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeShare.Uwp.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        private ObservableCollection<User> _allUsers = new ObservableCollection<User>();
        public ObservableCollection<User> AllUsers
        {
            get => _allUsers;
            set => SetField(ref _allUsers, value);
        }

        private ObservableCollection<User> _filteredUsers = new ObservableCollection<User>();
        public ObservableCollection<User> FilteredUsers
        {
            get => _filteredUsers;
            set => SetField(ref _filteredUsers, value);
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

        public UsersViewModel()
        {
            AllUsers.CollectionChanged += (s, e) =>
            {
                SearchByQuery(SearchQuery);
            };

            var allUsers = RestApiService<User>.Get().Result;

            if (allUsers == null)
                return;

            AllUsers = new ObservableCollection<User>(allUsers);
            FilteredUsers = AllUsers;
        }

        private void SearchByQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
                FilteredUsers = AllUsers;

            query = query?.ToLower();

            FilteredUsers = new ObservableCollection<User>(AllUsers?.Where(u => u.Name.ToLower().StartsWith(query)));
        }
    }
}
