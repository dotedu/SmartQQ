using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQLib.API
{
    class FriendList
    {
        public int retcode { get; set; }
        public FriendList_Result result { get; set; }
    }

    public class Friend
    {
        public int flag { get; set; }
        public object uin { get; set; }
        public int categories { get; set; }
    }
    public class Markname
    {
        public object uin { get; set; }
        public string markname { get; set; }
        public int type { get; set; }
    }
    public class Category
    {
        public int index { get; set; }
        public int sort { get; set; }
        public string name { get; set; }
    }
    public class Vipinfo
    {
        public int vip_level { get; set; }
        public object u { get; set; }
        public int is_vip { get; set; }
    }
    public class Info
    {
        public int face { get; set; }
        public int flag { get; set; }
        public string nick { get; set; }
        public object uin { get; set; }
    }
    public class FriendList_Result
    {
        public IList<Friend> friends { get; set; }
        public IList<Markname> marknames { get; set; }
        public IList<Category> categories { get; set; }
        public IList<Vipinfo> vipinfo { get; set; }
        public IList<Info> info { get; set; }
    }

}