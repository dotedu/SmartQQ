using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQQ.API.Http;
using SmartQQ.API.RPC;
//using SmartQQ.tools;

namespace SmartQQ.API
{
    public class SmartQQAPIService
    {
        static HttpClient http;
        public SmartQQAPIService(HttpClient httpClient)
        {
            http = httpClient;
        }
        public static Image GetQRCodeImage()
        {
            string url = ApiUrl.Get_QrCode;
            var bytes = http.GET(url);
            return Image.FromStream(new MemoryStream(bytes));
        }
    }
}
