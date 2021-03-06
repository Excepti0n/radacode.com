﻿using System;
using System.Collections.Generic;
using RadaCode.Web.Application.Membership;
using putaty.web.Areas.SuperUser.Models;

namespace RadaCode.Web.Areas.SuperUser.Models
{
    public class RolesAndUsersModel
    {
        public List<RoleModel> RoleModels { get; set; }
        public List<PermissionModel> PermissionsModels { get; set; } 
        public List<RadaCodeWebMembershipUser> UsersInFirstRole { get; set; }

        public List<ControllerAndActionsGroupModel> AllControllersAndActions { get; set; } 
    }

    public class RoleModel
    {
        public string RoleName { get; set; }
        public int RoleUsersCount { get; set; }
        public bool AdminFeaturesAvailable { get; set; }
    }

    public class PermissionModel
    {
        public string Id { get; set; }
        public string PermissionName { get; set; }
        public List<String> Roles { get; set; }
        public string ControllerAndAction { get; set; } 
    }
}