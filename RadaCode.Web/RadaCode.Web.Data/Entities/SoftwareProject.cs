using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadaCode.Web.Data.Entities
{
    public class SoftwareProject: IdableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual List<string> TechnologiesUsed { get; set; }
        public DateTime? DateStarted { get; set; }
        public TimeSpan ProjectEstimate { get; set; }
        public TimeSpan ProjectActualCompletionSpan { get; set; }
        public DateTime DateFinished { get; set; }
        public string WebSiteUrl { get; set; }
        public int CurrentUsersCount { get; set; }
        public int ROIpercentage { get; set; }
        public virtual List<string> SpecialFeatures { get; set; }
        public bool IsCloudConnected { get; set; }
        public string ProjectDescriptionMarkup { get; set; }
    }

    public class WebDevelopmentProject: SoftwareProject
    {
        
    }

    public class MobileDevelopment: SoftwareProject
    {
        public virtual List<string> PlatformsSupported { get; set; }
    }
}
