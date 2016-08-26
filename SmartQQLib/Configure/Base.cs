using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQLib
{
    internal class Base
    {
        public static long clientid { get; set; } = 53999199;
        public static string status { get; set; } = "online";   //callme|online|away|busy|silent|hidden|offline,
        public static string psessionid { get; set; } = "";
        public static string vfwebqq { get; set; } = "";
        public static string ptwebqq { get; set; } = "";
        public static string skey { get; set; } = "";
        public static string passwd_sig { get; set; } = "";
        public static string verifycode { get; set; } = "";
        public static string pt_verifysession { get; set; } = "";
        public static string ptvfsession { get; set; } = "";
        public static string md5_salt { get; set; } = "";
        public static string cap_cd { get; set; } = "";
        public static int isRandSalt { get; set; } = 0;
        public static string api_check_sig { get; set; } = "";
        public static string g_login_sig { get; set; } = "";
        public static int g_style { get; set; } = 16;

        public static string g_mibao_css = "m_webqq";
        public static int g_daid { get; set; } = 164;
        public static int g_appid { get; set; } = 501004106;
        public static int g_pt_version = 10137;
        public static string rc { get; set; } = "1";
        public static string csrf_token { get; set; } = "";
    }
}
