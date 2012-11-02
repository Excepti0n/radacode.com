using System.Linq;
using System.Web.Mvc;
using RadaCode.Web.Application.Membership;
using RadaCode.Web.Data.EF;

namespace RadaCode.Web.Application.ActionFilters
{
    public class PermissionsBasedAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userIsAllowedToPerformAction = false;

            var dataContext = DependencyResolver.Current.GetService<RadaCodeWebStoreContext>();
            var roleProvider = DependencyResolver.Current.GetService<WebUserRoleProvider>();

            //var controllerName = context.Controller.ControllerContext.RouteData.Values["Controller"];
            var controllerName = context.Controller.GetType().Name;

            var actionSignature = context.ActionDescriptor.ActionName +
                                      GetParamSignatureString(context.ActionDescriptor);

            var currentControllerActionCombination = controllerName + ";" + actionSignature;

            var permission =
                dataContext.RolePermissions.SingleOrDefault(
                    pr => pr.ControllerActionPair == currentControllerActionCombination);

            var currentUserRoles = roleProvider.GetRolesForUser(context.HttpContext.User.Identity.Name);

            foreach (var currentUserRole in currentUserRoles)
            {
                foreach (var role in permission.VisitorRoles.ToList())
                {
                    if(role.RoleName == currentUserRole)
                    {
                        userIsAllowedToPerformAction = true;
                        break;
                    }
                }
            }
            
            if(!userIsAllowedToPerformAction)
            {
                context.Result = new HttpUnauthorizedResult();
            }
        }

        private string GetParamSignatureString(ActionDescriptor action)
        {
            var res = "(";

            ReflectedActionDescriptor aD = action as ReflectedActionDescriptor;

            foreach (var parameterDescriptor in aD.GetParameters())
            {
                res += parameterDescriptor.ParameterType.Name + " " + parameterDescriptor.ParameterName + ", ";
            }

            if (res == "(")
            {
                res += ")";
                return res;
            }

            if (res.Substring(res.Length - 2) == ", ")
            {
                res = res.Substring(0, res.Length - 2);
                res += ")";
            }

            return res;
        }
    }
}