using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RadaCode.Web.Models
{
    public class PortfolioViewModel
    {
        public string SelectedProjectTypeId { get; set; }
        public List<PortfolioSelectorItem> MenuItems { get; set; } 
    }

    public class PortfolioSelectorItem
    {
        public string ItemText { get; set; }
        public string ItemId { get; set; }
    }
}