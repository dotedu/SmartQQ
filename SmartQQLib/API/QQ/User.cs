using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQLib.API
{
    public class User
    {
        public int retcode { get; set; } = 0;
        public Result result { get; set; }
    }


    public class Birthday
    {
        public int month { get; set; }
        public int year { get; set; }
        public int day { get; set; }
    }
    public class Result
    {
        public Birthday birthday { get; set; }
        public int face { get; set; }
        public string phone { get; set; }
        public string occupation { get; set; }
        public int allow { get; set; }
        public string college { get; set; }
        public int uin { get; set; }
        public int blood { get; set; }
        public int constel { get; set; }
        public string lnick { get; set; }
        public string vfwebqq { get; set; }
        public string homepage { get; set; }
        public int vip_info { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string personal { get; set; }
        public int shengxiao { get; set; }
        public string nick { get; set; }
        public string email { get; set; }
        public string province { get; set; }
        public int account { get; set; }
        public string gender { get; set; }
        public string mobile { get; set; }
    }
}
