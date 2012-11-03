using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RadaCode.Web.Data.Entities;

namespace RadaCode.Web.Areas.SuperUser.Models
{
    public class IndustryModel: IdableEntity 
    {
        public string Name { get; set; }
    }

    public class ClientModel: IdableEntity
    {
        public string Name { get; set; }
        public Guid IndustryId { get; set; }
        public string IndustryName { get; set; }
        public string CustomerCompanySize { get; set; }
        public string NetRevenue { get; set; }
        public string WebSiteUrl { get; set; }
    }

    public abstract class ProjectModel: IdableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
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

    public class WebProjectModel: ProjectModel
    {
    }

    public class MobileProjectMode: ProjectModel
    {
        public List<string> PlatformsSupported { get; set; }
    }

    public class ProjectsManagementModel
    {
        public List<IndustryModel> Industries { get; set; }
        public List<ClientModel> ClientModels { get; set; }
        public List<WebProjectModel> WebProjects { get; set; }
        public List<MobileProjectModel> MobileProjects { get; set; } 
    }
}