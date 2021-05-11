using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PCGD.Libs;

namespace PCGD
{
    public class RoleAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedRoles;
        public RoleAttribute(params string[] roles)
        {
            this.allowedRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            foreach (string role in allowedRoles)
                if (role == NguoiDungLib.Role(NguoiDungLib.Get().QuyenHan))
                    return true;
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(string.Format("/Home/Unauthorized?targetUrl={0}", filterContext.HttpContext.Request.Url.AbsolutePath));
        }
    }
}