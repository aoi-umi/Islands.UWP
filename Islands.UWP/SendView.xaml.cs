using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class SendView : UserControl
    {
        public class PostModel {
            public IslandsCode islandCode { get; set; }
            public bool IsMain { get; set; }
            public string Host { get; set; }
            public Model.CookieModel Cookie { get; set; }
            public string Id { get; set; }
            public string Api { get; set; }
            public PostModel() { }
        }
        public delegate void ResponseEventHandler(bool Success, Model.SendModel send);
        public event ResponseEventHandler Response;
        void OnResponse(bool Success, Model.SendModel send)
        {
            if (Response != null) Response(Success, send);
        }

        public delegate void SendClickEventHandler(object sender, RoutedEventArgs e);
        public event SendClickEventHandler SendClick;
        void OnSendClick(object sender, RoutedEventArgs e)
        {
            if (Response != null) SendClick(sender, e);
            this.IsHitTestVisible = true;
        }

        public IslandsCode islandCode { get; set; }
        public string title
        {
            set
            {
                Title.Text = "发送(" + value + ")";
            }
        }

        string txtSendTitle
        {
            get { return SendTitle.Text; }
            set { SendTitle.Text = value; }
        }
        string txtSendEmail
        {
            get { return SendEmail.Text; }
            set { SendEmail.Text = value; }
        }
        string txtSendName
        {
            get { return SendName.Text; }
            set { SendName.Text = value; }
        }
        string txtSendContent
        {
            get
            {
                string outString;
                SendContent.Document.GetText(TextGetOptions.None, out outString);
                return outString.Trim();
            }
            set { SendContent.Document.SetText(TextSetOptions.None, value); }
        }
        string txtImageUri
        {
            get { return SendImageStr.Text; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    SendImage.Source = null;
                SendImageStr.Text = value;
            }
        }

        public PostModel postModel { get; set; }
        public SendView()
        {
            this.InitializeComponent();
            EmojiBox.Items.Add("颜文字");
            EmojiBox.SelectedIndex = 0;
            foreach(var emoji in Config.Emoji)
                EmojiBox.Items.Add(emoji);            
        }

        private void EmptyButton_Click(object sender, RoutedEventArgs e)
        {
            txtSendTitle = "";
            txtSendEmail = "";
            txtSendName = "";
            txtSendContent = "";
            txtImageUri = "";
        }

        private async void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtImageUri)) txtImageUri = string.Empty;
            else txtImageUri = await Data.File.SetLocalImage(SendImage);
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(postModel.Id))
                    throw new Exception("无法回复空串");
                var send = new Model.SendModel()
                {
                    sendTitle = txtSendTitle,
                    sendEmail = txtSendEmail,
                    sendName = txtSendName,
                    sendContent = txtSendContent,
                    sendImage = txtImageUri,
                    Host = postModel.Host,
                    PostApi = postModel.Api,
                    sendId = postModel.Id,
                    islandCode = postModel.islandCode,
                    isMain = postModel.IsMain,
                    CookieValue = postModel.Cookie.CookieValue,
                    sendDateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")                                      
                };

                if (string.IsNullOrEmpty(send.sendContent) && string.IsNullOrEmpty(send.sendImage))
                {
                    throw new Exception("内容不能为空");
                }
                if (string.IsNullOrEmpty(send.sendContent) && !string.IsNullOrEmpty(send.sendImage))
                    send.sendContent = "[分享图片]";

                IsHitTestVisible = false;
                OnSendClick(sender, e);
                var res = await Data.Http.PostData(send);
                bool IsSuccess = false;
                string ThreadId = "";
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                        if (res.body.IndexOf("回复成功") >= 0 || res.body.IndexOf("发帖成功") >= 0)
                            IsSuccess = true;
                        break;
                    case IslandsCode.Koukuko:
                        JObject jobj = (JObject)JsonConvert.DeserializeObject(res.body);
                        if (jobj["success"].ToString().ToLower() == "true")
                        {
                            IsSuccess = true;
                            ThreadId = jobj["threadsId"].ToString();
                        }
                        break;
                }
                if (IsSuccess)
                {
                    EmptyButton_Click(null,null);
                }
                send.ThreadId = ThreadId;
                OnResponse(IsSuccess, send);
            }
            catch (Exception ex) {
                this.IsHitTestVisible = true;
                Data.Message.ShowMessage(ex.Message);
            }
        }

        private void EmojiBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmojiBox.SelectedIndex != 0)
            {
                var emoji = EmojiBox.SelectedItem.ToString();
                var start = SendContent.Document.Selection.StartPosition;
                SendContent.Document.Selection.SetText(TextSetOptions.None, emoji);                
                SendContent.Document.Selection.StartPosition = start + emoji.Length;
                EmojiBox.SelectedIndex = 0;
            }
        }

        private void RefButton_Click(object sender, RoutedEventArgs e)
        {
            SendContent.Document.Selection.SetText(TextSetOptions.None, ">>No.");
        }

        private void SendContent_GotFocus(object sender, RoutedEventArgs e)
        {
            //Bottom.Height = this.ActualHeight / 2;
            
        }

        private void SendContent_LostFocus(object sender, RoutedEventArgs e)
        {
            //Bottom.ClearValue(HeightProperty);
        }
    }
}
