// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="StorageUtilities.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CodeShare.Model;
using CodeShare.Utilities;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace CodeShare.Uwp.Utilities
{
    /// <summary>
    /// Class StorageUtilities.
    /// </summary>
    public static class StorageUtilities
    {
        /// <summary>
        /// Gets the application folder.
        /// </summary>
        /// <value>The application folder.</value>
        public static StorageFolder AppFolder => ApplicationData.Current.LocalFolder;
        /// <summary>
        /// Gets the installation folder.
        /// </summary>
        /// <value>The installation folder.</value>
        public static StorageFolder InstallationFolder => Windows.ApplicationModel.Package.Current.InstalledLocation;

        /// <summary>
        /// Gets the image picker.
        /// </summary>
        /// <value>The image picker.</value>
        public static Windows.Storage.Pickers.FileOpenPicker ImagePicker => new Windows.Storage.Pickers.FileOpenPicker
        {
            ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
            SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary,
            FileTypeFilter = { ".jpg", ".jpeg", ".png", ".gif" }
        };

        /// <summary>
        /// Gets the file picker.
        /// </summary>
        /// <value>The file picker.</value>
        public static Windows.Storage.Pickers.FileOpenPicker FilePicker => new Windows.Storage.Pickers.FileOpenPicker
        {
            ViewMode = Windows.Storage.Pickers.PickerViewMode.List,
            SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary,
            FileTypeFilter = {"*"}
        };

        /// <summary>
        /// Gets the save picker.
        /// </summary>
        /// <value>The save picker.</value>
        public static Windows.Storage.Pickers.FileSavePicker SavePicker
        {
            get
            {
                var picker = new Windows.Storage.Pickers.FileSavePicker
                {
                    SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary,
                    SuggestedFileName = "file"
                };
                picker.FileTypeChoices.Add("Image", new List<string>() { ".jpg", ".png", ".jpeg", ".gif" });

                return picker;
            }
        }

        /// <summary>
        /// Gets the folder picker.
        /// </summary>
        /// <value>The folder picker.</value>
        public static Windows.Storage.Pickers.FolderPicker FolderPicker
        {
            get
            {
                var picker = new Windows.Storage.Pickers.FolderPicker
                {
                    SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop,
                };
                picker.FileTypeFilter.Add("*");

                return picker;
            }
        }

        /// <summary>
        /// Saves the web file to storage folder.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="webFile">The web file.</param>
        /// <param name="storageFolder">The storage folder.</param>
        /// <returns>Task.</returns>
        internal static async Task SaveWebFileToStorageFolder<T>(T webFile, StorageFolder storageFolder) where T : class, IWebFile, new()
        {
            var sampleFile = await storageFolder.CreateFileAsync(webFile.Path, CreationCollisionOption.GenerateUniqueName);
            await FileIO.WriteBytesAsync(sampleFile, await webFile.DownloadAsync());
        }

        /// <summary>
        /// Picks the multiple images.
        /// </summary>
        /// <returns>Task&lt;IReadOnlyList&lt;StorageFile&gt;&gt;.</returns>
        public static async Task<IReadOnlyList<StorageFile>> PickMultipleImages() => await ImagePicker.PickMultipleFilesAsync();

        /// <summary>
        /// Picks the single image.
        /// </summary>
        /// <returns>Task&lt;StorageFile&gt;.</returns>
        public static async Task<StorageFile> PickSingleImage() => await ImagePicker.PickSingleFileAsync();

        /// <summary>
        /// Picks the single file.
        /// </summary>
        /// <returns>Task&lt;StorageFile&gt;.</returns>
        public static async Task<StorageFile> PickSingleFile() => await FilePicker.PickSingleFileAsync();

        /// <summary>
        /// Picks the multiple files.
        /// </summary>
        /// <returns>Task&lt;IReadOnlyList&lt;StorageFile&gt;&gt;.</returns>
        public static async Task<IReadOnlyList<StorageFile>> PickMultipleFiles() => await FilePicker.PickMultipleFilesAsync();

        /// <summary>
        /// Picks the file destination.
        /// </summary>
        /// <returns>Task&lt;StorageFile&gt;.</returns>
        public static async Task<StorageFile> PickFileDestination() => await SavePicker.PickSaveFileAsync();

        /// <summary>
        /// Picks the folder destination.
        /// </summary>
        /// <returns>Task&lt;StorageFolder&gt;.</returns>
        public static async Task<StorageFolder> PickFolderDestination() => await FolderPicker.PickSingleFolderAsync();

        /// <summary>
        /// Gets the storage file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Task&lt;StorageFile&gt;.</returns>
        public static async Task<StorageFile> GetStorageFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Logger.WriteLine("GetStorageFile: Provided file path was empty.");
                return null;
            }

            try
            {
                return await StorageFile.GetFileFromPathAsync(filePath);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the storage folder.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <returns>Task&lt;StorageFolder&gt;.</returns>
        public static async Task<StorageFolder> GetStorageFolder(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                Logger.WriteLine("GetStorageFolder: Provided folder path was empty.");
                return null;
            }

            try
            {
                return await StorageFolder.GetFolderFromPathAsync(folderPath);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts the file to bitmap image.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>BitmapImage.</returns>
        public static BitmapImage ConvertFileToBitmapImage(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Logger.WriteLine("ConvertFileToBitmapImage: Provided file path was empty.");
                return null;
            }

            var fileInBytes = ConvertFileToByteArray(filePath);

            return ConvertByteArrayToBitmapImage(fileInBytes);
        }

        /// <summary>
        /// Files to byte array.
        /// </summary>
        /// <param name="filePath">The filepath.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ConvertFileToByteArray(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Logger.WriteLine("ConvertFileToByteArray: Provided path was empty.");
                return null;
            }

            using (var stream = System.IO.File.OpenRead($@"{Windows.ApplicationModel.Package.Current.InstalledLocation.Path}\{filePath}"))
            {
                var fileBytes = new byte[stream.Length];

                stream.Read(fileBytes, 0, fileBytes.Length);
                return fileBytes;
            }
        }

        /// <summary>
        /// Converts the file to string.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>System.String.</returns>
        public static string ConvertFileToString(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Logger.WriteLine("ConvertFileToString: Provided path was empty.");
                return null;
            }

            try
            {
                return System.IO.File.ReadAllText($@"{Windows.ApplicationModel.Package.Current.InstalledLocation.Path}\{filePath}");
            }
            catch (Exception e)
            {
                Logger.WriteLine($"ConvertFileToString: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Converts the storage file to bitmap image.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Task&lt;BitmapImage&gt;.</returns>
        public static async Task<BitmapImage> ConvertStorageFileToBitmapImage(StorageFile file)
        {
            if (file == null)
            {
                Logger.WriteLine("ConvertStorageFileToBitmapImage: Provided storage file was empty.");
                return null;
            }

            BitmapImage bitmapImage = new BitmapImage();
            using (var stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read))
            {
                await bitmapImage.SetSourceAsync(stream);
            }

            return bitmapImage;
        }

        /// <summary>
        /// Convert from storage file to byte array.
        /// </summary>
        /// <param name="savedStorageFile">The saved storage file.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public static async Task<byte[]> ConvertStorageFileToByteArrayAsync(StorageFile savedStorageFile)
        {
            using (var stream = await savedStorageFile.OpenStreamForReadAsync())
            {
                var bytes = new byte[(int)stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                return bytes;
            }   
        }

        /// <summary>
        /// Bitmaps the image to byte array.
        /// </summary>
        /// <param name="bitmapImage">The bitmap image.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public static async Task<byte[]> ConvertBitmapImageToByteArrayAsync(BitmapImage bitmapImage)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(bitmapImage.UriSource);
            using (var inputStream = await file.OpenSequentialReadAsync())
            {
                var readStream = inputStream.AsStreamForRead();

                var byteArray = new byte[readStream.Length];
                await readStream.ReadAsync(byteArray, 0, byteArray.Length);
                return byteArray;
            }
        }

        /// <summary>
        /// Bytes the array to bitmap image.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>BitmapImage.</returns>
        public static BitmapImage ConvertByteArrayToBitmapImage(byte[] bytes)
        {
            using (var ms = new InMemoryRandomAccessStream())
            {
                using (var writer = new DataWriter(ms.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(bytes);
                    writer.StoreAsync().GetResults();
                }
                var image = new BitmapImage();
                image.SetSource(ms);
                return image;
            }
        }
    }
}
