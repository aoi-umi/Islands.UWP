using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.IO;
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
                Data.Message.ShowMessage(ex.Message);
                return null;
            }
        }

    }
}
