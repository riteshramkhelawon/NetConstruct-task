using System.Web.Mvc;
using System.Web.Routing;

namespace NetC.JuniorDeveloperExam.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Blog", action = "BlogContent", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "blogContent",
            url: "blog/blogcontent/{blogId}",
            defaults: new { controller = "Blog", action = "BlogContent", id = UrlParameter.Optional }
        );

        }
    }
}
