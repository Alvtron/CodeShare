using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeShare.Uwp.ViewModels
{
    public abstract class EditorViewModel : BaseViewModel
    {
        private bool _changed;
        public bool Changed
        {
            get => _changed;
            set => SetField(ref _changed, value);
        }
        public abstract Task UploadImagesAsync();
        public abstract Task UploadVideoAsync();
        public abstract void DeleteImage(WebFile screenshot);
        public abstract void DeleteVideo(Video video);
        public abstract Task SaveAsync();
        public abstract void Reset();
        
        private RelayCommand _uploadImagesCommand;
        public ICommand UploadImagesCommand => _uploadImagesCommand = _uploadImagesCommand ?? new RelayCommand(async param => await UploadImagesAsync());

        private RelayCommand _uploadVideoCommand;
        public ICommand UploadVideoCommand => _uploadVideoCommand = _uploadVideoCommand ?? new RelayCommand(async param => await UploadVideoAsync());

        private RelayCommand<WebFile> _deleteImageCommand;
        public ICommand DeleteImageCommand => _deleteImageCommand = _deleteImageCommand ?? new RelayCommand<WebFile>(param => DeleteImage(param));

        private RelayCommand<Video> _deleteVideoCommand;
        public ICommand DeleteVideoCommand => _deleteVideoCommand = _deleteVideoCommand ?? new RelayCommand<Video>(param => DeleteVideo(param));

        private RelayCommand _saveCommand;
        public ICommand SaveCommand => _saveCommand = _saveCommand ?? new RelayCommand(async param => await SaveAsync());

        private RelayCommand _resetCommand;
        public ICommand ResetCommand => _resetCommand = _resetCommand ?? new RelayCommand(param => Reset());
    }
}
