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
        public ActionResult UpdateDisplayName(string userName, string newDisplayName)
        {
            try
            {
                _membershipProvider.UpdateUserDisplayName(userName, newDisplayName);
            }
            catch (Exception ex)
            {
                return Json("SPCD: ERR - " + ex.Message);
            }

            return Json("SPCD: USRNMUPDATED");
        }

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
            var projectsModel = new ProjectsManagementModel
                {
                    Clients = new List<ClientModel>(),
                    CloudProjects = new List<DistributedProjectModel>(),
                    Industries = new List<IndustryModel>(),
                    MobileProjects = new List<MobileProjectModel>(),
                    WebProjects = new List<WebProjectModel>()
                };

            foreach (var industry in _context.Industries.ToList())
            {
                projectsModel.Industries.Add(new IndustryModel
                                                 {
                                                     Id = industry.Id,
                                                     Name = industry.Name
                                                 });
            }

            foreach (var customer in _context.Customers.ToList())
            {
                projectsModel.Clients.Add(new ClientModel
                                              {
                                                  Id = customer.Id,
                                                  CustomerCompanySize = customer.CustomerCompanySize,
                                                  IndustryId = customer.Industry.Id,
                                                  IndustryName = customer.Industry.Name,
                                                  Name = customer.CustomerName,
                                                  NetRevenue = customer.NetRevenue,
                                                  WebSiteUrl = customer.WebSiteUrl
                                              });
            }

            foreach (var sp in _context.SoftwareProjects.ToList())
            {
                if(sp is WebDevelopmentProject)
                {
                    projectsModel.WebProjects.Add(new WebProjectModel
                                                      {
                                                          Id = sp.Id,
                                                          ClientId = sp.Customer.Id,
                                                          ClientName = sp.Customer.CustomerName,
                                                          CurrentUsersCount = sp.CurrentUsersCount,
                                                          DateFinished = sp.DateFinished,
                                                          DateStarted = sp.DateStarted,
                                                          Description = sp.Description,
                                                          IsCloudConnected = sp.IsCloudConnected,
                                                          Name = sp.Name,
                                                          ProjectActualCompletionSpan = sp.ProjectActualCompletionSpan,
                                                          ProjectEstimate = sp.ProjectEstimate,
                                                          ProjectDescriptionMarkup = sp.ProjectDescriptionMarkup,
                                                          ROIpercentage = sp.ROIpercentage, 
                                                          SpecialFeatures = sp.SpecialFeatures,
                                                          TechnologiesUsed = sp.TechnologiesUsed,
                                                          WebSiteUrl = sp.WebSiteUrl
                                                      });
                } else if (sp is MobileDevelopmentProject)
                {
                    projectsModel.MobileProjects.Add(new MobileProjectModel
                    {
                        Id = sp.Id,
                        ClientId = sp.Customer.Id,
                        ClientName = sp.Customer.CustomerName,
                        CurrentUsersCount = sp.CurrentUsersCount,
                        DateFinished = sp.DateFinished,
                        DateStarted = sp.DateStarted,
                        Description = sp.Description,
                        IsCloudConnected = sp.IsCloudConnected,
                        Name = sp.Name,
                        ProjectActualCompletionSpan = sp.ProjectActualCompletionSpan,
                        ProjectEstimate = sp.ProjectEstimate,
                        ProjectDescriptionMarkup = sp.ProjectDescriptionMarkup,
                        ROIpercentage = sp.ROIpercentage,
                        SpecialFeatures = sp.SpecialFeatures,
                        TechnologiesUsed = sp.TechnologiesUsed,
                        WebSiteUrl = sp.WebSiteUrl,
                        PlatformsSupported = (sp as MobileDevelopmentProject).PlatformsSupported
                    });
                } else if (sp is DistributedDevelopmentProject)
                {
                    projectsModel.CloudProjects.Add(new DistributedProjectModel
                    {
                        Id = sp.Id,
                        ClientId = sp.Customer.Id,
                        ClientName = sp.Customer.CustomerName,
                        CurrentUsersCount = sp.CurrentUsersCount,
                        DateFinished = sp.DateFinished,
                        DateStarted = sp.DateStarted,
                        Description = sp.Description,
                        IsCloudConnected = sp.IsCloudConnected,
                        Name = sp.Name,
                        ProjectActualCompletionSpan = sp.ProjectActualCompletionSpan,
                        ProjectEstimate = sp.ProjectEstimate,
                        ProjectDescriptionMarkup = sp.ProjectDescriptionMarkup,
                        ROIpercentage = sp.ROIpercentage,
                        SpecialFeatures = sp.SpecialFeatures,
                        TechnologiesUsed = sp.TechnologiesUsed,
                        WebSiteUrl = sp.WebSiteUrl
                    });
                }
            }

            return PartialView("_Projects", projectsModel);
        }

        #region Industries

        [HttpPost]
        public JsonResult AddIndustry(string name)
        {
            if (String.IsNullOrEmpty(name)) return Json(new { status = "SPCD: PARAM-ERROR" });

            var industry = _context.Industries.Add(new Industry
                                        {
                                            Name = name
                                        });
            try
            {
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                return Json(new {status = "SPCD: ERROR", error = ex.Message});
            }

            return Json(new {status = "SPCD: OK", industry});
        }

        [HttpPost]
        public JsonResult RemoveIndustry(string id)
        {
            if (String.IsNullOrEmpty(id)) return Json(new { status = "SPCD: PARAM-ERROR" });

            var gid = Guid.Parse(id);

            var industryToRemove = _context.Industries.SingleOrDefault(ind => ind.Id == gid);

            if(industryToRemove == null)
            {
                return Json(new { status = "SPCD: NO-IND-FOUND" });
            }

            _context.Industries.Remove(industryToRemove);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { status = "SPCD: ERROR", error = ex.Message });
            }

            return Json(new { status = "SPCD: OK"});
        }

        [HttpPost]
        public JsonResult UpdateIndustry(string id, string newName)
        {
            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(newName)) return Json(new { status = "SPCD: PARAM-ERROR" });

            var gid = Guid.Parse(id);

            var industryToUpdate = _context.Industries.SingleOrDefault(ind => ind.Id == gid);

            if (industryToUpdate == null)
            {
                return Json(new { status = "SPCD: NO-IND-FOUND" });
            }

            industryToUpdate.Name = newName;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { status = "SPCD: ERROR", error = ex.Message });
            }

            return Json(new { status = "SPCD: OK", industry = industryToUpdate });
        }

        #endregion

        #region Customers

        [HttpPost]
        public JsonResult AddCustomer(string name, string industryId, string size, string ravenue, string webUrl)
        {
            if (String.IsNullOrEmpty(name) || 
                String.IsNullOrEmpty(industryId) || 
                String.IsNullOrEmpty(size) || 
                String.IsNullOrEmpty(ravenue) || 
                String.IsNullOrEmpty(webUrl)) return Json(new { status = "SPCD: PARAM-ERROR" });

            var inId = Guid.Parse(industryId);

            var industry = _context.Industries.SingleOrDefault(ind => ind.Id == inId);

            if (industry == null) return Json(new { status = "SPCD: NO-IND-FOUND" });

            var customer =_context.Customers.Add(new Customer
                                       {
                                           CustomerName = name,
                                           CustomerCompanySize = size,
                                           Industry = industry,
                                           NetRevenue = ravenue,
                                           WebSiteUrl = webUrl
                                       }
                );

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { status = "SPCD: ERROR", error = ex.Message });
            }

            var custModel = new ClientModel
                                {
                                    CustomerCompanySize = customer.CustomerCompanySize,
                                    Id = customer.Id,
                                    IndustryId = customer.Industry.Id,
                                    IndustryName = customer.Industry.Name,
                                    Name = customer.CustomerName,
                                    NetRevenue = customer.NetRevenue,
                                    WebSiteUrl = customer.WebSiteUrl
                                };

            return Json(new { status = "SPCD: OK", customer = custModel });
        }

        [HttpPost]
        public JsonResult RemoveCustomer(string id)
        {
            if (String.IsNullOrEmpty(id)) return Json(new { status = "SPCD: PARAM-ERROR" });

            var gid = Guid.Parse(id);

            var customerToRemove = _context.Customers.SingleOrDefault(ct => ct.Id == gid);

            if (customerToRemove == null)
            {
                return Json(new { status = "SPCD: NO-CUST-FOUND" });
            }

            _context.Customers.Remove(customerToRemove);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { status = "SPCD: ERROR", error = ex.Message });
            }

            return Json(new { status = "SPCD: OK" });
        }

        [HttpPost]
        public JsonResult UpdateCustomer(string id, string name, string industryId, string size, string ravenue, string webUrl)
        {
            if (String.IsNullOrEmpty(id) || 
                String.IsNullOrEmpty(name) || 
                String.IsNullOrEmpty(industryId) || 
                String.IsNullOrEmpty(size) || 
                String.IsNullOrEmpty(ravenue) || 
                String.IsNullOrEmpty(webUrl)) return Json(new { status = "SPCD: PARAM-ERROR" });

            var inId = Guid.Parse(industryId);

            var industryToUpdate = _context.Industries.SingleOrDefault(ind => ind.Id == inId);

            if (industryToUpdate == null)
            {
                return Json(new { status = "SPCD: NO-IND-FOUND" });
            }

            var ctId = Guid.Parse(id);

            var customerToUpdate = _context.Customers.SingleOrDefault(ct => ct.Id == ctId);

            if (customerToUpdate == null)
            {
                return Json(new { status = "SPCD: NO-CUST-FOUND" });
            }

            customerToUpdate.CustomerName = name;
            customerToUpdate.CustomerCompanySize = size;
            customerToUpdate.Industry = industryToUpdate;
            customerToUpdate.NetRevenue = ravenue;
            customerToUpdate.WebSiteUrl = webUrl;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { status = "SPCD: ERROR", error = ex.Message });
            }

            return Json(new { status = "SPCD: OK", customer = customerToUpdate });
        }

        #endregion

        #region Projects

        [HttpPost]
        public JsonResult AddProject(string type, string name, string description, string customerId, string[] technologiesUsed, string dateStarted, string dateFinished, string estimate, string usersCount, string roi, string[] specialFeatures, string isCloudConnected, string markup, string webUrl, string[] platformsSupported)
        {
            if (String.IsNullOrEmpty(name) ||
                String.IsNullOrEmpty(description) ||
                String.IsNullOrEmpty(customerId) ||
                !technologiesUsed.Any() ||
                String.IsNullOrEmpty(dateStarted) ||
                String.IsNullOrEmpty(dateFinished) ||
                String.IsNullOrEmpty(estimate) ||
                String.IsNullOrEmpty(usersCount) ||
                String.IsNullOrEmpty(roi) ||
                String.IsNullOrEmpty(isCloudConnected) ||
                String.IsNullOrEmpty(markup) ||
                String.IsNullOrEmpty(type) ||
                String.IsNullOrEmpty(webUrl)) return Json(new { status = "SPCD: PARAM-ERROR" });

            var cusId = Guid.Parse(customerId);

            var customer = _context.Customers.SingleOrDefault(cs => cs.Id == cusId);

            if (customer == null) return Json(new { status = "SPCD: NO-CUST-FOUND" });

            SoftwareProject projectToAdd;

            switch (type)
            {
                case "Web":
                    projectToAdd = new WebDevelopmentProject
                                       {
                                           Name = name,
                                           CurrentUsersCount = int.Parse(usersCount),
                                           Customer = customer,
                                           DateFinished = DateTime.Parse(dateFinished),
                                           DateStarted = DateTime.Parse(dateStarted),
                                           Description = description,
                                           IsCloudConnected = bool.Parse(isCloudConnected),
                                           ProjectDescriptionMarkup = markup,
                                           ProjectEstimate = TimeSpan.Parse(estimate),
                                           ROIpercentage = int.Parse(roi),
                                           WebSiteUrl = webUrl,
                                           SpecialFeatures = specialFeatures.ToList(),
                                           TechnologiesUsed = technologiesUsed.ToList()
                                       };
                    break;
                case "Mobile":
                    projectToAdd = new MobileDevelopmentProject
                    {
                        Name = name,
                        CurrentUsersCount = int.Parse(usersCount),
                        Customer = customer,
                        DateFinished = DateTime.Parse(dateFinished),
                        DateStarted = DateTime.Parse(dateStarted),
                        Description = description,
                        IsCloudConnected = bool.Parse(isCloudConnected),
                        ProjectDescriptionMarkup = markup,
                        ProjectEstimate = TimeSpan.Parse(estimate),
                        ROIpercentage = int.Parse(roi),
                        WebSiteUrl = webUrl,
                        SpecialFeatures = specialFeatures.ToList(),
                        TechnologiesUsed = technologiesUsed.ToList(),
                        PlatformsSupported = platformsSupported.ToList()
                    };
                    break;
                case "Distributed":
                    projectToAdd = new DistributedDevelopmentProject
                    {
                        Name = name,
                        CurrentUsersCount = int.Parse(usersCount),
                        Customer = customer,
                        DateFinished = DateTime.Parse(dateFinished),
                        DateStarted = DateTime.Parse(dateStarted),
                        Description = description,
                        IsCloudConnected = bool.Parse(isCloudConnected),
                        ProjectDescriptionMarkup = markup,
                        ProjectEstimate = TimeSpan.Parse(estimate),
                        ROIpercentage = int.Parse(roi),
                        WebSiteUrl = webUrl,
                        SpecialFeatures = specialFeatures.ToList(),
                        TechnologiesUsed = technologiesUsed.ToList()
                    };
                    break;
                default:
                    return Json(new { status = "SPCD: UNKNOWN-TYPE" });
            }

            _context.SoftwareProjects.Add(projectToAdd);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { status = "SPCD: ERROR", error = ex.Message });
            }

            return Json(new { status = "SPCD: OK", project = projectToAdd });
        }

        [HttpPost]
        public JsonResult RemoveProject(string id)
        {
            if (String.IsNullOrEmpty(id)) return Json(new { status = "SPCD: PARAM-ERROR" });

            var gid = Guid.Parse(id);

            var projectToRemove = _context.SoftwareProjects.SingleOrDefault(pr => pr.Id == gid);

            if (projectToRemove == null)
            {
                return Json(new { status = "SPCD: NO-CUST-FOUND" });
            }

            _context.SoftwareProjects.Remove(projectToRemove);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { status = "SPCD: ERROR", error = ex.Message });
            }

            return Json(new { status = "SPCD: OK" });
        }

        [HttpPost]
        public JsonResult UpdateProject(string id, string type, string name, string description, string customerId, string[] technologiesUsed, string dateStarted, string dateFinished, string estimate, string usersCount, string roi, string[] specialFeatures, string isCloudConnected, string markup, string webUrl, string[] platformsSupported)
        {
            if (String.IsNullOrEmpty(id) ||
                String.IsNullOrEmpty(description) ||
                String.IsNullOrEmpty(customerId) ||
                !technologiesUsed.Any() ||
                String.IsNullOrEmpty(dateStarted) ||
                String.IsNullOrEmpty(dateFinished) ||
                String.IsNullOrEmpty(estimate) ||
                String.IsNullOrEmpty(usersCount) ||
                String.IsNullOrEmpty(roi) ||
                String.IsNullOrEmpty(isCloudConnected) ||
                String.IsNullOrEmpty(markup) ||
                String.IsNullOrEmpty(type) ||
                String.IsNullOrEmpty(webUrl)) return Json(new { status = "SPCD: PARAM-ERROR" });

            var cusId = Guid.Parse(customerId);

            var customer = _context.Customers.SingleOrDefault(cs => cs.Id == cusId);

            if (customer == null) return Json(new { status = "SPCD: NO-CUST-FOUND" });

            var prId = Guid.Parse(id);

            var projectToUpdate = _context.SoftwareProjects.SingleOrDefault(pr => pr.Id == prId);

            if (projectToUpdate == null) return Json(new { status = "SPCD: NO-PR-FOUND" });

            projectToUpdate.Name = name;
            projectToUpdate.CurrentUsersCount = int.Parse(usersCount);
            projectToUpdate.Customer = customer;
            projectToUpdate.DateFinished = DateTime.Parse(dateFinished);
            projectToUpdate.DateStarted = DateTime.Parse(dateStarted);
            projectToUpdate.Description = description;
            projectToUpdate.IsCloudConnected = bool.Parse(isCloudConnected);
            projectToUpdate.ProjectDescriptionMarkup = markup;
            projectToUpdate.ProjectEstimate = TimeSpan.Parse(estimate);
            projectToUpdate.ROIpercentage = int.Parse(roi);
            projectToUpdate.WebSiteUrl = webUrl;
            projectToUpdate.SpecialFeatures = specialFeatures.ToList();
            projectToUpdate.TechnologiesUsed = technologiesUsed.ToList();
            projectToUpdate.CurrentUsersCount = int.Parse(usersCount);

            if(type == "Mobile")
            {
                var mobileDevelopmentProject = projectToUpdate as MobileDevelopmentProject;
                if (mobileDevelopmentProject != null)
                    mobileDevelopmentProject.PlatformsSupported = platformsSupported.ToList();
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { status = "SPCD: ERROR", error = ex.Message });
            }

            return Json(new { status = "SPCD: OK", project = projectToUpdate });
        }

        #endregion

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
