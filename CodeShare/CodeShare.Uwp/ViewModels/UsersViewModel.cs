// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="UsersViewModel.cs" company="CodeShare">
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
    /// Class UsersViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.ViewModel" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.ViewModel" />
    public class UsersViewModel : ViewModel
    {
        /// <summary>
        /// All users
        /// </summary>
        private ObservableCollection<User> _allUsers;
        /// <summary>
        /// Gets or sets all users.
        /// </summary>
        /// <value>All users.</value>
        public ObservableCollection<User> AllUsers
        {
            get => _allUsers;
            set => SetField(ref _allUsers, value);
        }

        /// <summary>
        /// The filtered users
        /// </summary>
        private ObservableCollection<User> _filteredUsers;
        /// <summary>
        /// Gets or sets the filtered users.
        /// </summary>
        /// <value>The filtered users.</value>
        public ObservableCollection<User> FilteredUsers
        {
            get => _filteredUsers;
            set => SetField(ref _filteredUsers, value);
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
        /// Initializes a new instance of the <see cref="UsersViewModel"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <exception cref="ArgumentNullException">users</exception>
        public UsersViewModel(IEnumerable<User> users)
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            AllUsers = new ObservableCollection<User>(users);
            FilteredUsers = AllUsers;

            AllUsers.CollectionChanged += (s, e) =>
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
                FilteredUsers = AllUsers;
            }

            query = query?.ToLower();
            var result = AllUsers?.Where(u => u.Name.ToLower().StartsWith(query));

            if (result == null)
            {
                return;
            }

            FilteredUsers = new ObservableCollection<User>(result);
        }
    }
}
