﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Dialogs;
using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Views;
using CodeShare.RestApi;

namespace CodeShare.Uwp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private List<Code> _allResults;

        private List<Code> _searchResults;
        public List<Code> SearchResults
        {
            get => _searchResults;
            set => SetField(ref _searchResults, value);
        }

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set => SetField(ref _currentUser, value);
        }

        public MainViewModel()
        {
            CurrentUser = AuthService.CurrentUser;

            // Change current user whenever auth service user changes.
            AuthService.CurrentUserChanged += (s, e) =>
            {
                CurrentUser = AuthService.CurrentUser;
            };
        }

        public async Task Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return;

            if (_allResults == null)
                _allResults = (await RestApiService<Code>.Get()).ToList();

            //Check each item in search list if it contains the query
            SearchResults = _allResults?.Where(x => x.Name != null && x.Name.ToLower().Contains(query.ToLower())).ToList();
        }

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

        private RelayCommand _signInCommand;
        public ICommand SignInCommand => _signInCommand = _signInCommand ?? new RelayCommand(async param => await SignIn());
        public async Task SignIn()
        {
            var signInDialog = new SignInDialog();
            await signInDialog.ShowAsync();

            // Update current user with new signed in user.
            CurrentUser = AuthService.CurrentUser;
        }
        
        private RelayCommand _signOutCommand;
        public ICommand SignOutCommand => _signOutCommand = _signOutCommand ?? new RelayCommand(async param => await SignOut());
        public async Task SignOut()
        {
            NavigationService.Lock();

            await AuthService.SignOutAsync();

            // Resetting current user. Assigning empty user first to trigger property change for all children.
            CurrentUser = new User();
            CurrentUser = null;

            NavigationService.Unlock();
        }
        
        private RelayCommand _goToMyProfileCommand;
        public ICommand GoToMyProfileCommand => _goToMyProfileCommand = _goToMyProfileCommand ??
            new RelayCommand(param => NavigationService.Navigate(typeof(UserPage), AuthService.CurrentUser, AuthService.CurrentUser.Name));
        
        private RelayCommand _goToMyAccountSettingsCommand;
        public ICommand GoToMyAccountSettingsCommand => _goToMyAccountSettingsCommand = _goToMyAccountSettingsCommand ??
            new RelayCommand(param => NavigationService.Navigate(typeof(UserSettingsPage), AuthService.CurrentUser, AuthService.CurrentUser.Name));

        private RelayCommand _goToAppSettingsCommand;
        public ICommand GoToAppSettingsCommand => _goToAppSettingsCommand = _goToAppSettingsCommand ??
            new RelayCommand (param => NavigationService.Navigate(typeof(AppSettingsPage), null, "Settings"));
    }
}
