using System.Web.Mvc;
using System.Web.Routing;

namespace RadaCode.Web.Areas.SuperUser
{
    public class SuperUserAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SuperUser";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var dataTokens = new RouteValueDictionary();
            var ns = new string[] { "RadaCode.Web.Areas.SuperUser.Controllers" };

            dataTokens["Namespaces"] = ns;
            dataTokens["Area"] = this.AreaName;

            var areaRoute = new Route(
                                    "SuperUser/{controller}/{action}/{id}",                                                           // Route URL
                                    new RouteValueDictionary{
                                            { "area" , this.AreaName}, 
                                            { "controller" , "RadaCodeWebManagement"}, 
                                            { "action" , "Index"}, 
                                            { "id" , UrlParameter.Optional }
                                        },
                                    null,
                                    dataTokens,
                                    new MvcRouteHandler()                                              
                                    );

            context.Routes.Insert(2, areaRoute);
        }
    }
}
