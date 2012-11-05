namespace RadaCode.Web.Application.Membership
{
    public class UrlAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public string AuthUrl { get; set; }

        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            filterContext.Result = new System.Web.Mvc.RedirectResult(AuthUrl, false);
        }
    }
}