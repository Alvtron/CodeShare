using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Data;
using CodeShare.Uwp.Dialogs;
using Windows.Storage;

namespace CodeShare.Uwp.ViewModels
{
    public class ImageManager<T> : ObservableObject where T : WebImage
    {
        private RelayCommand<IList<object>> _downloadImagesCommand;
        public ICommand DownloadImagesCommand => _downloadImagesCommand = _downloadImagesCommand ?? new RelayCommand<IList<object>>(async images => await DownloadImagesAsync(images));

        private RelayCommand<IList<object>> _deleteImagesCommand;
        public ICommand DeleteImagesCommand => _deleteImagesCommand = _deleteImagesCommand ?? new RelayCommand<IList<object>>(images => DeleteImagesAsync(images));

        public ObservableCollection<T> Images { get; }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetField(ref _isBusy, value);
        }

        public ImageManager(ObservableCollection<T> images)
        {
            Images = images;
        }

        private async Task DownloadImagesAsync(IList<object> objects)
        {
            if (objects == null)
            {
                return;
            }

            IsBusy = true;

            var images = objects.Cast<ProfilePicture>();

            var folder = await StorageUtilities.PickFolderDestination();

            if (folder == null)
            {
                return;
            }

            foreach (var image in images)
            {
                await StorageUtilities.SaveWebFileToStorageFolder(image, folder);
            }

            IsBusy = false;
        }

        private void DeleteImagesAsync(IList<object> objects)
        {
            if (objects == null)
            {
                return;
            }

            IsBusy = true;

            var images = objects.Cast<T>();

            foreach (var image in images)
            {
                Images.Remove(image);
                image.Delete();
            }

            IsBusy = false;
        }
    }
}
