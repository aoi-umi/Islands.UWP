using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Islands.UWP.Model
{
    public class ReplyModel
    {
        public string id { get; set; }
        public string img { get; set; }
        public string ext { get; set; }
        public string now { get; set; }
        public string userid { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string sage { get; set; }
        public string admin { get; set; }


        public IslandsCode islandCode;
        //for view
        public Visibility IsHadTitle
        {
            get
            {
                if (Title != "标题:" && Title != "标题:无标题")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility IsHadEmail
        {
            get
            {
                if (Email != "email:")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility IsHadName
        {
            get
            {
                if (Name != "名字:" && Name != "名字:无名氏")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public string Title
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A: return "标题:" + title;
                    default: return "";
                }
            }
        }
        public string Email
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A: return "email:" + email;
                    default: return "";
                }
            }
        }
        public string Name
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A: return "名字:" + name;
                    default: return "";
                }
            }
        }
        public string No
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A: return id;
                    default: return "";
                }
            }
        }

        public string CreateDate
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A: return now;
                    default: return "";
                }
            }
        }
        public string ID
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A: return userid;
                    default: return "";
                }
            }
        }
        public string Image
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                        if (string.IsNullOrEmpty(img))
                            return "";
                        return (Config.A.PictureHost + "thumb/" + img + ext);
                    default: return "";
                }
            }
        }
        public string Content
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A: return content;
                    default: return "";
                }
            }
        }

        public ReplyModel() { }
    }
}
