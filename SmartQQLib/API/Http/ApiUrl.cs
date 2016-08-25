using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQLib.API
{
    public class ApiUrl
    {
        public static string Get_QrCode { get; set; } = "https://ssl.ptlogin2.qq.com/ptqrshow?appid=501004106&e=0&l=M&s=5&d=72&v=4&t={0}";
        public static string[] Verify_QrCode { get; set; } = {
            "https://ssl.ptlogin2.qq.com/ptqrlogin?webqq_type=10&remember_uin=1&login2qq=1&aid=501004106 &u1=http%3A%2F%2Fw.qq.com%2Fproxy.html%3Flogin2qq%3D1%26webqq_type%3D10 &ptredirect=0&ptlang=2052&daid=164&from_ui=1&pttype=1&dumy=&fp=loginerroralert &action=0-0-{0}&mibao_css=m_webqq&t=1&g=1&js_type=0&js_ver=10143&login_sig=&pt_randsalt=0",
            "https://ui.ptlogin2.qq.com/cgi-bin/login?daid=164&target=self&style=16&mibao_css=m_webqq&appid=501004106&enable_qlogin=0&no_verifyimg=1 &s_url=http%3A%2F%2Fw.qq.com%2Fproxy.html&f_url=loginerroralert &strong_login=1&login_state=10&t=20131024001"
        };
        public static string[] Get_PTwebqq { get; set; } = { "", "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1" };

        public static string[] Get_VFwebqq { get; set; } = {
            "http://s.web2.qq.com/api/getvfwebqq?ptwebqq={0}&clientid=53999199&psessionid=&t={1}",
            "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1"
            };
        public static string[] Get_Uin_And_Psessionid { get; set; } = {
            "http://d1.web2.qq.com/channel/login2",
            "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2"
            };

        public static string[] get_user_info { get; set; } = {
            "http://s.web2.qq.com/api/get_self_info2",
            "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2"
            };




        public static string[] Get_Group_List { get; set; } = {
            "http://s.web2.qq.com/api/get_group_name_list_mask2",
            "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2"
            };
        public static string[] Poll_Message { get; set; } = {
            "http://d1.web2.qq.com/channel/poll2",
            "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2"
            };
        public static string[] Send_Message_To_Group { get; set; } = {
            "http://d1.web2.qq.com/channel/send_qun_msg2",
            "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2"
            };
        public static string[] Get_Friend_List { get; set; } = {
            "http://s.web2.qq.com/api/get_user_friends2",
            "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1"
            };
        public static string[] SEND_Message_TO_Friend { get; set; } = {
            "http://d1.web2.qq.com/channel/send_buddy_msg2",
            "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2"
            };
        public static string[] Get_Discuss_List { get; set; } = {
            "http://s.web2.qq.com/api/get_discus_list?clientid=53999199&psessionid={0}&vfwebqq={1}&t={2}",
            "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1"
            };
        public static string[] Send_Message_To_Discuss { get; set; } = {
            "http://d1.web2.qq.com/channel/send_discu_msg2",
            "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2"
            };
        public static string[] Get_Account_Info { get; set; } = {
            "http://s.web2.qq.com/api/get_self_info2?t=0.1",
            "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1"
            };
        public static string[] Get_Recent_List { get; set; } = {
            "http://d1.web2.qq.com/channel/get_recent_list2",
            "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2"
            };
        public static string[] Get_Friend_Status { get; set; } = {
            "http://d1.web2.qq.com/channel/get_online_buddies2?vfwebqq={0}&clientid=53999199&psessionid={1}&t={2}",
            "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2"
            };
        public static string[] Get_Group_Info { get; set; } = {
            "http://s.web2.qq.com/api/get_group_info_ext2?gcode={0}&vfwebqq={1}&t={2}",
            "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1"
            };
        public static string[] Get_Qq_By_Id { get; set; } = {
            "http://s.web2.qq.com/api/get_friend_uin2?tuin={0}&type=1&vfwebqq={1}&t={2}",
            "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1"
            };
        public static string[] Get_Discuss_Info { get; set; } = {
            "http://d1.web2.qq.com/channel/get_discu_info?did={0}&vfwebqq={1}&clientid=53999199&psessionid={2}&t={3}",
            "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2"
            };
        public static string[] Get_Friend_Info { get; set; } = {
            "http://s.web2.qq.com/api/get_friend_info2?tuin={0}&vfwebqq={1}&clientid=53999199&psessionid={3}&t={4}",
            "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1"
            };
    }
}
