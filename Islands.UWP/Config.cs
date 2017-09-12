using Islands.UWP.Model;
using System.Collections.Generic;
using System.IO;
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
        public const string AHost = "AHost";
        public const string APictureHost = "APictureHost";
        public const string BHost = "BHost";
        public const string BPictureHost = "BPictureHost";
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
        public static string SendImageFolder = Path.Combine(SavedImageFolder, "SendImage");
        public static double MaxImageWidth = 200;
        public static double MaxImageHeight = 200;
        public static string PoString = " (POPOPO)";
        public static string AdminString = " (猴猴猴)";
        #region 颜文字
        public static List<string> Kaomoji = new List<string>() {
            "颜文字",
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
            #region 错误提示
            "为什么会变成这样呢(´・ω・`)",
            "ギリギリeye ( ﾟ∀。)",
            "my little pony ( ﾟ∀。)",
            "出错也好，崩溃也好，明明是我先的ﾟ(つд`ﾟ)",
            "仙客来根雕",
            "Excuse 咪(´・ω・`)？",
            "ルン☆ピカ☆ビーム!",
            "猴子*眼小",
            #endregion
        };

        //key与IslandsCode一致
        public static Dictionary<string, IslandConfigModel> Island = new Dictionary<string, IslandConfigModel>()
        {
            { "A" , new IslandConfigModel() {
                    Host = "https://h.nimingban.com",
                    PictureHost = "http://img6.nimingban.com/",
                    GetThreadAPI = "{0}/Api/showf/id/{1}/page/{2}",
                    GetReplyAPI = "{0}/Api/thread/id/{1}/page/{2}",
                    GetRefAPI = "{0}/Api/ref?id={1}",
                    PostThreadAPI = "{0}/Home/Forum/doPostThread.html",
                    PostReplyAPI = "{0}/Home/Forum/doReplyThread.html",
                    PageSize = 19,
                    IslandCode = IslandsCode.A,
                    #region 板块
                    Groups = new List<Group<ForumModel>>() {
                        new Group<ForumModel>() {
                            GroupName = "综合",
                            Models = new List<ForumModel>() {
                                new ForumModel() {  forumName = "综合版", forumValue = "4", forumGroupId = "1"},
                                new ForumModel() {  forumName = "欢乐恶搞", forumValue = "20", forumGroupId = "1"},
                                new ForumModel() {  forumName = "推理", forumValue = "11", forumGroupId = "1"},
                                new ForumModel() {  forumName = "技术讨论", forumValue = "30", forumGroupId = "1"},
                                new ForumModel() {  forumName = "美食", forumValue = "32", forumGroupId = "1"},
                                new ForumModel() {  forumName = "喵版", forumValue = "40", forumGroupId = "1"},
                                new ForumModel() {  forumName = "音乐", forumValue = "35", forumGroupId = "1"},
                                new ForumModel() {  forumName = "校园", forumValue = "56", forumGroupId = "1"},
                                new ForumModel() {  forumName = "社畜", forumValue = "110", forumGroupId = "1"},
                                new ForumModel() {  forumName = "科学", forumValue = "15", forumGroupId = "1"},
                                new ForumModel() {  forumName = "活动", forumValue = "104", forumGroupId = "1"},
                                new ForumModel() {  forumName = "文学", forumValue = "103", forumGroupId = "1"},
                                new ForumModel() {  forumName = "二次创作", forumValue = "17", forumGroupId = "1"},
                                new ForumModel() {  forumName = "姐妹", forumValue = "98", forumGroupId = "1"},
                                new ForumModel() {  forumName = "女性向", forumValue = "102", forumGroupId = "1"},
                                new ForumModel() {  forumName = "女装", forumValue = "97", forumGroupId = "1"},
                                new ForumModel() {  forumName = "日记", forumValue = "89", forumGroupId = "1"},
                                new ForumModel() {  forumName = "WIKI", forumValue = "27", forumGroupId = "1"},
                                new ForumModel() {  forumName = "都市怪谈", forumValue = "81", forumGroupId = "1"},
                                new ForumModel() {  forumName = "买买买", forumValue = "106", forumGroupId = "1"},
                                new ForumModel() {  forumName = "活动", forumValue = "104", forumGroupId = "1"},
                                new ForumModel() {  forumName = "圈内", forumValue = "96", forumGroupId = "1"},
                                new ForumModel() {  forumName = "速报", forumValue = "83", forumGroupId = "1"},
                            }
                        },
                        new Group<ForumModel>() {
                            GroupName = "二次元",
                            Models = new List<ForumModel>() {
                                new ForumModel() {  forumName = "动画", forumValue = "14", forumGroupId = "1"},
                                new ForumModel() {  forumName = "漫画", forumValue = "12", forumGroupId = "1"},
                                new ForumModel() {  forumName = "国漫", forumValue = "99", forumGroupId = "1"},
                                new ForumModel() {  forumName = "美漫", forumValue = "90", forumGroupId = "1"},
                                new ForumModel() {  forumName = "轻小说", forumValue = "87", forumGroupId = "1"},
                                new ForumModel() {  forumName = "小说", forumValue = "19", forumGroupId = "1"},
                                new ForumModel() {  forumName = "GALGAME", forumValue = "64", forumGroupId = "1"},
                                new ForumModel() {  forumName = "VOCALOID", forumValue = "6", forumGroupId = "1"},
                                new ForumModel() {  forumName = "东方", forumValue = "5", forumGroupId = "1"},
                                new ForumModel() {  forumName = "舰娘", forumValue = "93", forumGroupId = "1"},
                                new ForumModel() {  forumName = "LL", forumValue = "101", forumGroupId = "1"},
                            }
                        },
                        new Group<ForumModel>() {
                            GroupName = "游戏",
                            Models = new List<ForumModel>() {
                                new ForumModel() {  forumName = "游戏综合版", forumValue = "2", forumGroupId = "1"},
                                new ForumModel() {  forumName = "守望先锋", forumValue = "109", forumGroupId = "1"},
                                new ForumModel() {  forumName = "手游", forumValue = "3", forumGroupId = "1"},
                                new ForumModel() {  forumName = "EVE", forumValue = "73", forumGroupId = "1"},
                                new ForumModel() {  forumName = "DNF", forumValue = "72", forumGroupId = "1"},
                                new ForumModel() {  forumName = "战争雷霆", forumValue = "86", forumGroupId = "1"},
                                new ForumModel() {  forumName = "LOL", forumValue = "22", forumGroupId = "1"},
                                new ForumModel() {  forumName = "DOTA", forumValue = "70", forumGroupId = "1"},
                                new ForumModel() {  forumName = "Steam", forumValue = "107", forumGroupId = "1"},
                                new ForumModel() {  forumName = "辐射4", forumValue = "108", forumGroupId = "1"},
                                new ForumModel() {  forumName = "GTA5", forumValue = "95", forumGroupId = "1"},
                                new ForumModel() {  forumName = "MC", forumValue = "10", forumGroupId = "1"},
                                new ForumModel() {  forumName = "音游", forumValue = "34", forumGroupId = "1"},
                                new ForumModel() {  forumName = "WOT", forumValue = "51", forumGroupId = "1"},
                                new ForumModel() {  forumName = "WOW", forumValue = "44", forumGroupId = "1"},
                                new ForumModel() {  forumName = "D3", forumValue = "23", forumGroupId = "1"},
                                new ForumModel() {  forumName = "卡牌桌游", forumValue = "45", forumGroupId = "1"},
                                new ForumModel() {  forumName = "炉石传说", forumValue = "80", forumGroupId = "1"},
                                new ForumModel() {  forumName = "怪物猎人", forumValue = "28", forumGroupId = "1"},
                                new ForumModel() {  forumName = "口袋妖怪", forumValue = "38", forumGroupId = "1"},
                                new ForumModel() {  forumName = "AC大逃杀", forumValue = "29", forumGroupId = "1"},
                                new ForumModel() {  forumName = "索尼", forumValue = "24", forumGroupId = "1"},
                                new ForumModel() {  forumName = "任天堂", forumValue = "25", forumGroupId = "1"},
                                new ForumModel() {  forumName = "日麻", forumValue = "92", forumGroupId = "1"},
                            }
                        },
                        new Group<ForumModel>() {
                            GroupName = "2.5次元,",
                            Models = new List<ForumModel>() {
                                new ForumModel() {  forumName = "AKB48", forumValue = "16", forumGroupId = "1"},
                                new ForumModel() {  forumName = "SNH48", forumValue = "100", forumGroupId = "1"},
                                new ForumModel() {  forumName = "眼科", forumValue = "13", forumGroupId = "1"},
                                new ForumModel() {  forumName = "声优", forumValue = "55", forumGroupId = "1"},
                                new ForumModel() {  forumName = "模型", forumValue = "39", forumGroupId = "1"},
                            }
                        },
                        new Group<ForumModel>() {
                            GroupName = "三次元",
                            Models = new List<ForumModel>() {
                                new ForumModel() {  forumName = "影视", forumValue = "31", forumGroupId = "1"},
                                new ForumModel() {  forumName = "摄影", forumValue = "54", forumGroupId = "1"},
                                new ForumModel() {  forumName = "体育", forumValue = "33", forumGroupId = "1"},
                                new ForumModel() {  forumName = "军武", forumValue = "37", forumGroupId = "1"},
                                new ForumModel() {  forumName = "数码", forumValue = "75", forumGroupId = "1"},
                                new ForumModel() {  forumName = "天台", forumValue = "88", forumGroupId = "1"},
                            }
                        },
                        new Group<ForumModel>() {
                            GroupName = "其他",
                            Models = new List<ForumModel>() {
                                new ForumModel() {  forumName = "询问2", forumValue = "36", forumGroupId = "1"},
                            }
                        },
                    }
                    #endregion
            }
            },
            { "Koukuko" , new IslandConfigModel() {
                    Host = "http://h.koukuko.com",
                    PictureHost = "http://static.koukuko.com/h",
                    GetThreadAPI = "{0}/api/{1}?page={2}",
                    GetReplyAPI = "{0}/api/t/{1}?page={2}",
                    GetRefAPI = "{0}/api/homepage/ref?tid={1}",
                    PostThreadAPI = "{0}/api/{1}/create",
                    PostReplyAPI = "{0}/api/t/{1}/create",
                    PageSize = 20,
                    IslandCode = IslandsCode.Koukuko,
                    #region 板块
                    Groups = new List<Group<ForumModel>>()
                    {
                        new Group<ForumModel>() {
                            GroupName = "综合",
                            Models = new List<ForumModel>() {
                                new ForumModel() {  forumName = "综合版1", forumValue = "综合版1", forumGroupId = "4"},
                                new ForumModel() {  forumName = "欢乐恶搞", forumValue = "欢乐恶搞", forumGroupId = "20"},
                                new ForumModel() {  forumName = "推理", forumValue = "推理", forumGroupId = "11"},
                                new ForumModel() {  forumName = "技术讨论", forumValue = "技术宅", forumGroupId = "30"},
                                new ForumModel() {  forumName = "美食", forumValue = "料理", forumGroupId = "32"},
                                new ForumModel() {  forumName = "喵版", forumValue = "貓版", forumGroupId = "40"},
                                new ForumModel() {  forumName = "音乐", forumValue = "音乐", forumGroupId = "35"},
                                new ForumModel() {  forumName = "体育", forumValue = "体育", forumGroupId = "33"},
                                new ForumModel() {  forumName = "军武", forumValue = "军武", forumGroupId = "37"},
                                new ForumModel() {  forumName = "模型", forumValue = "模型", forumGroupId = "39"},
                                new ForumModel() {  forumName = "考试", forumValue = "考试", forumGroupId = "56"},
                                new ForumModel() {  forumName = "数码", forumValue = "数码", forumGroupId = "75"},
                                new ForumModel() {  forumName = "日记", forumValue = "日记", forumGroupId = "89"},
                                new ForumModel() {  forumName = "速报", forumValue = "速报", forumGroupId = "83"},
                                new ForumModel() {  forumName = "都市怪谈", forumValue = "都市怪谈", forumGroupId = "81"},
                            }
                        },
                        new Group<ForumModel>() {
                            GroupName = "二次元",
                            Models = new List<ForumModel>() {
                                new ForumModel() {  forumName = "动画", forumValue = "动画", forumGroupId = "14"},
                                new ForumModel() {  forumName = "漫画", forumValue = "漫画", forumGroupId = "12"},
                                new ForumModel() {  forumName = "美漫", forumValue = "美漫", forumGroupId = "90"},
                                new ForumModel() {  forumName = "轻小说", forumValue = "轻小说", forumGroupId = "87"},
                                new ForumModel() {  forumName = "小说", forumValue = "小说", forumGroupId = "19"},
                                new ForumModel() {  forumName = "二次创作", forumValue = "二次创作", forumGroupId = "17"},
                                new ForumModel() {  forumName = "VOCALOID", forumValue = "VOCALOID", forumGroupId = "6"},
                                new ForumModel() {  forumName = "东方", forumValue = "东方Project", forumGroupId = "5"},
                                new ForumModel() {  forumName = "辣鸡", forumValue = "辣鸡", forumGroupId = "95"},
                            }
                        },
                        new Group<ForumModel>() {
                            GroupName = "游戏",
                            Models = new List<ForumModel>() {
                                new ForumModel() {  forumName = "游戏综合版", forumValue = "游戏", forumGroupId = "2"},
                                new ForumModel() {  forumName = "EVE", forumValue = "EVE", forumGroupId = "73"},
                                new ForumModel() {  forumName = "DNF", forumValue = "DNF", forumGroupId = "72"},
                                new ForumModel() {  forumName = "战争雷霆", forumValue = "战争雷霆", forumGroupId = "86"},
                                new ForumModel() {  forumName = "百万亚瑟王", forumValue = "扩散性百万亚瑟王", forumGroupId = "63"},
                                new ForumModel() {  forumName = "LOL", forumValue = "LOL", forumGroupId = "22"},
                                new ForumModel() {  forumName = "DOTA", forumValue = "DOTA", forumGroupId = "70"},
                                new ForumModel() {  forumName = "MC", forumValue = "Minecraft", forumGroupId = "10"},
                                new ForumModel() {  forumName = "音游", forumValue = "MUG", forumGroupId = "34"},
                                new ForumModel() {  forumName = "MUGEN", forumValue = "MUGEN", forumGroupId = "48"},
                                new ForumModel() {  forumName = "WOT", forumValue = "WOT", forumGroupId = "51"},
                                new ForumModel() {  forumName = "WOW", forumValue = "WOW", forumGroupId = "44"},
                                new ForumModel() {  forumName = "D3", forumValue = "D3", forumGroupId = "23"},
                                new ForumModel() {  forumName = "卡牌桌游", forumValue = "卡牌桌游", forumGroupId = "45"},
                                new ForumModel() {  forumName = "炉石传说", forumValue = "炉石传说", forumGroupId = "80"},
                                new ForumModel() {  forumName = "怪物猎人", forumValue = "怪物猎人", forumGroupId = "28"},
                                new ForumModel() {  forumName = "口袋妖怪", forumValue = "口袋妖怪", forumGroupId = "38"},
                                new ForumModel() {  forumName = "索尼", forumValue = "索尼", forumGroupId = "24"},
                                new ForumModel() {  forumName = "任天堂", forumValue = "任天堂", forumGroupId = "25"},
                                new ForumModel() {  forumName = "日麻", forumValue = "日麻", forumGroupId = "92"},
                                new ForumModel() {  forumName = "舰娘", forumValue = "舰娘", forumGroupId = "93"},
                                new ForumModel() {  forumName = "LL", forumValue = "LoveLive", forumGroupId = "97"},
                                new ForumModel() {  forumName = "辐射", forumValue = "辐射", forumGroupId = "96"},
                            }
                        },
                        new Group<ForumModel>() {
                            GroupName = "三次元",
                            Models = new List<ForumModel>() {
                                new ForumModel() {  forumName = "AKB48", forumValue = "AKB", forumGroupId = "16"},
                                new ForumModel() {  forumName = "眼科", forumValue = "COSPLAY", forumGroupId = "13"},
                                new ForumModel() {  forumName = "影视", forumValue = "影视", forumGroupId = "31"},
                                new ForumModel() {  forumName = "摄影", forumValue = "摄影", forumGroupId = "54"},
                                new ForumModel() {  forumName = "声优", forumValue = "声优", forumGroupId = "55"},
                            }
                        },
                        new Group<ForumModel>() {
                            GroupName = "其他",
                            Models = new List<ForumModel>() {
                                new ForumModel() {  forumName = "询问2", forumValue = "询问2", forumGroupId = "36"},
                            }
                        },
                    }
                    #endregion
            }
            },
            { "Beitai" , new IslandConfigModel() {
                    Host = "https://tnmb.org",
                    PictureHost = "https://tnmbstatic.fastmirror.org/Public/Upload/",
                    GetThreadAPI = "{0}/Api/showf/id/{1}/page/{2}",
                    GetReplyAPI = "{0}/Api/thread/id/{1}/page/{2}",
                    GetRefAPI = "{0}/Api/ref?id={1}",
                    PostThreadAPI = "{0}/home/forum/dopostthread.html",
                    PostReplyAPI = "{0}/home/forum/doreplythread.html",
                    PageSize = 19,
                    IslandCode = IslandsCode.Beitai,
                    #region 板块
                    Groups = new List<Group<ForumModel>>()
                    {
                        new Group<ForumModel>() {
                            GroupName = "板块",
                            Models = new List<ForumModel>() {
                                new ForumModel() {  forumName = "综合", forumValue = "1", forumGroupId = "1"},
                                new ForumModel() {  forumName = "技术", forumValue = "2", forumGroupId = "1"},
                                new ForumModel() {  forumName = "二次创作", forumValue = "3", forumGroupId = "1"},
                                new ForumModel() {  forumName = "动画漫画", forumValue = "4", forumGroupId = "1"},
                                new ForumModel() {  forumName = "值班室", forumValue = "5", forumGroupId = "1"},
                                new ForumModel() {  forumName = "游戏", forumValue = "6", forumGroupId = "1"},
                                new ForumModel() {  forumName = "欢乐恶搞", forumValue = "7", forumGroupId = "1"},
                                new ForumModel() {  forumName = "小说", forumValue = "11", forumGroupId = "1"},
                                new ForumModel() {  forumName = "数码音乐", forumValue = "13", forumGroupId = "1"},
                                new ForumModel() {  forumName = "射影", forumValue = "14", forumGroupId = "1"},
                                new ForumModel() {  forumName = "都市怪谈", forumValue = "15", forumGroupId = "1"},
                                new ForumModel() {  forumName = "支援1", forumValue = "17", forumGroupId = "1"},
                                new ForumModel() {  forumName = "基佬", forumValue = "18", forumGroupId = "1"},
                                new ForumModel() {  forumName = "姐妹2", forumValue = "19", forumGroupId = "1"},
                                new ForumModel() {  forumName = "日记", forumValue = "20", forumGroupId = "1"},
                                new ForumModel() {  forumName = "美食", forumValue = "21", forumGroupId = "1"},
                                new ForumModel() {  forumName = "喵版", forumValue = "22", forumGroupId = "1"},
                                new ForumModel() {  forumName = "社畜", forumValue = "23", forumGroupId = "1"},
                                new ForumModel() {  forumName = "车万养老院", forumValue = "24", forumGroupId = "1"},
                            }
                        }
                    }
                    #endregion
            }
            },
        };

        public static List<string> DatabaseTypeList = new List<string>() {
            "本地",
            "漫游"
        };
    }

    public class IslandConfigModel
    {
        public string Host { get; set; }
        public string PictureHost { get; set; }
        public string GetThreadAPI { get; set; }
        public string GetReplyAPI { get; set; }
        public string GetRefAPI { get; set; }
        public string PostThreadAPI { get; set; }
        public string PostReplyAPI { get; set; }
        public int PageSize { get; set; }
        public IslandsCode IslandCode { get; set; }
        public List<Group<ForumModel>> Groups { get; set; }
    }
}
