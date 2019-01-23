using CodeShare.Model;
using CodeShare.Uwp.Controls;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Xaml;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.ViewModels
{
    public abstract class ContentPageViewModel : BaseViewModel
    {
        private bool _isUserAuthor;
        internal bool IsUserAuthor
        {
            get => _isUserAuthor;
            set => SetField(ref _isUserAuthor, value);
        }
        public abstract Task<bool> Refresh();
        public abstract Task ReportAsync();
        public abstract void ViewVideo(Video video);
        public abstract void ViewImage(WebFile image);
        public abstract void LogClick(ILog log);

        private RelayCommand _reportCommand;
        public ICommand ReportCommand => _reportCommand = _reportCommand ?? new RelayCommand(async param => await ReportAsync());

        private RelayCommand<Video> _viewVideoCommand;
        public ICommand ViewVideoCommand => _viewVideoCommand = _viewVideoCommand ?? new RelayCommand<Video>(ViewVideo);

        private RelayCommand<WebFile> _viewImageCommand;
        public ICommand ViewImageCommand => _viewImageCommand = _viewImageCommand ?? new RelayCommand<WebFile>(ViewImage);

        private RelayCommand<ILog> _logClickCommand;
        public ICommand LogClickCommand => _logClickCommand = _logClickCommand ?? new RelayCommand<ILog>(param => LogClick(param));
    }
}
