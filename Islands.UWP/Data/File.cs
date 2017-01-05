using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Islands.UWP.Data
{
    public static class File
    {
        public static async Task<string> SetLocalImage(Image image)
        {
            var imageUri = "";
            FileOpenPicker Picker = new FileOpenPicker();
            Picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            Picker.ViewMode = PickerViewMode.List;
            Picker.FileTypeFilter.Add(".jpg");
            Picker.FileTypeFilter.Add(".jpeg");
            Picker.FileTypeFilter.Add(".bmp");
            Picker.FileTypeFilter.Add(".gif");
            Picker.FileTypeFilter.Add(".png");
            StorageFile file = await Picker.PickSingleFileAsync();
            if (file != null)
            {
                if (image != null)
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", file);
                    var bitmap = new BitmapImage();
                    var stream = await file.OpenReadAsync();
                    await bitmap.SetSourceAsync(stream);
                    image.Source = bitmap;
                }
                imageUri = file.Path;
            }
            return imageUri;
        }

        public static async Task<string> CopyImageToLocal(string toFileName)
        {
            var imageUri = "";
            FileOpenPicker Picker = new FileOpenPicker();
            Picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            Picker.ViewMode = PickerViewMode.List;
            Picker.FileTypeFilter.Add(".jpg");
            Picker.FileTypeFilter.Add(".jpeg");
            Picker.FileTypeFilter.Add(".bmp");
            Picker.FileTypeFilter.Add(".gif");
            Picker.FileTypeFilter.Add(".png");
            StorageFile file = await Picker.PickSingleFileAsync();
            if (file != null && !string.IsNullOrEmpty(toFileName))
            {
                //toFileName += file.Name.GetExt();
                StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(Config.SavedImageFolder, CreationCollisionOption.OpenIfExists);
                StorageFile outfile = await folder.CreateFileAsync(toFileName, CreationCollisionOption.ReplaceExisting);
                if (outfile != null)
                {
                    CachedFileManager.DeferUpdates(file);
                    await file.CopyAndReplaceAsync(outfile);
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                }
                imageUri = outfile.Path;
            }
            return imageUri;
        }

        public static async Task<string> SetPath()
        {
            FolderPicker Picker = new FolderPicker();
            Picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            Picker.FileTypeFilter.Add(".");
            StorageFolder Folder = await Picker.PickSingleFolderAsync();
            return Folder.Path;
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
                    if (bitmap != null && bitmap.PixelWidth < Config.MaxImageWidth) image.Stretch = Stretch.None;
                    image.Source = bitmap;
                }
            }
            catch(Exception ex)
            {
                image.Source = new BitmapImage(new Uri(Config.FailedImageUri, UriKind.RelativeOrAbsolute));
            }
        }

        public static async Task<Stream> ReadFileStreamAsync(string path)
        {
            var stream = await ReadFileRandomAccessStreamWithContentTypeAsync(path);
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
            SaveFile(file, filename, uri);
        }

        public static async void SaveFileWithoutDialog(string urlStr)
        {
            var uri = new Uri(urlStr);
            string filename = Path.GetFileName(urlStr);
            StorageFolder folder = await KnownFolders.SavedPictures.CreateFolderAsync(Config.SavedImageFolder, CreationCollisionOption.OpenIfExists);
            StorageFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            SaveFile(file, filename, uri);
        }

        private static async void SaveFile(StorageFile file, string filename, Uri uri)
        {
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                var downloadFile = await StorageFile.CreateStreamedFileFromUriAsync(filename, uri, RandomAccessStreamReference.CreateFromUri(uri));
                await downloadFile.CopyAndReplaceAsync(file);
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

        public static async Task<string> SaveTextToImage(string filename, RenderTargetBitmap bitmap)
        {            
            var folder = await ApplicationData.Current.LocalFolder.CreateFoldersAsync(Config.SendImageFolder);
            StorageFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            var buffer = await bitmap.GetPixelsAsync();
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encod = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                encod.SetPixelData(BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Ignore,
                (uint)bitmap.PixelWidth,
                (uint)bitmap.PixelHeight,
                DisplayInformation.GetForCurrentView().LogicalDpi,
                DisplayInformation.GetForCurrentView().LogicalDpi,
                buffer.ToArray()
               );
                await encod.FlushAsync();
            }
            return file.Path;
        }

        public static async Task<bool> TryCopyImage(this StorageFolder sf, string sourceFileName, string folderName, string destFileName)
        {
            bool result = true;
            try
            {
                if (sourceFileName == Path.Combine(sf.Path, folderName, destFileName)) return false;
                await sf.CreateFoldersAsync(folderName);
                await Task.Run(() =>
                {
                    System.IO.File.Copy(sourceFileName, Path.Combine(sf.Path, folderName, destFileName), true);
                });
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public static async Task<StorageFolder> CreateFoldersAsync(this StorageFolder sf, string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName)) return sf;
            var s = folderName.Split(new string[] { @"\","/" }, StringSplitOptions.RemoveEmptyEntries);
            if (s == null || s.Length < 1) return sf;
            StorageFolder folder = sf;
            foreach (var x in s)
            {
                if (folder == null) return folder;
                folder = await folder.CreateFolderAsync(x, CreationCollisionOption.OpenIfExists);
            }
            return folder;
        }
    }
}
