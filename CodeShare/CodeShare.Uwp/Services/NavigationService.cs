using CodeShare.Model;
using CodeShare.Uwp.Views;
using System;
using System.Diagnostics;
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

        public static bool Initialized => Frame != null && NavigationView != null && ProgressRing != null && Headers != null;

        public static void Initialize(Frame frame, NavigationView navigationView, ProgressRing progressRing)
        {
            Debug.WriteLine("Initializing NavigationService...");

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
                Debug.WriteLine("NavigationService was not properly initialized.");
            else
                Debug.WriteLine("NavigationService was properly initialized.");
        }

        public static bool CanGoBack => Frame?.CanGoBack ?? false;

        public static bool CanGoForward => Frame?.CanGoForward ?? false;

        public static void SetHeaderTitle(string title) => NavigationView.Header = title;

        public static void UpdateBackButtonVisibillity()
        {
            if (!Initialized)
            {
                Debug.WriteLine("Can't update back-button visibillity. NavigationService is not initialized.");
                return;
            }

            NavigationView.IsBackButtonVisible = Frame.CanGoBack ?
                NavigationViewBackButtonVisible.Auto : NavigationViewBackButtonVisible.Collapsed;
        }

        public static void Clear()
        {
            Frame.BackStack.Clear();
            Headers.Clear();
            UpdateBackButtonVisibillity();
        }

        public static void Navigate(Type viewType, object parameter, string newHeader)
        {
            if (!Initialized)
            {
                Debug.WriteLine("Can't navigate. NavigationService is not initialized.");
                return;
            }

            Headers.Push(newHeader);
            Frame?.Navigate(viewType, parameter);

            if (viewType != typeof(HomePage))
                return;

            Clear();
            Headers.Push(newHeader);
        }

        public static void Navigate(Type viewType, object parameter)
        {
            if (!Initialized)
            {
                Debug.WriteLine("Can't navigate. NavigationService is not initialized.");
                return;
            }

            if (viewType == typeof(HomePage))
                Navigate(typeof(HomePage), parameter, "Home");

            else if (viewType == typeof(CodesPage))
                Navigate(typeof(CodesPage), parameter, "Codes");

            else if (viewType == typeof(QuestionsPage))
                Navigate(typeof(QuestionsPage), parameter, "Questions");

            else if (viewType == typeof(UsersPage))
                Navigate(typeof(UsersPage), parameter, "Users");
        }

        public static void Navigate(string pageName)
        {
            if (!Initialized)
            {
                Debug.WriteLine("Can't navigate. NavigationService is not initialized.");
                return;
            }

            switch (pageName)
            {
                case "Home":
                    Navigate(typeof(HomePage), null, "Home");
                    break;
                case "Codes":
                    Navigate(typeof(CodesPage), null, "Codes");
                    break;
                case "Questions":
                    Navigate(typeof(QuestionsPage), null, "Questions");
                    break;
                case "Users":
                    Navigate(typeof(UsersPage), null, "Users");
                    break;
                default:
                    break;
            }
        }

        public static void GoBack()
        {
            if (!Initialized)
            {
                Debug.WriteLine("Can't go back. NavigationService is not initialized.");
                return;
            }

            if (!CanGoBack)
            {
                Debug.WriteLine("Can't go backward. No pages are front in the stack.");
                return;
            }

            Headers.Pop();
            Frame.GoBack();
        }

        private static void GoForward()
        {
            if (!Initialized)
            {
                Debug.WriteLine("Can't go forward. NavigationService is not initialized.");
                return;
            }

            if (!CanGoForward)
            {
                Debug.WriteLine("Can't go forward. No pages are front in the stack.");
                return;
            }

            Frame.GoForward();
        }

        public static void Lock()
        {
            if (!Initialized)
            {
                Debug.WriteLine("Can't lock navigator. NavigationService is not initialized.");
                return;
            }

            NavigationView.IsEnabled = false;
            ProgressRing.IsActive = true;
        }

        public static void Unlock()
        {
            if (!Initialized)
            {
                Debug.WriteLine("Can't unlock navigator. NavigationService is not initialized.");
                return;
            }

            NavigationView.IsEnabled = true;
            ProgressRing.IsActive = false;
        }

        public static void LockFrame()
        {
            if (!Initialized)
            {
                Debug.WriteLine("Can't lock frame. NavigationService is not initialized.");
                return;
            }

            Frame.IsEnabled = false;
            ProgressRing.IsActive = true;
        }

        public static void UnlockFrame()
        {
            if (!Initialized)
            {
                Debug.WriteLine("Can't unlock frame. NavigationService is not initialized.");
                return;
            }

            Frame.IsEnabled = true;
            ProgressRing.IsActive = false;
        }
    }
}
