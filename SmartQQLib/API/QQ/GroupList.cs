using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQLib.API.QQ
{
    class GroupList
    {
        public IList<Create> create { get; set; }
        public int ec { get; set; }
        public IList<Join> join { get; set; }
        public IList<Manage> manage { get; set; }

    }
    public class Create
    {
        public int gc { get; set; }
        public string gn { get; set; }
        public int owner { get; set; }
    }
    public class Join
    {
        public int gc { get; set; }
        public string gn { get; set; }
        public object owner { get; set; }
    }
    public class Manage
    {
        public int gc { get; set; }
        public string gn { get; set; }
        public int owner { get; set; }
    }
}
