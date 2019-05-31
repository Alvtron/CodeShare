// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="UserViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CodeShare.RestApi;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class UserViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.ContentViewModel{CodeShare.Model.User}" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.ContentViewModel{CodeShare.Model.User}" />
    public class UserViewModel : ContentViewModel<User>
    {
        /// <summary>
        /// The is friend
        /// </summary>
        private bool _isFriend;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is friend.
        /// </summary>
        /// <value><c>true</c> if this instance is friend; otherwise, <c>false</c>.</value>
        public bool IsFriend
        {
            get => _isFriend;
            set => SetField(ref _isFriend, value);
        }
        /// <summary>
        /// The is friend or pending friend
        /// </summary>
        private bool _isFriendOrPendingFriend;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is friend or pending friend.
        /// </summary>
        /// <value><c>true</c> if this instance is friend or pending friend; otherwise, <c>false</c>.</value>
        public bool IsFriendOrPendingFriend
        {
            get => _isFriendOrPendingFriend;
            set => SetField(ref _isFriendOrPendingFriend, value);
        }
        /// <summary>
        /// The befriend command
        /// </summary>
        private RelayCommand _befriendCommand;
        /// <summary>
        /// Gets the befriend command.
        /// </summary>
        /// <value>The befriend command.</value>
        public ICommand BefriendCommand => _befriendCommand = _befriendCommand ?? new RelayCommand(async param => await Befriend());

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public UserViewModel(User user)
            : base(user)
        {
            
        }

        /// <summary>
        /// Befriends this instance.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Befriend()
        {
            if (CurrentUser == null)
            {
                await NotificationService.DisplayErrorMessage("You are not logged in!");
                return;
            }
            if (CurrentUser.Equals(Model))
            {
                await NotificationService.DisplayErrorMessage("You can't be friends with yourself!");
                return;
            }
            NavigationService.Lock();
            if (CurrentUser.IsFriendsWith(Model) || Model.ReceivedFriendRequests.Any(f => f.RequesterUid.Equals(CurrentUser.Uid)))
            {
                await RemoveFriendship();
            }
            else if (Model.SentFriendRequests.Any(f => f.ConfirmerUid.Equals(CurrentUser.Uid)))
            {
                await AcceptFriendshipAsync();
            }
            else
            {
                await SendFriendRequest();
            }
            NavigationService.Unlock();
        }

        /// <summary>
        /// Removes the friendship.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        private async Task<bool> RemoveFriendship()
        {
            var friendship = CurrentUser.RemoveFriendship(Model);
            await RestApiService<User>.Update(CurrentUser, CurrentUser.Uid);
            await RestApiService<User>.Update(Model, Model.Uid);
            if (friendship == null || !await RestApiService<Friendship>.Delete(friendship.Uid))
            {
                await NotificationService.DisplayErrorMessage("Friendship could not be removed. Sorry about that.");
                return false;
            }

            return await RefreshAsync();
        }

        /// <summary>
        /// accept friendship as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        private async Task<bool> AcceptFriendshipAsync()
        {
            var friendship = CurrentUser.AcceptFriendRequest(Model);
            await RestApiService<User>.Update(CurrentUser, CurrentUser.Uid);
            await RestApiService<User>.Update(Model, Model.Uid);
            if (friendship == null || !await RestApiService<Friendship>.Update(friendship, friendship.Uid))
            {
                await NotificationService.DisplayErrorMessage($"Failed to accept friend request from {CurrentUser.Name} to {Model.Name}. Sorry about that.");
                return false;
            }

            return await RefreshAsync();
        }

        /// <summary>
        /// Sends the friend request.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        private async Task<bool> SendFriendRequest()
        {
            var friendship = CurrentUser.SendFriendRequest(Model);
            await RestApiService<User>.Update(CurrentUser, CurrentUser.Uid);
            await RestApiService<User>.Update(Model, Model.Uid);
            if (friendship == null || !await RestApiService<Friendship>.Update(friendship, friendship.Uid))
            {
                await NotificationService.DisplayErrorMessage($"Failed to send friend request from {CurrentUser.Name} to {Model.Name}. Sorry about that.");
                return false;
            }

            return await RefreshAsync();
        }

        /// <summary>
        /// Updates the friendship UI.
        /// </summary>
        /// <param name="thisUser">The this user.</param>
        /// <param name="currentUser">The current user.</param>
        private void UpdateFriendshipUi(User thisUser, User currentUser)
        {
            IsFriend = currentUser != null && thisUser.IsFriendsWith(currentUser);
            IsFriendOrPendingFriend = currentUser != null && thisUser.IsPendingFriendWith(currentUser);
        }

        /// <summary>
        /// Sets the user administrator privileges.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool SetUserAdministratorPrivileges(User currentUser)
        {
            return Model?.Equals(currentUser) ?? false;
        }

        /// <summary>
        /// Called when [current user changed].
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        protected override void OnCurrentUserChanged(User currentUser)
        {
            UpdateFriendshipUi(Model, currentUser);
        }

        /// <summary>
        /// Called when [model changed].
        /// </summary>
        /// <param name="model">The model.</param>
        protected override void OnModelChanged(User model)
        {
            UpdateFriendshipUi(model, CurrentUser);
        }
    }
}
