﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Islands.UWP.Model
{
    public class ThreadModel
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
        public string remainReplys { get; set; }
        public string replyCount { get; set; }


        public IslandsCode islandCode;
        //for view
        public string Title
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A: return title;
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
                    case IslandsCode.A: return email;
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
                    case IslandsCode.A: return name;
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
        public string ReplyCount
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A: return replyCount;
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

        public ThreadModel() {}
    }
}
