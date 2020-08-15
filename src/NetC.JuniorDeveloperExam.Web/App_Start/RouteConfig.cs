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
                name: "addCommentDefault",
                url: "AddComment",
                defaults: new { controller = "Blog", action = "AddComment" }
            );

            routes.MapRoute(
                name: "addComment",
                url: "blog/AddComment",
                defaults: new { controller = "Blog", action = "AddComment"}
            );

            routes.MapRoute(
                name: "blogContent",
                url: "blog/{id}",
                defaults: new { controller = "Blog", action = "BlogContent", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "default",
                url: "",
                defaults: new { controller = "blog", action = "blogcontent", id = 1 }
            );
        }
    }
}
