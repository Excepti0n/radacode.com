using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using RadaCode.Web.Data.Entities;
using RadaCode.Web.Data.Repositories;

namespace RadaCode.Web.Application.Membership
{
    public class WebUserRoleProvider: RoleProvider
    {
        private readonly IWebUserRepository _repository;

        public WebUserRoleProvider()
        {
            _repository = DependencyResolver.Current.GetService<IWebUserRepository>();
        }

        public override bool IsUserInRole(string userName, string roleName)
        {
            var user = _repository.GetUser(userName);
            var role = _repository.GetRole(roleName);

            return _repository.UserExists(user) &&
                   (_repository.RoleExists(role) && user.Roles.Any(curRole => curRole.RoleName == roleName));
        }

        public override string[] GetRolesForUser(string userName)
        {
            var roles = _repository.GetRolesForUser(userName);

            return roles.Any(webUserRole => !_repository.RoleExists(webUserRole)) ? new string[] { string.Empty } : roles.Select(rl => rl.RoleName).ToArray();
        }

        public void ClearUserRoles(string userName)
        {
            _repository.ClearUserRoles(userName);
            _repository.SaveChanges();
        }

        public void AddUserToRole(string userName, string roleName)
        {
            var user = _repository.GetUser(userName);
            var role = _repository.GetRole(roleName);

            if (user.Roles == null) user.Roles = new List<WebUserRole>();

            if (!user.Roles.Contains(role))
            {
                user.Roles.Add(role);
            }

            _repository.SaveChanges();
        }

        public override void CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) return;
            var role = _repository.GetRole(roleName);
            if (role != null) return;
            _repository.AddRole(roleName); 
            _repository.SaveChanges();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }

            var role = _repository.GetRole(roleName);
            if (role == null)
            {
                return false;
            }
            if (throwOnPopulatedRole)
            {
                if (role.Users.Any())
                {
                    return false;
                }
            }
            else
            {
                role.Users.Clear();
            }
            _repository.DeleteRole(role);
            _repository.SaveChanges();
            return true;
        }

        public override bool RoleExists(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }
            
            var role = _repository.GetAllRoles().FirstOrDefault(Rl => Rl.RoleName == roleName);
            return role != null;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var users = _repository.GetUsersAsQueryable().Where(usr => usernames.Contains(usr.UserName)).ToList();
            var roles = _repository.GetAllRoles().Where(Rl => roleNames.Contains(Rl.RoleName)).ToList();
            foreach (var user in users)
            {
                foreach (var role in roles)
                {
                    if (!user.Roles.Contains(role))
                    {
                        user.Roles.Add(role);
                    }
                }
            }
            _repository.SaveChanges();  
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (var username in usernames)
            {
                var user = _repository.GetUser(username);
                if (user != null)
                {
                    foreach (var roleName in roleNames)
                    {
                        var role = _repository.GetRole(roleName);
                        if (role != null)
                        {
                            user.Roles.Remove(role);
                        }
                    }
                }
            }
            _repository.SaveChanges();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return null;
            }
            
            var role = _repository.GetRole(roleName);
            return role != null ? role.Users.Select(usr => usr.UserName).ToArray() : null;
        }

        public override string[] GetAllRoles()
        {
            return _repository.GetAllRoles().Select(Rl => Rl.RoleName).ToArray();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return null;
            }

            return string.IsNullOrEmpty(usernameToMatch) ? null : (from rl in _repository.GetAllRoles() from usr in rl.Users where rl.RoleName == roleName && usr.UserName.Contains(usernameToMatch) select usr.UserName).ToArray();
        }

        public override string ApplicationName
        {
            get
            {
                return this.GetType().Assembly.GetName().Name.ToString();
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                this.ApplicationName = this.GetType().Assembly.GetName().Name.ToString();
            }
        }

        public bool ShouldShowUserAnAdminFeatures(string userName)
        {
            var userRoles = _repository.GetRolesForUser(userName).ToList();

            return userRoles.Any(visitorRole => visitorRole.AdminFeaturesAvailable);
        }

        public bool DoesRoleHaveAnAdminFeatures(string roleName)
        {
            return _repository.GetRole(roleName).AdminFeaturesAvailable;
        }

        public void SetAdminFeaturesAvailabilityForRole(string roleName, bool adminFeaturesAvailabilityValue)
        {
            var role = _repository.GetRole(roleName);

            if (role == null) throw new Exception("No role with such name found");

            role.AdminFeaturesAvailable = adminFeaturesAvailabilityValue;

            _repository.SaveChanges();
        }
    }
}