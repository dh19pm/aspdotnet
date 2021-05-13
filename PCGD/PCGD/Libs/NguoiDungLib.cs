using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCGD.Models;

namespace PCGD.Libs
{
    public class NguoiDungLib
    {
        private static NguoiDung nguoiDung;
        private const string Key = "!@#$%^&*%%^@#$@$!@#!@$%^&(*&^%$#$%^&*(*&^%$#@";
        static NguoiDungLib()
        {
            if (HttpContext.Current.Session["users" + Key + Libs.Request.Protect()] == null)
            {
                nguoiDung = new NguoiDung();
                HttpContext.Current.Session["users" + Key + Libs.Request.Protect()] = nguoiDung;
            }
            else
            {
                nguoiDung = (NguoiDung)HttpContext.Current.Session["users" + Key + Libs.Request.Protect()];
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
        public static NguoiDung Get()
        {
            return nguoiDung;
        }
        public static void Set(NguoiDung n)
        {
            nguoiDung = n;
        }
        public static void Clear()
        {
            nguoiDung = new NguoiDung();
        }
    }
}