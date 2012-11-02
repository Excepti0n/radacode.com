using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RadaCode.Web.Data.Entities
{
    public class RolePermission: IdableEntity
    {
        [Required]
        public string PermissionName { get; set; }

        public string Description { get; set; }

        public virtual IList<WebUserRole> VisitorRoles { get; set; }

        [Required]
        public string ControllerActionPair { get; set; }

        public string TargetController
        {
            get { return ControllerActionPair.Substring(0, ControllerActionPair.IndexOf(';') + 1); }
        }

        public string TargetAction
        {
            get { return ControllerActionPair.Substring(ControllerActionPair.IndexOf(';')); }
        }
    }


}
