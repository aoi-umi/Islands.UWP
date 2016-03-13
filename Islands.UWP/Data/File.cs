using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Islands.UWP.Data
{
    public static class File
    {
        public static async Task<string> SetLocalImage(Image image)
        {
            var imageUri = "";
            FileOpenPicker openFile = new FileOpenPicker();
            openFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openFile.ViewMode = PickerViewMode.List;
            openFile.FileTypeFilter.Add(".jpg");
            openFile.FileTypeFilter.Add(".jpeg");
            openFile.FileTypeFilter.Add(".bmp");
            openFile.FileTypeFilter.Add(".gif");
            openFile.FileTypeFilter.Add(".png");
            StorageFile file = await openFile.PickSingleFileAsync();
            if (file != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", file);
                var bitmap = new BitmapImage();
                var stream = await file.OpenReadAsync();
                await bitmap.SetSourceAsync(stream);
                image.Source = bitmap;
                imageUri = file.Path;
            }
            return imageUri;
        }
        public static async void SetLocalImage(Image image, string path)
        {
            var stream = await ReadFileRandomAccessStreamWithContentTypeAsync(path);
            if (stream != null)
            {
                var bitmap = new BitmapImage();
                await bitmap.SetSourceAsync(stream);
                image.Source = bitmap;
            }
        }

        public static async Task<Stream> ReadFileStreamAsync(string path)
        {
            var file = await StorageFile.GetFileFromPathAsync(path);
            StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", file);
            var stream = await file.OpenReadAsync();
            return stream.AsStream();
        }

        public static async Task<IRandomAccessStreamWithContentType> ReadFileRandomAccessStreamWithContentTypeAsync(string path)
        {
            var file = await StorageFile.GetFileFromPathAsync(path);
            StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", file);
            var stream = await file.OpenReadAsync();
            return stream;
        }
    }
}
