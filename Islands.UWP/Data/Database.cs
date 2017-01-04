using Islands.UWP.Model;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;


namespace Islands.UWP.Data
{
    public static class Database
    {
        public static int Insert<T>(T model)
        {
            int result = 0;
            using (var conn = GetDbConnection<T>(DbPath))
            {
                if (conn == null) throw new Exception(Config.ConnectDatabaseError);
                result = conn.Insert(model);
                return result;
            }
        }

        public static async Task<int> InsertAsync<T>(T model)
        {
            int result = 0;
            await Task.Run(() => {
                using (var conn = GetDbConnection<T>(DbPath))
                {
                    if (conn == null) throw new Exception(Config.ConnectDatabaseError);
                    result = conn.Insert(model);
                }
            });
            return result;
        }

        public static int Update<T>(T model)
        {
            int result = 0;
            using (var conn = GetDbConnection<T>(DbPath))
            {
                if (conn == null) throw new Exception(Config.ConnectDatabaseError);
                result = conn.Update(model);
                return result;
            }
        }

        public static async Task<int> UpdateAsync<T>(T model)
        {
            int result = 0;
            await Task.Run(() => {
                using (var conn = GetDbConnection<T>(DbPath))
                {
                    if (conn == null) throw new Exception(Config.ConnectDatabaseError);
                    result = conn.Update(model);
                }
            });
            return result;
        }

        public static bool Delete<T>(T model)
        {
            using (var conn = GetDbConnection<T>(DbPath))
            {
                if (conn == null) return false;
                conn.Delete(model);
            }
            return true;
        }

        public static int DeleteByIDs(string table, List<int> idList)
        {
            if (idList == null || idList.Count == 0) return 0;
            string sql = string.Format("delete from {0} where _id in ({1})", table, string.Join(",", idList));
            return Execute(sql);
        }

        public static List<SendModel> GetMyReplyList(IslandsCode islandCode)
        {
            using (var conn = GetDbConnection<SendModel>(DbPath))
            {
                if (conn != null)
                {
                    return (from model in conn.Table<SendModel>()
                            where islandCode == IslandsCode.All || model.islandCode == islandCode
                            orderby model.sendDateTime descending
                            select model).ToList();
                }
                return new List<SendModel>();
            }
        }

        public static List<ThreadModel> GetMarkList(IslandsCode islandCode, string id = null)
        {
            using (var conn = GetDbConnection<ThreadModel>(DbPath))
            {
                if (conn != null)
                {
                    if (string.IsNullOrEmpty(id))
                        return (from model in conn.Table<ThreadModel>()
                                where (islandCode == IslandsCode.All || model.islandCode == islandCode)
                                orderby model._id descending
                                select model).ToList();
                    return (from model in conn.Table<ThreadModel>()
                            where (islandCode == IslandsCode.All || model.islandCode == islandCode) && model.id == id
                            orderby model._id descending
                            select model).ToList();
                }
                return new List<ThreadModel>();
            }
        }

        public static List<SettingModel> GetSettingList(IslandsCode islandCode)
        {           
            using (var conn = GetDbConnection<SettingModel>(DbPath))
            {
                if (conn != null)
                    return (from model in conn.Table<SettingModel>()
                            where model.islandCode == islandCode
                            orderby model._id descending
                            group model by model.SettingName into g
                            select g.ElementAtOrDefault(0)).ToList();

                return new List<SettingModel>();
            }
        }

        public static int RoamingInsert<T>(T model)
        {
            int result = 0;
            using (var conn = GetDbConnection<T>(RoamingDbPath))
            {
                if (conn == null) throw new Exception(Config.ConnectDatabaseError);
                result = conn.Insert(model);
            }
            return result;
        }

        public static List<SendModel> RoamingGetMyReplyList()
        {
            using (var conn = GetDbConnection<SendModel>(RoamingDbPath))
            {
                if (conn != null)
                    return (from model in conn.Table<SendModel>()
                            select model).ToList();
                return new List<SendModel>();
            }
        }

        public static List<ThreadModel> RoamingGetMarkList()
        {
            using (var conn = GetDbConnection<ThreadModel>(RoamingDbPath))
            {
                if (conn != null)
                    return (from model in conn.Table<ThreadModel>()
                            select model).ToList();
                return new List<ThreadModel>();
            }
        }

        //System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();
        private readonly static string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, Config.dbName);
        private readonly static string RoamingDbPath = Path.Combine(ApplicationData.Current.RoamingFolder.Path, Config.dbName);

        private static SQLiteConnection GetDbConnection<T>(string dbPath)
        {
            try
            {
                var conn = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath);
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

        private static int Execute(string sql)
        {
            int count = 0;
            using (var conn = GetDbConnection())
            {
                if (conn == null) return 0;
                count = conn.Execute(sql);
            }
            return count;
        }
    }
}
