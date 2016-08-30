using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQLib.API
{
    public class GroupMembers
    {
        public int adm_max { get; set; }
        public int adm_num { get; set; }
        public int count { get; set; }
        public int ec { get; set; }
        public string levelname { get; set; }
        public int max_count { get; set; }
        public IList<GroupMembers_Mem> mems { get; set; }
        public int search_count { get; set; }
        public int svr_time { get; set; }
        public int vecsize { get; set; }
    }
    public class GroupMembers_Lv
    {
        public int level { get; set; }
        public int point { get; set; }
    }
    public class GroupMembers_Mem
    {
        public string card { get; set; }
        public int flag { get; set; }
        public int g { get; set; }
        public int join_time { get; set; }
        public int last_speak_time { get; set; }
        public GroupMembers_Lv lv { get; set; }
        public string nick { get; set; }
        public int qage { get; set; }
        public int role { get; set; }
        public string tags { get; set; }
        public long uin { get; set; }
    }

}
