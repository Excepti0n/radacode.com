using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using RadaCode.Web.Application.MVC;
using RadaCode.Web.Application.Membership;
using RadaCode.Web.Areas.SuperUser.Models;
using RadaCode.Web.Core.Setttings;
using RadaCode.Web.Data.EF;
using RadaCode.Web.Data.Entities;
using putaty.web.Application.Membership;
using putaty.web.Areas.SuperUser.Models;

namespace RadaCode.Web.Areas.SuperUser.Controllers
{
    [UrlAuthorize(Roles = "Admin", AuthUrl = "~/SuperUser/Authorization/Authenticate")]
    public class RadaCodeWebManagementController : Controller
    {
        private readonly IRadaCodeWebSettings _settings;
        private readonly RadaCodeWebStoreContext _context;
        private readonly WebUserMembershipProvider _membershipProvider;
        private readonly WebUserRoleProvider _roleProvider;

        public RadaCodeWebManagementController(RadaCodeWebStoreContext context, IRadaCodeWebSettings settings, WebUserMembershipProvider membershipProvider, WebUserRoleProvider roleProvider)
        {
            _context = context;
            _settings = settings;
            _membershipProvider = membershipProvider;
            _roleProvider = roleProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Users controller

        public ActionResult GetUsersControl()
        {
            var roleNamesArray = _roleProvider.GetAllRoles();

            var roleModels = roleNamesArray.Select(roleName => new RoleModel { RoleName = roleName, RoleUsersCount = _roleProvider.GetUsersInRole(roleName).Count(), AdminFeaturesAvailable = _roleProvider.DoesRoleHaveAnAdminFeatures(roleName) }).ToList();

            MembershipUserCollection users;

            var model = new RolesAndUsersModel
                            {
                                RoleModels = roleModels
                            };

            if (roleNamesArray.Length > 0)
            {
                users = _membershipProvider.GetAllUsersInRole(roleNamesArray[0]);

                foreach (RadaCodeWebMembershipUser user in users)
                {
                    user.Roles = _roleProvider.GetRolesForUser(user.UserName).ToList();
                }

                model.UsersInFirstRole = users.Cast<RadaCodeWebMembershipUser>().ToList();
            }

            var CADict = GetAllControllersAndActions();

            model.AllControllersAndActions = new List<ControllerAndActionsGroupModel>();

            foreach (var entry in CADict)
            {
                if (entry.Value.Count == 0) continue;
                model.AllControllersAndActions.Add(new ControllerAndActionsGroupModel
                                                       {
                                                           ControllerName = entry.Key,
                                                           Actions = entry.Value
                                                       });
            }

            model.PermissionsModels = new List<PermissionModel>();

            foreach (var rolePermission in _context.RolePermissions.ToList())
            {
                model.PermissionsModels.Add(new PermissionModel
                                                {
                                                    Id = rolePermission.Id.ToString(),
                                                    ControllerAndAction = rolePermission.ControllerActionPair,
                                                    PermissionName = rolePermission.PermissionName,
                                                    Roles = rolePermission.VisitorRoles.ToList().Select(role => role.RoleName).ToList()
                                                });
            }

            return PartialView("_Users", model);
        }

        #region Roles

        [HttpPost]
        public ActionResult AddNewRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) return Json("SPCD: NORLPROVIDED");

            try
            {
                _roleProvider.CreateRole(roleName);
            }
            catch (Exception ex)
            {
                return Json("SPCD: ERR - " + ex.Message);
            }

            return Json("SPCD: RLADDED");
        }

