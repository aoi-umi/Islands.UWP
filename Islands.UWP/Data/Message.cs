using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Islands.UWP.Data
{
    public static class Message
    {
        public static async void ShowMessage(string message)
        {
            await new MessageDialog(message).ShowAsync();
        }

        public static async void ShowMessage(string message, string title)
        {
            await new MessageDialog(message, title).ShowAsync();
        }

        public static async Task<int> GotoPageYesOrNo()
        {
            TextBox inputBox = new TextBox();
            var dialog = new ContentDialog()
            {
                Title = "输入页数",
                Content = inputBox,
                FullSizeDesired = false,
                PrimaryButtonText = "跳页",
                SecondaryButtonText = "取消"
            };
            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                int page;
                if (int.TryParse(inputBox.Text, out page) && page > 0)
                    return page;
                else
                {
                    ShowMessage("请输入大于0的数字");
                    return 0;
                }
            }
            else
                return 0;
        }

        public static async Task<int> GotoThreadYesOrNo()
        {
            TextBox inputBox = new TextBox();
            var dialog = new ContentDialog()
            {
                Title = "输入串号",
                Content = inputBox,
                FullSizeDesired = false,
                PrimaryButtonText = "跳转",
                SecondaryButtonText = "取消"
            };
            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                int page;
                if (int.TryParse(inputBox.Text, out page) && page > 0)
                    return page;
                else
                {
                    ShowMessage("请输入大于0的数字");
                    return 0;
                }
            }
            else
                return 0;
        }
    }
}
