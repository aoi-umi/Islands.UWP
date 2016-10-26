using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Islands.UWP
{
    public enum IslandsCode
    {
        A = 0,
        Koukuko = 1,
        Beitai = 2,
        All = 10
    };

    public static class Settings
    {
        public const string DataVersion = "DataVersion";
        public const string NightMode = "NightMode";
        public const string TitleFontSize = "TitleFontSize";
        public const string ContentFontSize = "ContentFontSize";
        public const string MaskOpacity = "MaskOpacity";
        public const string IsAskEachTime = "IsAskEachTime";
        public const string BackgroundImagePath = "BackgroundImagePath";
        public const string NoImage = "NoImage";
        public const string IsHideMenu = "IsHideMenu";
    }

    public static class Config
    {
        public static Brush PoColor = new SolidColorBrush(Colors.Blue);
        public static Brush AdminColor = new SolidColorBrush(Colors.Red);
        public static Brush ErrorColor = new SolidColorBrush(Colors.Gray);
        public static Brush SelectedColor = new SolidColorBrush(Colors.LightGray);
        public static string dbName = "Islands.db";
        public static string ConnectDatabaseError = "连接数据库失败";
        public static string FailedImageUri = "ms-appx:/Assets/luwei.jpg";
        public static string TextToImageUri = "TextToImage";
        public static string SavedImageFolder = "Islands";
        public static double MaxImageWidth = 200;
        public static double MaxImageHeight = 200;
        #region 颜文字
        public static List<string> Emoji = new List<string>() {
            "|∀ﾟ",
            "(´ﾟДﾟ`)",
            "(;´Д`)",
            "(｀･ω･)",
            "(=ﾟωﾟ)=",
            "| ω・´)",
            "|-` )",
            "|д` )",
            "|ー` )",
            "|∀` )",
            "(つд⊂)",
            "(ﾟДﾟ≡ﾟДﾟ)",
            "(＾o＾)ﾉ",
            "(|||ﾟДﾟ)",
            "( ﾟ∀ﾟ)",
            "( ´∀`)",
            "(*´∀`)",
            "(*ﾟ∇ﾟ)",
            "(*ﾟーﾟ)",
            "(　ﾟ 3ﾟ)",
            "( ´ー`)",
            "( ・_ゝ・)",
            "( ´_ゝ`)",
            "(*´д`)",
            "(・ー・)",
            "(・∀・)",
            "(ゝ∀･)",
            "(〃∀〃)",
            "(*ﾟ∀ﾟ*)",
            "( ﾟ∀。)",
            "( `д´)",
            "(`ε´ )",
            "(`ヮ´ )",
            "σ`∀´)",
            " ﾟ∀ﾟ)σ",
            "ﾟ ∀ﾟ)ノ",
            "(╬ﾟдﾟ)",
            "(|||ﾟдﾟ)",
            "( ﾟдﾟ)Σ",
            "( ﾟдﾟ)",
            "( ;ﾟдﾟ)",
            "( ;´д`)",
            "(　д ) ﾟ ﾟ",
            "( ☉д⊙)",
            "(((　ﾟдﾟ)))",
            "( ` ・´)",
            "( ´д`)",
            "( -д-)",
            "(>д<)",
            "･ﾟ( ﾉд`ﾟ)",
            "( TдT)",
            "(￣∇￣)",
            "(￣3￣)",
            "(￣ｰ￣)",
            "(￣ . ￣)",
            "(￣皿￣)",
            "(￣艸￣)",
            "(￣︿￣)",
            "(￣︶￣)",
            "ヾ(´ωﾟ｀)",
            "(*´ω`*)",
            "(・ω・)",
            "( ´・ω)",
            "(｀・ω)",
            "(´・ω・`)",
            "(`・ω・´)",
            "( `_っ´)",
            "( `ー´)",
            "( ´_っ`)",
            "( ´ρ`)",
            "( ﾟωﾟ)",
            "(oﾟωﾟo)",
            "(　^ω^)",
            "(｡◕∀◕｡)",
            @"/( ◕‿‿◕ )\",
            "ヾ(´ε`ヾ)",
            "(ノﾟ∀ﾟ)ノ",
            "(σﾟдﾟ)σ",
            "(σﾟ∀ﾟ)σ",
            "|дﾟ )",
            "┃電柱┃",
            "ﾟ(つд`ﾟ)",
            "ﾟÅﾟ )",
            "⊂彡☆))д`)",
            "⊂彡☆))д´)",
            "⊂彡☆))∀`)",
            "(´∀((☆ミつ"
        };
        #endregion

        public static List<string> ErrorMessage = new List<string>() {
            "为什么会变成这样呢(´・ω・`)",
            "ギリギリeye ( ﾟ∀。)",
            "my little pony ( ﾟ∀。)",
            "出错也好，崩溃也好，明明是我先的ﾟ(つд`ﾟ)",
            "仙客来根雕",
            "Excuse 咪(´・ω・`)？",
            "ルン☆ピカ☆ビーム!",
            "猴子*眼小",
        };
        public static class A
        {
            public static string Host = "https://h.nimingban.com";
            public static string PictureHost = "http://img1.nimingban.com/";
            public static string GetThreadAPI = "{0}/Api/showf/id/{1}/page/{2}";
            public static string GetReplyAPI = "{0}/Api/thread/id/{1}/page/{2}";
            public static string GetRefAPI = "{0}/Api/ref?id={1}";
            public static string PostThreadAPI = "{0}/Home/Forum/doPostThread.html";
            public static string PostReplyAPI = "{0}/Home/Forum/doReplyThread.html";
            public static int PageSize = 19;
            #region 板块
            public static List<string> Forums = new List<string>(){
                "综合,,group",
                "综合版,4,1",
                "欢乐恶搞,20,1",
                "推理,11,1",
                "技术讨论,30,1",
                "美食,32,1",
                "喵版,40,1",
                "音乐,35,1",
                "校园,56,1",
                "科学,15,1",
                "活动,104,1",
                "文学,103,1",
                "二次创作,17,1",
                "姐妹,98,1",
                "女性向,102,1",
                "女装,97,1",
                "日记,89,1",
                "WIKI,27,1",
                "都市怪谈,81,1",
                "买买买,106,1",
                "活动,104,1",
                
                "二次元,,group",
                "动画,14,1",
                "漫画,12,1",
                "国漫,99,1",
                "美漫,90,1",
                "轻小说,87,1",
                "小说,19,1",
                "GALGAME,64,1",
                "VOCALOID,6,1",
                "东方,5,1",
                "舰娘,93,1",
                "LL,101,1",
                
                "游戏,,group",
                "游戏综合版,2,1",
                "守望先锋,109,1",
                "手游,3,1",
                "EVE,73,1",
                "DNF,72,1",
                "战争雷霆,86,1",
                "LOL,22,1",
                "DOTA,70,1",
                "Steam,107,1",
                "辐射4,108,1",
                "GTA5,95,1",
                "MC,10,1",
                "音游,34,1",
                "WOT,51,1",
                "WOW,44,1",
                "D3,23,1",
                "卡牌桌游,45,1",
                "炉石传说,80,1",
                "怪物猎人,28,1",
                "口袋妖怪,38,1",
                "AC大逃杀,29,1",
                "索尼,24,1",
                "任天堂,25,1",
                "日麻,92,1",
                
                "2.5次元,,group",
                "AKB48,16,1",
                "SNH48,100,1",
                "眼科,13,1",
                "声优,55,1",
                "模型,39,1",
                
                "三次元,,group",
                "影视,31,1",
                "摄影,54,1",
                "体育,33,1",
                "军武,37,1",
                "数码,75,1",
                "天台,88,1",
                
                "其他,,group",
                "询问2,36,1"
            };
            #endregion
        }
        public static class K
        {
            public static string Host = "http://h.koukuko.com";
            public static string PictureHost = "http://static.koukuko.com/h";
            public static string GetThreadAPI = "{0}/api/{1}?page={2}";
            public static string GetReplyAPI = "{0}/api/t/{1}?page={2}";
            public static string GetRefAPI = "{0}/api/homepage/ref?tid={1}";
            public static string PostThreadAPI = "{0}/api/{1}/create";
            public static string PostReplyAPI = "{0}/api/t/{1}/create";
            public static int PageSize = 20;
            #region 板块
            public static List<string> Forums = new List<string>() {
                "综合,,group",
                "综合版1,综合版1,4",
                "欢乐恶搞,欢乐恶搞,20",
                "推理,推理,11",
                "技术讨论,技术宅,30",
                "美食,料理,32",
                "喵版,貓版,40",
                "音乐,音乐,35",
                "体育,体育,33",
                "军武,军武,37",
                "模型,模型,39",
                "考试,考试,56",
                "数码,数码,75",
                "日记,日记,89",
                "速报,速报,83",
                "都市怪谈,都市怪谈,81",

                "二次元,,group",
                "动画,动画,14",
                "漫画,漫画,12",
                "美漫,美漫,90",
                "轻小说,轻小说,87",
                "小说,小说,19",
                "二次创作,二次创作,17",
                "VOCALOID,VOCALOID,6",
                "东方,东方Project,5",
                "辣鸡,辣鸡,95",

                "游戏,,group",
                "游戏综合版,游戏,2",
                "EVE,EVE,73",
                "DNF,DNF,72",
                "战争雷霆,战争雷霆,86",
                "百万亚瑟王,扩散性百万亚瑟王,63",
                "LOL,LOL,22",
                "DOTA,DOTA,70",
                "MC,Minecraft,10",
                "音游,MUG,34",
                "MUGEN,MUGEN,48",
                "WOT,WOT,51",
                "WOW,WOW,44",
                "D3,D3,23",
                "卡牌桌游,卡牌桌游,45",
                "炉石传说,炉石传说,80",
                "怪物猎人,怪物猎人,28",
                "口袋妖怪,口袋妖怪,38",
                "索尼,索尼,24",
                "任天堂,任天堂,25",
                "日麻,日麻,92",
                "舰娘,舰娘,93",
                "LL,LoveLive,97",
                "辐射,辐射,96",

                "三次元,,group",
                "AKB48,AKB,16",
                "眼科,COSPLAY,13",
                "影视,影视,31",
                "摄影,摄影,54",
                "声优,声优,55",

                "其他,,group",
                "询问2,询问2,36"
            };
            #endregion
        }
        public static class B
        {
            public static string Host = "http://h.adnmb.com";
            public static string PictureHost = "http://h-adnmb-com.n1.yun.tf:8999/Public/Upload/";
            public static string GetThreadAPI = "{0}/Api/showf/id/{1}/page/{2}";
            public static string GetReplyAPI = "{0}/Api/thread/id/{1}/page/{2}";
            public static string GetRefAPI = "{0}/Api/ref?id={1}";
            public static string PostThreadAPI = "{0}/home/forum/dopostthread.html";
            public static string PostReplyAPI = "{0}/home/forum/doreplythread.html";
            public static int PageSize = 19;
            #region 板块
            public static List<string> Forums = new List<string>() {
                "板块,,group",
                "综合,1,1",
                "技术,2,1",
                "二次创作,3,1",
                "动画漫画,4,1",
                "值班室,5,1",
                "游戏,6,1",
                "欢乐恶搞,7,1",
                "小说,11,1",
                "数码音乐,13,1",
                "射影,14,1",
                "都市怪谈,15,1",
                "支援1,17,1"
            };
            #endregion
        }
    }
}
