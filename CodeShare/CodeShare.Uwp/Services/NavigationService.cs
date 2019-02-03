using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Utilities;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Services
{
    public static class NavigationService
    {
        private static Frame Frame { get; set; }
        private static NavigationView NavigationView { get; set; }
        private static ProgressRing ProgressRing { get; set; }

        private static ObservableStack<string> Headers { get; } = new ObservableStack<string>();
        private static Dictionary<Type, Action<Type, object, string>> Navigations { get; set; }

        public static bool Initialized => Frame != null && NavigationView != null && ProgressRing != null && Headers != null;

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

        public static bool CanGoBack => Frame?.CanGoBack ?? false;

        public static bool CanGoForward => Frame?.CanGoForward ?? false;

        public static void SetHeaderTitle(string title) => NavigationView.Header = title;

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

        public static void Clear()
        {
            Frame.BackStack.Clear();
            Headers.Clear();
            UpdateBackButtonVisibillity();
        }

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
                    var comment = await RestApiService<Reply>.Get((Guid)parameter);
                    var dialog = new ReplyDialog(comment);
                    await dialog.ShowAsync();
                    return;
                default:
                    break;
            }
        }

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

        private static void GoForward()
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
