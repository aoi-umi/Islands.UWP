﻿using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
            InputScope inputScope = new InputScope();
            inputScope.Names.Add(new InputScopeName() { NameValue = InputScopeNameValue.Number });
            TextBox inputBox = new TextBox() { InputScope = inputScope, VerticalContentAlignment = VerticalAlignment.Center }; 
             var dialog = new ContentDialog()
            {
                Title = "输入页数",
                Content = inputBox,
                FullSizeDesired = false,
                PrimaryButtonText = "跳页",
                SecondaryButtonText = "取消",                
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
            InputScope inputScope = new InputScope();
            inputScope.Names.Add(new InputScopeName() { NameValue = InputScopeNameValue.Number });
            TextBox inputBox = new TextBox() { InputScope = inputScope, VerticalContentAlignment = VerticalAlignment.Center };
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

        public static async Task<int> ShowRef(string title, object content)
        {
            try
            {
                ScrollViewer sv = new ScrollViewer() { MaxHeight = 500, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
                sv.Content = content;
                var dialog = new ContentDialog()
                {
                    Title = ">>No." + title,
                    Content = sv,
                    FullSizeDesired = true,
                    PrimaryButtonText = "关闭"
                };
                ContentDialogResult result = await dialog.ShowAsync();
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        public static string GetRandomErrorMessage()
        {
            int n = new Random().Next(0, Config.ErrorMessage.Count);
            return Config.ErrorMessage[n];
        }
    }
}