        [HttpPost]
        public ActionResult RemoveRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) return Json("SPCD: NORLPROVIDED");

            try
            {
                _roleProvider.DeleteRole(roleName, true);
            }
            catch (Exception ex)
            {
                return Json("SPCD: ERR - " + ex.Message);
            }

            return Json("SPCD: RLREMOVED");
        }

        [HttpGet]
        public JsonResult GetUsersInRole(string roleName)
        {
            var users = _membershipProvider.GetAllUsersInRole(roleName);

            foreach (RadaCodeWebMembershipUser user in users)
            {
                user.Roles = _roleProvider.GetRolesForUser(user.UserName).ToList();
            }

            return Json(new { 
                    status = "SPCD: OK",
                    users = users.Cast<RadaCodeWebMembershipUser>().ToList()
            },
                JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Permissions

        [HttpPost]
        public ActionResult AddRolesToPermission(string permissionId, string newRoles)
        {
            var gid = Guid.Parse(permissionId);

            try
            {
                if (newRoles == null) return Json("SPCD: NORLPROVIDED");

                var permission = _context.RolePermissions.Single(pr => pr.Id == gid);

                permission.VisitorRoles.Clear();

                var rolesList = JsonConvert.DeserializeObject<List<string>>(newRoles);
                foreach (var roleName in rolesList)
                {
                    permission.VisitorRoles.Add(_context.WebUserRoles.Single(rl => rl.RoleName == roleName));
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json("SPCD: ERR - " + ex.Message);
            }

            return Json("SPCD: OK");
        }

        [HttpPost]
        public ActionResult AddCAToPermission(string permissionId, string CA)
        {
            var gid = Guid.Parse(permissionId);

            try
            {
                if (CA == null) return Json("SPCD: NOCAPROVIDED");

                var permission = _context.RolePermissions.Single(pr => pr.Id == gid);

                var CAs = JsonConvert.DeserializeObject<List<string>>(CA);
                foreach (var ca in CAs)
                {
                    permission.ControllerActionPair = ca;
                }
            }
            catch (Exception ex)
            {
                return Json("SPCD: ERR - " + ex.Message);
            }

            return Json("SPCD: OK");
        }

        [HttpPost]
        public ActionResult AddNewPermission(string permissionName, string CAs)
        {
            var res = new AddNewPermissionResultModel();

            if (string.IsNullOrEmpty(permissionName))
            {
                res.status = "SPCD: NOPERNAMEPROVIDED";
                return Json(res);
            }
            if (string.IsNullOrEmpty(CAs))
            {
                res.status = "SPCD: NOCAPROVIDED";
                return Json(res);
            }

            var parsedCAs = JsonConvert.DeserializeObject<List<string>>(CAs);

            var addedPermission = _context.RolePermissions.Add(new RolePermission
                                             {
                                                 ControllerActionPair = parsedCAs[0],
                                                 PermissionName = permissionName
                                             });
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                res.status = "SPCD: ERR - " + ex.Message;
            }

            res.status = "SPCD: PRADDED";
            res.addedModel = new PermissionModel
                                 {
                                     Id = addedPermission.Id.ToString(),
                                     PermissionName = addedPermission.PermissionName,
                                     ControllerAndAction = addedPermission.ControllerActionPair,
                                     Roles = new List<string>()
                                 };
            return Json(res);
        }
        
        [HttpPost]
        public ActionResult RemovePermission(string permissionId)
        {
            if (string.IsNullOrEmpty(permissionId)) return Json("SPCD: NOPRIDPROVIDED");

            var gid = Guid.Parse(permissionId);

            try
            {
                var perm = _context.RolePermissions.Single(pr => pr.Id == gid);
                _context.RolePermissions.Remove(perm);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json("SPCD: ERR - " + ex.Message);
            }

            return Json("SPCD: PRREMOVED");
        }

        [HttpPost]
        public JsonResult SetRoleAdminFeaturesAvailability(string roleName, bool areAdminFeaturesAvailable)
        {
            if (string.IsNullOrEmpty(roleName)) return Json("SPCD: NORLPROVIDED");

            _roleProvider.SetAdminFeaturesAvailabilityForRole(roleName, areAdminFeaturesAvailable);

            return Json("SPCD: AFSET");
        }

        [HttpPost]
        public ActionResult AddUserToRoles(string userName, string newRoles)
        {
            try
            {
                if (newRoles == null) return Json("SPCD: NORLPROVIDED");

                _roleProvider.ClearUserRoles(userName);

                var rolesList = JsonConvert.DeserializeObject<List<string>>(newRoles);
                foreach (var roleName in rolesList)
                {
                    _roleProvider.AddUserToRole(userName, roleName);
                }
            }
            catch (Exception ex)
            {
                return Json("SPCD: ERR - " + ex.Message);
            }

            return Json("SPCD: OK");
        }

        #endregion

        #region Users

        [HttpPost]
        public ActionResult UpdateUserPassword(string userName, string newPass)
        {
            if (_membershipProvider.ChangePassword(userName, newPass)) return Json("SPCD: OK");
            else return Json("SPCD: FAIL");
        }

        [HttpPost]
        public ActionResult DeleteUser(string userName)
        {
            if (_membershipProvider.DeleteUser(userName, true)) return Json("SPCD: OK");
            else return Json("SPCD: FAIL");
        }

        [HttpPost]
        public ActionResult AddNewUser(string userName, string pass, string displayName, string email, string roles)
        {
            if (String.IsNullOrEmpty(roles)) return Json(new { status = "SPCD: ERR-NO-ROLES-PROVIDED" });

            MembershipCreateStatus status;
            var user = (RadaCodeWebMembershipUser)_membershipProvider.CreateUser(userName, pass, email, null, null, true, null, out status);
            if (status != MembershipCreateStatus.Success) return Json(new { status = "SPCD: ERR - " + status.ToString() });

            if (roles != null)
            {
                try
                {
                    var rolesList = JsonConvert.DeserializeObject<List<string>>(roles);
                    foreach (var roleName in rolesList)
                    {
                        _roleProvider.AddUserToRole(user.UserName, roleName);
                    }

                    user.Roles = _roleProvider.GetRolesForUser(user.UserName).ToList();
                }
                catch (Exception ex)
                {
                    return Json(new { status = "SPCD: ERR - " + ex.Message });
                }
            }

            return Json(new { status = "SPCD: OK", user });
        }

        #endregion

        #endregion

        #region Projects controller

        public ActionResult GetProjectsControl()
        {
            
        }

        #endregion

        #region Private Subroutines

        private Dictionary<string, List<string>> GetAllControllersAndActions()
        {
            var controllersAndActions = new Dictionary<string, List<string>>();

            foreach (var controller in GetControllers())
            {
                var newDictionaryEntry = new KeyValuePair<string, List<string>>(controller.Name, new List<string>());

                var controllerDescriptor = new ReflectedControllerDescriptor(controller);

                ActionDescriptor[] actions = controllerDescriptor.GetCanonicalActions();
                foreach (var action in actions)
                {
                    var paramSignatureString = GetParamSignatureString(action);
                    newDictionaryEntry.Value.Add(action.ActionName + paramSignatureString);
                    //controllersAndActions.Add(action.ControllerDescriptor.ControllerName + " -> " + action.ActionName + paramSignatureString);

                }

                controllersAndActions.Add(newDictionaryEntry.Key, newDictionaryEntry.Value);
            }

            return controllersAndActions;
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

        private IEnumerable<Type> GetControllers()
        {
            IEnumerable<Type> typesSoFar = Type.EmptyTypes;
            var assemblies = BuildManager.GetReferencedAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] typesInAsm;
                try
                {
                    typesInAsm = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    typesInAsm = ex.Types;
                }
                typesSoFar = typesSoFar.Concat(typesInAsm);
            }
            return typesSoFar.Where(type =>
                type != null &&
                type.IsPublic &&
                type.IsClass &&
                !type.IsAbstract &&
                typeof(RadaCodeBaseController).IsAssignableFrom(type)
                //typeof(IController).IsAssignableFrom(type)
            );
        }

        #endregion

    }
}
