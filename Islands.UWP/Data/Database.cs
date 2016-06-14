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
        public static string ExecSql(string sql)
        {
            string result = string.Empty;
            using (var conn = GetDbConnection())
            {
                var l = conn.ExecuteScalar<Model.SettingModel>(sql);
                result = "";
            }
            return result;
        }

        public static int Insert<T>(T model)
        {
            int result = 0;
            using (var conn = GetDbConnection<T>())
            {
                if (conn == null) throw new Exception(Config.ConnectDatabaseError);
                result = conn.Insert(model);
                return result;
            }
        }

        public static int Update<T>(T model)
        {
            int result = 0;
            using (var conn = GetDbConnection<T>())
            {
                if (conn == null) throw new Exception(Config.ConnectDatabaseError);
                result = conn.Update(model);
                return result;
            }
        }

        public static bool Delete<T>(T model)
        {
            using (var conn = GetDbConnection<T>())
            {
                if (conn == null) return false;
                conn.Delete(model);
            }
            return true;
        }

        public static List<Model.SendModel> GetMyReplyList(IslandsCode islandCode)
        {
            using (var conn = GetDbConnection<Model.SendModel>())
            {
                if (conn != null)
                    return (from model in conn.Table<Model.SendModel>()
                            where model.islandCode == islandCode
                            orderby model._id descending
                            select model).ToList();
                return new List<Model.SendModel>();
            }
        }

        public static List<Model.ThreadModel> GetMarkList(IslandsCode islandCode)
        {
            using (var conn = GetDbConnection<Model.ThreadModel>())
            {
                if (conn != null)
                    return (from model in conn.Table<Model.ThreadModel>()
                            where model.islandCode == islandCode
                            orderby model._id descending
                            select model).ToList();

                return new List<Model.ThreadModel>();
            }
        }

        public static List<Model.SettingModel> GetSettingList(IslandsCode islandCode)
        {           
            using (var conn = GetDbConnection<Model.SettingModel>())
            {
                if (conn != null)
                    return (from model in conn.Table<Model.SettingModel>()
                            where model.islandCode == islandCode
                            orderby model._id descending
                            group model by model.SettingName into g
                            select g.ElementAtOrDefault(0)).ToList();

                return new List<Model.SettingModel>();
            }
        }

        private readonly static string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, Config.dbName);

        private static SQLiteConnection GetDbConnection<T>()
        {
            try
            {
                var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
                conn.CreateTable<T>();
                return conn;
            }
            catch (Exception ex)
            {
                //Message.ShowMessage(ex.Message);
                return null;
            }
        }

        private static SQLiteConnection GetDbConnection()
        {
            try
            {
                var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
                return conn;
            }
            catch (Exception ex)
            {
                //Message.ShowMessage(ex.Message);
                return null;
            }
        }
    }
}
