// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 05-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="CommentViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Utilities;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class CommentViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.EntityViewModel{CodeShare.Model.Comment}" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.EntityViewModel{CodeShare.Model.Comment}" />
    public class CommentViewModel : EntityViewModel<Comment>
    {
        /// <summary>
        /// The score color
        /// </summary>
        private Brush _scoreColor;
        /// <summary>
        /// Gets or sets the color of the score.
        /// </summary>
        /// <value>The color of the score.</value>
        public Brush ScoreColor
        {
            get => _scoreColor;
            set => SetField(ref _scoreColor, value);
        }
        /// <summary>
        /// The is liked by user
        /// </summary>
        private bool _isLikedByUser;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is liked by user.
        /// </summary>
        /// <value><c>true</c> if this instance is liked by user; otherwise, <c>false</c>.</value>
        public bool IsLikedByUser
        {
            get => _isLikedByUser;
            set => SetField(ref _isLikedByUser, value);
        }
        /// <summary>
        /// The is disliked by user
        /// </summary>
        private bool _isDislikedByUser;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is disliked by user.
        /// </summary>
        /// <value><c>true</c> if this instance is disliked by user; otherwise, <c>false</c>.</value>
        public bool IsDislikedByUser
        {
            get => _isDislikedByUser;
            set => SetField(ref _isDislikedByUser, value);
        }
        /// <summary>
        /// Sets the user rating value.
        /// </summary>
        /// <value>The user rating value.</value>
        private RatingValue UserRatingValue
        {
            set
            {
                IsDislikedByUser = value == RatingValue.Negative;
                IsLikedByUser = value == RatingValue.Positive;
            }
        }
        /// <summary>
        /// The navigate to user command
        /// </summary>
        private RelayCommand _navigateToUserCommand;
        /// <summary>
        /// Gets the navigate to user command.
        /// </summary>
        /// <value>The navigate to user command.</value>
        public ICommand NavigateToUserCommand => _navigateToUserCommand = _navigateToUserCommand ?? new RelayCommand(parameter => NavigateToUser());
        /// <summary>
        /// The like command
        /// </summary>
        private RelayCommand _likeCommand;
        /// <summary>
        /// Gets the like command.
        /// </summary>
        /// <value>The like command.</value>
        public ICommand LikeCommand => _likeCommand = _likeCommand ?? new RelayCommand(async parameter => await SetRatingAsync(RatingValue.Positive));
        /// <summary>
        /// The dislike command
        /// </summary>
        private RelayCommand _dislikeCommand;
        /// <summary>
        /// Gets the dislike command.
        /// </summary>
        /// <value>The dislike command.</value>
        public ICommand DislikeCommand => _dislikeCommand = _dislikeCommand ?? new RelayCommand(async parameter => await SetRatingAsync(RatingValue.Negative));
        /// <summary>
        /// The reply command
        /// </summary>
        private RelayCommand _replyCommand;
        /// <summary>
        /// Gets the reply command.
        /// </summary>
        /// <value>The reply command.</value>
        public ICommand ReplyCommand => _replyCommand = _replyCommand ?? new RelayCommand(async parameter => await AddReply());
        /// <summary>
        /// The share command
        /// </summary>
        private RelayCommand _shareCommand;
        /// <summary>
        /// Gets the share command.
        /// </summary>
        /// <value>The share command.</value>
        public ICommand ShareCommand => _shareCommand = _shareCommand ?? new RelayCommand(async parameter => await Share());
        /// <summary>
        /// The report command
        /// </summary>
        private RelayCommand _reportCommand;
        /// <summary>
        /// Gets the report command.
        /// </summary>
        /// <value>The report command.</value>
        public ICommand ReportCommand => _reportCommand = _reportCommand ?? new RelayCommand(async parameter => await Report());

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentViewModel"/> class.
        /// </summary>
        /// <param name="comment">The comment.</param>
        public CommentViewModel(Comment comment)
            : base(comment)
        {
            DetermineUserRating();
            DetermineScoreColor();
            Model.RatingCollection.CollectionChanged += (s,e) => DetermineUserRating();
            Model.RatingCollection.CollectionChanged += (s, e) => DetermineScoreColor();
        }

        /// <summary>
        /// Determines the user rating.
        /// </summary>
        private void DetermineUserRating()
        {
            if (CurrentUser == null)
            {
                return;
            }
            var rating = Model.RatingCollection.Ratings.FirstOrDefault(r => r.UserUid.Equals(CurrentUser.Uid));
            UserRatingValue = rating?.Value ?? RatingValue.None;
        }

        /// <summary>
        /// Determines the color of the score.
        /// </summary>
        private void DetermineScoreColor()
        {
            if (Model.RatingCollection.NumberOfLikes > Model.RatingCollection.NumberOfDislikes)
            {
                ScoreColor = new SolidColorBrush(Colors.Green);
            }
            else if (Model.RatingCollection.NumberOfLikes < Model.RatingCollection.NumberOfDislikes)
            {
                ScoreColor = new SolidColorBrush(Colors.Red);
            }
            else
            {
                ScoreColor = new SolidColorBrush(Colors.Gray);
            }
        }

        /// <summary>
        /// Navigates to user.
        /// </summary>
        /// <exception cref="InvalidOperationException">Developer error. Comment is null.</exception>
        private void NavigateToUser()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Developer error. Comment is null.");
            }
            NavigationService.Navigate(typeof(UserPage), Model.User);
        }

        /// <summary>
        /// create rating as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="ratingValue">The rating value.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        private async Task<bool> CreateRatingAsync(User user, RatingValue ratingValue)
        {
            var rating = new Rating(Model.RatingCollection, user, ratingValue);
            if (!await RestApiService<Rating>.Add(rating))
            {
                Logger.WriteLine("Failed to add new rating to REST API.");
                return false;
            }
            rating = await RestApiService<Rating>.Get(rating.Uid);
            if (rating == null)
            {
                Logger.WriteLine("Failed to retrieve new rating from REST API.");
                return false;
            }
            Model.RatingCollection.AddRating(rating);
            return true;
        }

        /// <summary>
        /// update rating as an asynchronous operation.
        /// </summary>
        /// <param name="rating">The rating.</param>
        /// <param name="ratingValue">The rating value.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        private async Task<bool> UpdateRatingAsync(Rating rating, RatingValue ratingValue)
        {
            rating.Update(ratingValue);
            Model.RatingCollection.AddRating(rating);
            return await RestApiService<Rating>.Update(rating, rating.Uid);
        }

        /// <summary>
        /// create or update rating as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="ratingValue">The rating value.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        private async Task<bool> CreateOrUpdateRatingAsync(User user, RatingValue ratingValue)
        {
            var rating = Model.RatingCollection.Ratings.FirstOrDefault(r => r.UserUid.Equals(user.Uid));
            return rating == null ? await CreateRatingAsync(user, ratingValue) : await UpdateRatingAsync(rating, ratingValue);
        }

        /// <summary>
        /// set rating as an asynchronous operation.
        /// </summary>
        /// <param name="ratingValue">The rating value.</param>
        /// <returns>Task.</returns>
        /// <exception cref="InvalidOperationException">Developer error. Comment is null.</exception>
        private async Task SetRatingAsync(RatingValue ratingValue)
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Developer error. Comment is null.");
            }
            if (AuthService.CurrentUser == null)
            {
                await NotificationService.DisplayErrorMessage("You need to log in first!");
                return;
            }
            if (!await CreateOrUpdateRatingAsync(CurrentUser, ratingValue))
            {
                await NotificationService.DisplayErrorMessage("Something went wrong. Please try again later.");
            }
        }

        /// <summary>
        /// Adds the reply.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exception cref="InvalidOperationException">Developer error. Comment is null.</exception>
        private async Task AddReply()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Developer error. Comment is null.");
            }
            if (CurrentUser == null)
            {
                await NotificationService.DisplayErrorMessage("You need to log in first!");
                return;
            }

            var replyDialog = new AddCommentDialog($"Reply to {Model?.User?.Name}");

            if (await replyDialog.ShowAsync() != ContentDialogResult.Primary)
            {
                return;
            }

            var newComment = new Comment(Model, CurrentUser, replyDialog.Text);
            NavigationService.Lock();
            if (!await RestApiService<Comment>.Add(newComment))
            {
                Logger.WriteLine("Failed to upload new comment to REST API.");
                await NotificationService.DisplayErrorMessage("Failed to post comment");
                NavigationService.Unlock();
                return;
            }
            newComment = await RestApiService<Comment>.Get(newComment.Uid);
            if (newComment == null)
            {
                Logger.WriteLine("Failed to retrieve new comment from REST API.");
                await NotificationService.DisplayErrorMessage("The comment was successfully posted, but a page refresh is needed.");
                NavigationService.Unlock();
                return;
            }
            Model.AddReply(newComment);
            NavigationService.Unlock();
        }

        /// <summary>
        /// Shares this instance.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task Share()
        {
            if (Model == null)
            {
                await NotificationService.DisplayErrorMessage("Developer error.");
                return;
            }

            await NotificationService.DisplayErrorMessage("This is not implemented.");
        }

        /// <summary>
        /// Reports this instance.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exception cref="InvalidOperationException">Developer error. Comment is null.</exception>
        private async Task Report()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Developer error. Comment is null.");
            }

            var reportDialog = new ReportDialog($"Reply by {Model?.User?.Name}");

            var dialogResult = await reportDialog.ShowAsync();

            if (dialogResult != ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(reportDialog.Message))
            {
                await NotificationService.DisplayErrorMessage($"Please provide a reason for why you want to report.");
                return;
            }

            if (!await RestApiService<Report>.Add(new Report(Model, reportDialog.Message)))
            {
                await NotificationService.DisplayErrorMessage("We where unable to upload that report. Sorry about that. Please try again later.");
                return;
            }

            await NotificationService.DisplayThankYouMessage("Thanks for contributing to a better community for everyone! You rock!");
        }

        /// <summary>
        /// Sets the user administrator privileges.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool SetUserAdministratorPrivileges(User currentUser)
        {
            return Model.User?.Equals(currentUser) ?? false;
        }

        /// <summary>
        /// Called when [current user changed].
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        protected override void OnCurrentUserChanged(User currentUser)
        {
        }

        /// <summary>
        /// Called when [model changed].
        /// </summary>
        /// <param name="model">The model.</param>
        protected override void OnModelChanged(Comment model)
        {
        }
    }
}
