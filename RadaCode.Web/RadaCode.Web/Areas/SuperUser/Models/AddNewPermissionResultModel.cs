using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace putaty.web.Areas.SuperUser.Models
{
    public class AddNewPermissionResultModel
    {
        public string status { get; set; }
        public PermissionModel addedModel { get; set; }
    }
}