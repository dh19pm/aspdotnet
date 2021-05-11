using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using PCGD.Libs;

namespace PCGD
{
    public class Authentication : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (string.IsNullOrEmpty(Libs.NguoiDungLib.Get().TaiKhoan))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult(string.Format("/Home/Login?targetUrl={0}", HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.AbsolutePath)));
            }
        }
    }
}