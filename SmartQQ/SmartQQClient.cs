using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQQ.API;
using SmartQQ.API.RPC;
using System.Drawing;

namespace SmartQQ
{
    public class SmartQQClient
    {
        public SmartQQClient()
        {
            api = new SmartQQAPIService(new API.Http.HttpClient());
        }
        private SmartQQAPIService api = null;
        private string mPass_ticket;
        private BaseRequest mBaseReq;


        public bool IsLogin { get; private set; }



        //callback
        public Action<Image> OnGetQRCodeImage;
        public Action<Image> OnUserScanQRCode;
        public Action OnLoginSucess;
        public Action OnInitComplate;
        public Action<User> OnAddUser;
        public Action<User> OnUpdateUser;
        public Action<AddMsg> OnRecvMsg;
    }
}
