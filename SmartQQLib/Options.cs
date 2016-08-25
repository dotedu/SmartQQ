using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQLib
{
    public class Options
    {

        public int is_security { get; set; } = 0;                            //https|http 

        public int is_init_friend { get; set; } = 1;                            //是否在首次登录时初始化好友信息 bool b= X==0? false:true; 
        public int is_init_group { get; set; } = 1;                            //是否在首次登录时初始化群组信息
        public int is_init_discuss { get; set; } = 1;                            //是否在首次登录时初始化讨论组信息
        public int is_init_recent { get; set; } = 0;                            //是否在首次登录时初始化最近联系人信息

        public int is_update_user { get; set; } = 0;                            //是否定期更新个人信息
        public int is_update_group { get; set; } = 1;                            //是否定期更新群组信息
        public int is_update_friend { get; set; } = 1;                            //是否定期更新好友信息
        public int is_update_discuss { get; set; } = 1;                            //是否定期更新讨论组信息
        public int update_interval { get; set; } = 600;                          //定期更新的时间间隔
    }


}
