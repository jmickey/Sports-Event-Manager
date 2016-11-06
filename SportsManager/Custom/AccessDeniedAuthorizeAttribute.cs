using System.Web.Mvc;

namespace SportsManager.Custom
{
    // Inherit from the builtin authorize attribute. Source: http://stackoverflow.com/a/1279854
    public class AccessDeniedAuthorizeAttribute : AuthorizeAttribute
    {
        // Overrise existing OnAuthorization method provided by Microsoft
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var returnUrl = filterContext.HttpContext.Request.Url.LocalPath;
                filterContext.Result = new RedirectResult("~/Account/Login?ReturnUrl=" + returnUrl);
            }

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("~/Account/Denied");
            }
        }
    }
}