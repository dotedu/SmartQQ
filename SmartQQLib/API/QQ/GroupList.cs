using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQLib.API
{
    
    class GroupList_Qun
    {
        public IList<GroupList_Qun_Create> create { get; set; }
        public int ec { get; set; }
        public IList<GroupList_Qun_Join> join { get; set; }
        public IList<GroupList_Qun_Manage> manage { get; set; }

    }
    public class GroupList_Qun_Create
    {
        public int gc { get; set; }
        public string gn { get; set; }
        public int owner { get; set; }
    }
    public class GroupList_Qun_Join
    {
        public int gc { get; set; }
        public string gn { get; set; }
        public object owner { get; set; }
    }
    public class GroupList_Qun_Manage
    {
        public int gc { get; set; }
        public string gn { get; set; }
        public int owner { get; set; }
    }
}
