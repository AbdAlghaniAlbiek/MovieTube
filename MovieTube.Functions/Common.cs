using System;
using System.Net;

namespace MovieTube.Functions
{
    public class Common
    {
        //We use this statment when we connect to another device (this device is a server)
        //private static readonly string BASE_URL = "http://192.168.43.117";
        private static readonly string BASE_URL = "http://" + GetIPAddress();
        private static readonly string PORT = "3000";
        public static readonly string URL = BASE_URL + ":" + PORT + "/api";
        public static readonly string BANNERS = BASE_URL + ":" + PORT + "/public/loading/images/banners/";
        public static readonly string ACTORS = BASE_URL + ":" + PORT + "/public/loading/images/actors/";
        public static readonly string POSTERS = BASE_URL + ":" + PORT + "/public/loading/images/posters/";
        public static readonly string MOVIES_PHOTOS = BASE_URL + ":" + PORT + "/public/loading/images/movies_photos/";
        public static readonly string MOVIES = BASE_URL + ":" + PORT + "/public/loading/videos/movies/";
        public static readonly string TRAILERS = BASE_URL + ":" + PORT + "/public/loading/videos/trailers/";
        public static readonly string USERS = BASE_URL + ":" + PORT + "/public/upload/images/users/";

        public static readonly string JWT_AUTH_KEY = "hviui5125pbvl5124nunoinp315983bkbfai3412obua308517fbiaqour1";
        public static readonly string AES_KEY = "341%p3s$fc*^g4r#";
        public static readonly string AES_IV = "jpbwso01$80*&rw-";
        public static readonly string SECRET_KEYWORD = "U4dmXgA/QZv9i7JA3mIpGw==";

        public static Models.User User { set; get; } = null;

        public static string TOKEN => $"Bearer {User.Token}";


        public static string GetIPAddress()
        {
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }
            return string.Empty;
        }
    }
}
