// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="MainViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using CodeShare.Uwp.Utilities;
using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Views;
using CodeShare.RestApi;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class MainViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.ViewModel" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.ViewModel" />
    public class MainViewModel : ViewModel
    {
        /// <summary>
        /// All results
        /// </summary>
        private List<Code> _allResults;

        /// <summary>
        /// The search results
        /// </summary>
        private List<Code> _searchResults;
        /// <summary>
        /// Gets or sets the search results.
        /// </summary>
        /// <value>The search results.</value>
        public List<Code> SearchResults
        {
            get => _searchResults;
            set => SetField(ref _searchResults, value);
        }

        /// <summary>
        /// Searches the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Task.</returns>
        public async Task Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return;

            if (_allResults == null)
            {
                _allResults = (await RestApiService<Code>.Get()).ToList();
            }

            //Check each item in search list if it contains the query
            SearchResults = _allResults?.Where(x => x.Name != null && x.Name.ToLower().Contains(query.ToLower())).ToList();
        }

        /// <summary>
        /// Submits the search.
        /// </summary>
        /// <param name="args">The <see cref="AutoSuggestBoxQuerySubmittedEventArgs"/> instance containing the event data.</param>
        public void SubmitSearch(AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                var item = (Code)args.ChosenSuggestion;

                switch (item)
                {
                    case Code _:
                        NavigationService.Navigate(typeof(CodePage), item.Uid, item.Name);
                        break;
                }
            }
            else
            {
                // No match. Navigate to search page and list appropriate results for the search-query
                // NavigationService.Navigate(typeof(SearchPage), args.QueryText);
            }
        }

        /// <summary>
        /// The sign out command
        /// </summary>
        private RelayCommand _signOutCommand;
        /// <summary>
        /// Gets the sign out command.
        /// </summary>
        /// <value>The sign out command.</value>
        public ICommand SignOutCommand => _signOutCommand = _signOutCommand ?? new RelayCommand(async param => await SignOut());
        /// <summary>
        /// Represents an event that is raised when the sign-out operation is complete.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SignOut()
        {
            NavigationService.Lock();

            await AuthService.SignOutAsync();

            NavigationService.Unlock();
        }

        /// <summary>
        /// The go to my profile command
        /// </summary>
        private RelayCommand _goToMyProfileCommand;
        /// <summary>
        /// Gets the go to my profile command.
        /// </summary>
        /// <value>The go to my profile command.</value>
        public ICommand GoToMyProfileCommand => _goToMyProfileCommand = _goToMyProfileCommand ??
            new RelayCommand(param => NavigationService.Navigate(typeof(UserPage), AuthService.CurrentUser, AuthService.CurrentUser.Name));

        /// <summary>
        /// The go to my account settings command
        /// </summary>
        private RelayCommand _goToMyAccountSettingsCommand;
        /// <summary>
        /// Gets the go to my account settings command.
        /// </summary>
        /// <value>The go to my account settings command.</value>
        public ICommand GoToMyAccountSettingsCommand => _goToMyAccountSettingsCommand = _goToMyAccountSettingsCommand ??
            new RelayCommand(param => NavigationService.Navigate(typeof(UserSettingsPage), AuthService.CurrentUser, AuthService.CurrentUser.Name));

        /// <summary>
        /// The go to application settings command
        /// </summary>
        private RelayCommand _goToAppSettingsCommand;
        /// <summary>
        /// Gets the go to application settings command.
        /// </summary>
        /// <value>The go to application settings command.</value>
        public ICommand GoToAppSettingsCommand => _goToAppSettingsCommand = _goToAppSettingsCommand ??
            new RelayCommand (param => NavigationService.Navigate(typeof(AppSettingsPage), null, "Settings"));
    }
}
