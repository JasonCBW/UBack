using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace UBack
{
    public class LoginAuthorizeFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string actionName = filterContext.RouteData.Values["action"].ToString().ToLower();
            if (actionName == "login" || actionName == "userlogin" || actionName == "loginout") return;
            ContentResult content = new ContentResult();
            var userid = filterContext.HttpContext.Session["userid"];
            if (userid == null)
            {
                ReturnIndex(filterContext);
                return;
            }

            base.OnActionExecuting(filterContext);

        }

        protected void ReturnIndex(ActionExecutingContext actionContext)
        {
            actionContext.HttpContext.Response.Redirect("/WebAdmin/Login/Login");
        }
    }
}