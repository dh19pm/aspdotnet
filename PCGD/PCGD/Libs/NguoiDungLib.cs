using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCGD.Models;

namespace PCGD.Libs
{
    public class NguoiDungLib
    {
        private static Models.NguoiDung nguoiDung;
        private const string Key = "!@#$%^&*%%^@#$@$!@#!@$%^&(*&^%$#$%^&*(*&^%$#@";
        static NguoiDungLib()
        {
            if (HttpContext.Current.Session["users" + Key + Libs.Request.Protect()] == null)
            {
                nguoiDung = new Models.NguoiDung();
                HttpContext.Current.Session["users" + Key + Libs.Request.Protect()] = nguoiDung;
            }
            else
            {
                nguoiDung = (Models.NguoiDung)HttpContext.Current.Session["users" + Key + Libs.Request.Protect()];
            }
        }
        public static string Role(int role)
        {
            switch (role)
            {
                case 1:
                    return "Admin";
                default:
                    return "User";
            }
        }
        public static Models.NguoiDung Get()
        {
            return nguoiDung;
        }
        public static void Set(Models.NguoiDung n)
        {
            nguoiDung = n;
        }
        public static void Clear()
        {
            nguoiDung = new Models.NguoiDung();
        }
    }
}