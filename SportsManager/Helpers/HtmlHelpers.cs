using System.Web.Mvc;

namespace SportsManager.Helpers
{
    // Source: http://stackoverflow.com/a/22408778
    public static class HtmlHelpers
    {
        public static string MakeActive(this HtmlHelper htmlHelper, string controller)
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            var routeController = routeData.Values["controller"].ToString();

            var returnActive = (controller == routeController);

            return returnActive ? "active" : "";
        }
    }
}