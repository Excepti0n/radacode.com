using System.Collections.Generic;

namespace RadaCode.Web.Models
{
    public class PortfolioViewModel
    {
        public string SelectedProjectTypeId { get; set; }
        public List<PortfolioSelectorItem> MenuItems { get; set; }
        public List<PortfolioProjectItem> InitialProjects { get; set; } 
    }

    public class PortfolioSelectorItem
    {
        public string ItemText { get; set; }
        public string ItemId { get; set; }
    }

    public class PortfolioProjectItem
    {
        public string Id { get; set; }
        public string DateFinished { get; set; }
        public string Markup { get; set; }
    }
}