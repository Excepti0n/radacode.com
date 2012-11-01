using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RadaCode.Web.Data.Entities
{
    public class Customer: IdableEntity
    {
        public string CustomerName { get; set; }
        public virtual Industry Industry { get; set; }
        public string CustomerCompanySize { get; set; }
        public string NetRevenue { get; set; }
        public string WebSiteUrl { get; set; }
    }
}
