using CodeShare.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace CodeShare.Uwp.Utilities
{
    public class ImageUtilities
    {
        public static async Task<T> CreateNewImageAsync<T>(StorageFile storageFile, string description = "") where T : WebImage, ICroppableImage, new()
        {
            var imageInBytes = await StorageUtilities.ConvertStorageFileToByteArrayAsync(storageFile);

            using (var stream = await storageFile.OpenReadAsync())
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);

                return (T)Activator.CreateInstance(typeof(T), new object[]
                {
                    (int)decoder.PixelWidth,
                    (int)decoder.PixelHeight,
                    imageInBytes,
                    storageFile.FileType
                });
            }
        }

        public static async Task<ImageSource> WebImageToCroppedImage<T>(T webImage) where T : WebImage, ICroppableImage
        {
            // Convert start point and size to unsigned integer. 
            uint startPointX = (uint)webImage.Crop.X;
            uint startPointY = (uint)webImage.Crop.Y;
            uint height = (uint)webImage.Crop.Height;
            uint width = (uint)webImage.Crop.Width;

            var bitmap = new BitmapImage(webImage.Url);
            var randomAccessStreamReference = RandomAccessStreamReference.CreateFromUri(bitmap.UriSour‌​ce);

            using (var stream = await randomAccessStreamReference.OpenReadAsync())
            {
                // Create a decoder from the stream. With the decoder, we can get the properties of the image. 
                var decoder = await BitmapDecoder.CreateAsync(stream);

                // Create the bitmap bounds
                var bitmapBounds = new BitmapBounds
                {
                    X = startPointX,
                    Y = startPointY,
                    Height = height,
                    Width = width
                };

                // Create cropping BitmapTransform. 
                var bitmapTransform = new BitmapTransform
                {
                    Bounds = bitmapBounds,
                    ScaledWidth = width,
                    ScaledHeight = height
                };

                // Get the cropped pixels within the bounds of transform. 
                PixelDataProvider pixelDataProvider = await decoder.GetPixelDataAsync(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Straight,
                    bitmapTransform,
                    ExifOrientationMode.IgnoreExifOrientation,
                    ColorManagementMode.ColorManageToSRgb);

                byte[] pixels = pixelDataProvider.DetachPixelData();

                // Stream the bytes into a WriteableBitmap 
                WriteableBitmap cropBmp = new WriteableBitmap((int)width, (int)height);
                Stream pixStream = cropBmp.PixelBuffer.AsStream();
                pixStream.Write(pixels, 0, (int)(width * height * 4));

                return cropBmp;
            }
        }
    }
}
