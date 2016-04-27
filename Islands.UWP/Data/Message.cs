using System;
using Windows.UI.Popups;

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
    }
}
