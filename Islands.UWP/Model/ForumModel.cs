using System.Collections.Generic;

namespace Islands.UWP.Model
{
    public class Group<T>
    {
        public string GroupName { get; set; }
        public List<T> Models { get; set; }
    }

    public class ForumModel
    {
        public string forumName { get; set; }
        public string forumValue { get; set; }
        public string forumGroupId { get; set; }
        public bool forumVisiable { get; set; }
        public int forumIndex { get; set; }
        public ForumModel() { }

    }
}
