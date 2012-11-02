using System.Web.Mvc;

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
            context.MapRoute(
                "SuperUser_default",
                "SuperUser/{controller}/{action}/{id}",
                new { controller = "PutatyManagement", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
