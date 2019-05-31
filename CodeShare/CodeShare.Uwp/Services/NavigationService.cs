// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="NavigationService.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Utilities;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Views;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Services
{
    /// <summary>
    /// Class NavigationService.
    /// </summary>
    public static class NavigationService
    {
        /// <summary>
        /// Gets or sets the frame.
        /// </summary>
        /// <value>The frame.</value>
        private static Frame Frame { get; set; }
        /// <summary>
        /// Gets or sets the navigation view.
        /// </summary>
        /// <value>The navigation view.</value>
        private static NavigationView NavigationView { get; set; }
        /// <summary>
        /// Gets or sets the progress ring.
        /// </summary>
        /// <value>The progress ring.</value>
        private static ProgressRing ProgressRing { get; set; }
        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>The headers.</value>
        private static ObservableStack<string> Headers { get; } = new ObservableStack<string>();
        /// <summary>
        /// Gets a value indicating whether this <see cref="NavigationService"/> is initialized.
        /// </summary>
        /// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
        public static bool Initialized => Frame != null && NavigationView != null && ProgressRing != null && Headers != null;

        /// <summary>
        /// Initializes the specified frame.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="navigationView">The navigation view.</param>
        /// <param name="progressRing">The progress ring.</param>
        public static void Initialize(Frame frame, NavigationView navigationView, ProgressRing progressRing)
        {
            Logger.WriteLine("Initializing NavigationService...");

            Frame = frame;
            NavigationView = navigationView;
            ProgressRing = progressRing;

            // When header collection changes, update navigation view header
            Headers.CollectionChanged += (s, e) =>
            {
                NavigationView.Header = (Headers.Count > 0) ? Headers.Peek() : "";
            };

            Frame.Navigated += (s, e) =>
            {
                UpdateBackButtonVisibillity();
            };

            if (!Initialized)
            {
                Logger.WriteLine("NavigationService was not properly initialized.");
            }
            else
            {
                Logger.WriteLine("NavigationService was properly initialized.");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can go back.
        /// </summary>
        /// <value><c>true</c> if this instance can go back; otherwise, <c>false</c>.</value>
        public static bool CanGoBack => Frame?.CanGoBack ?? false;

        /// <summary>
        /// Gets a value indicating whether this instance can go forward.
        /// </summary>
        /// <value><c>true</c> if this instance can go forward; otherwise, <c>false</c>.</value>
        public static bool CanGoForward => Frame?.CanGoForward ?? false;

        /// <summary>
        /// Sets the header title.
        /// </summary>
        /// <param name="title">The title.</param>
        public static void SetHeaderTitle(string title) => NavigationView.Header = title;

        /// <summary>
        /// Updates the back button visibillity.
        /// </summary>
        public static void UpdateBackButtonVisibillity()
        {
            if (!Initialized)
            {
                Logger.WriteLine("Can't update back-button visibillity. NavigationService is not initialized.");
                return;
            }

            NavigationView.IsBackButtonVisible = Frame.CanGoBack
                ? NavigationViewBackButtonVisible.Auto
                : NavigationViewBackButtonVisible.Collapsed;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public static void Clear()
        {
            Frame.BackStack.Clear();
            Headers.Clear();
            UpdateBackButtonVisibillity();
        }

        /// <summary>
        /// Navigates the specified view type.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="newHeader">The new header.</param>
        public static void Navigate(Type viewType, object parameter, string newHeader = "")
        {
            if (!Initialized)
            {
                Logger.WriteLine("Can't navigate. NavigationService is not initialized.");
                return;
            }

            Headers.Push(newHeader);
            Frame?.Navigate(viewType, parameter);

            if (viewType == typeof(HomePage))
            {
                Clear();
                Headers.Push(newHeader);
            }
        }

        /// <summary>
        /// Navigates the specified page name.
        /// </summary>
        /// <param name="pageName">Name of the page.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task.</returns>
        public static async Task Navigate(string pageName, object parameter = null)
        {
            if (!Initialized)
            {
                Logger.WriteLine("Can't navigate. NavigationService is not initialized.");
                return;
            }

            if (string.IsNullOrWhiteSpace(pageName))
            {
                Logger.WriteLine("Can't navigate. Provided page name is empty.");
                return;
            }

            switch (pageName)
            {
                case "Home":
                    Navigate(typeof(HomePage), parameter, pageName);
                    return;
                case "Code":
                    Navigate(typeof(CodePage), parameter, pageName);
                    return;
                case "Codes":
                    Navigate(typeof(CodesPage), parameter, pageName);
                    return;
                case "Question":
                    Navigate(typeof(QuestionPage), parameter, pageName);
                    return;
                case "Questions":
                    Navigate(typeof(QuestionsPage), parameter, pageName);
                    return;
                case "User":
                    Navigate(typeof(UserPage), parameter, pageName);
                    return;
                case "Users":
                    Navigate(typeof(UsersPage), parameter, pageName);
                    return;
                case "Comment":
                    if (parameter == null)
                    {
                        return;
                    }
                    var codeComment = await RestApiService<Comment>.Get((Guid)parameter);
                    await new CommentDialog(codeComment).ShowAsync();
                    return;
                case "Upload code":
                    await new AddCodeDialog().ShowAsync();
                    return;
                case "Ask a question":
                    await new AddQuestionDialog().ShowAsync();
                    return;
            }
        }

        /// <summary>
        /// Goes the back.
        /// </summary>
        public static void GoBack()
        {
            if (!Initialized)
            {
                Logger.WriteLine("Can't go back. NavigationService is not initialized.");
                return;
            }

            if (!CanGoBack)
            {
                Logger.WriteLine("Can't go backward. No pages are front in the stack.");
                return;
            }

            Headers.Pop();
            Frame.GoBack();
        }

        /// <summary>
        /// Goes the forward.
        /// </summary>
        public static void GoForward()
        {
            if (!Initialized)
            {
                Logger.WriteLine("Can't go forward. NavigationService is not initialized.");
                return;
            }

            if (!CanGoForward)
            {
                Logger.WriteLine("Can't go forward. No pages are front in the stack.");
                return;
            }

            Frame.GoForward();
        }

        /// <summary>
        /// Locks this instance.
        /// </summary>
        public static void Lock()
        {
            if (!Initialized)
            {
                Logger.WriteLine("Can't lock navigator. NavigationService is not initialized.");
                return;
            }

            NavigationView.IsEnabled = false;
            ProgressRing.IsActive = true;
        }

        /// <summary>
        /// Unlocks this instance.
        /// </summary>
        public static void Unlock()
        {
            if (!Initialized)
            {
                Logger.WriteLine("Can't unlock navigator. NavigationService is not initialized.");
                return;
            }

            NavigationView.IsEnabled = true;
            ProgressRing.IsActive = false;
        }

        /// <summary>
        /// Locks the frame.
        /// </summary>
        public static void LockFrame()
        {
            if (!Initialized)
            {
                Logger.WriteLine("Can't lock frame. NavigationService is not initialized.");
                return;
            }

            Frame.IsEnabled = false;
            ProgressRing.IsActive = true;
        }

        /// <summary>
        /// Unlocks the frame.
        /// </summary>
        public static void UnlockFrame()
        {
            if (!Initialized)
            {
                Logger.WriteLine("Can't unlock frame. NavigationService is not initialized.");
                return;
            }

            Frame.IsEnabled = true;
            ProgressRing.IsActive = false;
        }
    }
}
