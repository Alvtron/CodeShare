﻿using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class AddQuestionDialog : ContentDialog
    {
        private AddQuestionViewModel ViewModel { get; }

        public AddQuestionDialog()
        {
            if (AuthService.CurrentUser == null)
            {
                Hide();
                return;
            }

            ViewModel = new AddQuestionViewModel();
            InitializeComponent();
        }

        public void ASB_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                ViewModel.FilterCodeLanguage(sender.Text.ToLower());
        }

        private void ASB_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            sender.IsSuggestionListOpen = false;
            ViewModel.SubmitCodeLanguage(args.ChosenSuggestion);
        }

        private async void ASB_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.Initialize();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (await ViewModel.UploadQuestionAsync())
            {
                Hide();
            }
        }
    }
}
