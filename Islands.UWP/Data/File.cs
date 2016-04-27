using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
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
            try
            {
                var stream = await ReadFileRandomAccessStreamWithContentTypeAsync(path);
                if (stream != null)
                {
                    var bitmap = new BitmapImage();
                    await bitmap.SetSourceAsync(stream);
                    image.Source = bitmap;
                }
            }
            catch
            {
                image.Source = new BitmapImage(new Uri(Config.FailedImageUri, UriKind.RelativeOrAbsolute));
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

        public static async void SaveFile(string urlStr)
        {
            var uri = new Uri(urlStr);
            string filename = Path.GetFileName(urlStr);
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.FileTypeChoices.Add("Image", new List<string>() { ".jpg", ".jpeg", ".png", ".bmp", ".gif" });
            savePicker.SuggestedFileName = filename;

            //StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                var downloadFile = await StorageFile.CreateStreamedFileFromUriAsync(filename, uri, RandomAccessStreamReference.CreateFromUri(uri));
                var readStream = await downloadFile.OpenReadAsync();
                var inStream = readStream.AsStreamForRead().AsInputStream();
                await downloadFile.CopyAndReplaceAsync(file);
                //var buffer = await FileIO.ReadBufferAsync(downloadFile);
                //await FileIO.WriteBufferAsync(file, buffer);
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == FileUpdateStatus.Failed)
                {
                    Message.ShowMessage("图片保存失败");
                }
                else
                {
                    Message.ShowMessage("图片保存成功");
                }
            }

        }
    }
}
