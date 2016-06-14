using SQLite.Net.Attributes;

namespace Islands.UWP.Model
{
    public class SettingModel
    {
        public IslandsCode islandCode { get; set; }

        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
    }
}
