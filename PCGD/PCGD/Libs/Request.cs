using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PCGD.Libs
{
    public class Request
    {
        public static string GetIP()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return ip;
        }
        public static string GetUserAgent()
        {
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Headers["User-Agent"]))
                return System.Web.HttpContext.Current.Request.Headers["User-Agent"].ToString();
            return null;
        }
        public static string Protect()
        {
            return Libs.Sha1.Convert(Request.GetIP() + "_" + Request.GetUserAgent());
        }
    }
}