using System;
using CodeShare.Model;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Views;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using CodeShare.RestApi;
using CodeShare.Utilities;

namespace CodeShare.Uwp.ViewModels
{
    public abstract class ContentViewModel<TContent> : ObservableObject where TContent : IContent
    {
        public bool IsUserAuthor { get; }
        public abstract bool OnSetAuthorPrivileges(TContent model);

        private TContent _model;
        public TContent Model
        {
            get => _model;
            set => SetField(ref _model, value);
        }

        private RelayCommand _reportCommand;
        public ICommand ReportCommand => _reportCommand = _reportCommand ?? new RelayCommand(async param => await ReportAsync());

        private RelayCommand<Video> _viewVideoCommand;
        public ICommand ViewVideoCommand => _viewVideoCommand = _viewVideoCommand ?? new RelayCommand<Video>(ViewVideo);

        private RelayCommand<WebFile> _viewImageCommand;
        public ICommand ViewImageCommand => _viewImageCommand = _viewImageCommand ?? new RelayCommand<WebFile>(image => ViewImage(image));

        public ContentViewModel(TContent model)
        {
            Model = model;
            IsUserAuthor = OnSetAuthorPrivileges(model);
        }

        private void ViewVideo(Video video)
        {
            NavigationService.Navigate(typeof(MediaPage), video, video.Title);
        }

        private void ViewImage(WebFile image)
        {
            NavigationService.Navigate(typeof(MediaPage), image, image.Path);
        }

        public async Task IncrementViewsAsync()
        {
            Model.Views++;

            if (!await RestApiService<TContent>.Update(Model, Model.Uid))
            {
                Logger.WriteLine($"Failed to increment view counter for code {Model.Uid}.");
                Model.Views--;
            }
        }

        public async Task ReportAsync()
        {
            if (AuthService.CurrentUser == null || Model == null)
            {
                await NotificationService.DisplayErrorMessage("Something went wrong. Try again later.");
                return;
            }

            var reportDialog = new ReportDialog(Model?.Name);

            var dialogResult = await reportDialog.ShowAsync();

            if (dialogResult != ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(reportDialog.Message))
            {
                await NotificationService.DisplayErrorMessage($"Please provide a reason for why you want to report '{Model?.Name}'.");
                return;
            }

            if (!await RestApiService<Report>.Add(new Report(Model, reportDialog.Message)))
            {
                await NotificationService.DisplayErrorMessage("We where unable to upload that report. Sorry about that. Please try again later.");
                return;
            }

            await NotificationService.DisplayThankYouMessage("Thanks for contributing to a better community for everyone! You rock!");
        }
    }
}