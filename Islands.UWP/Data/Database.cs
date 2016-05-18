using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;


namespace Islands.UWP.Data
{
    public static class Database
    {
        public readonly static string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, Config.dbName);
        public static SQLiteConnection GetDbConnection<T>()
        {
            try
            {
                var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
                conn.CreateTable<T>();
                return conn;
            }
            catch (Exception ex) {
                Message.ShowMessage(ex.Message);
                return null;
            }
        }

        public static List<Model.SendModel> GetMyReplyList(IslandsCode islandCode)
        {
            using (var conn = Database.GetDbConnection<Model.SendModel>())
            {
                if (conn != null)
                    return (from send in conn.Table<Model.SendModel>()
                            where send.islandCode == islandCode
                            orderby send._id descending
                            select send).ToList();
                return new List<Model.SendModel>();
            }
        }

        public static List<Model.ThreadModel> GetMarkList(IslandsCode islandCode)
        {
            using (var conn = Database.GetDbConnection<Model.ThreadModel>())
            {
                if (conn != null)
                    return (from mark in conn.Table<Model.ThreadModel>()
                            where mark.islandCode == islandCode
                            orderby mark._id descending
                            select mark).ToList();

                return new List<Model.ThreadModel>();
            }
        }

        public static bool Delete<T>(T model)
        {
            using (var conn = Database.GetDbConnection<Model.ThreadModel>())
            {
                if (conn != null)
                {
                    conn.Delete(model);
                    return true;
                }
                return false;
            }
        }

    }
}
