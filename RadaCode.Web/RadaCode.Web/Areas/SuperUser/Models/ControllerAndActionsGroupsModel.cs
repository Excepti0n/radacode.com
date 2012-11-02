using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace putaty.web.Areas.SuperUser.Models
{
    public class ControllerAndActionsGroupModel
    {
        public string ControllerName { get; set; }
        public List<string> Actions { get; set; } 
    }
}