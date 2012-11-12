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
        public string Name_En { get; set; }
    }

    public class ClientModel: IdableEntity
    {
        public string Name { get; set; }
        public string Name_En { get; set; }
        public Guid IndustryId { get; set; }
        public string IndustryName { get; set; }
        public string IndustryName_En { get; set; }
        public string CustomerCompanySize { get; set; }
        public string NetRevenue { get; set; }
        public string WebSiteUrl { get; set; }
    }

    public abstract class ProjectModel: IdableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Name_En { get; set; }
        public string Description_En { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientName_En { get; set; }
        public virtual IList<string> TechnologiesUsed { get; set; }
        public string DateStarted { get; set; }
        public string ProjectEstimate { get; set; }
        public string ProjectActualCompletionSpan { get; set; }
        public string DateFinished { get; set; }
        public string WebSiteUrl { get; set; }
        public int CurrentUsersCount { get; set; }
        public int ROIpercentage { get; set; }
        public virtual IList<string> SpecialFeatures { get; set; }
        public virtual IList<string> SpecialFeatures_En { get; set; }
        public bool IsCloudConnected { get; set; }
        public string ProjectDescriptionMarkup { get; set; }
        public string ProjectDescriptionMarkup_En { get; set; }
        public virtual string Type
        {
            get { return "ROOT"; }
        }
    }

    public class WebProjectModel: ProjectModel
    {
        public override string Type
        {
            get { return "Web"; }
        }
    }

    public class MobileProjectModel: ProjectModel
    {
        public IList<string> PlatformsSupported { get; set; }

        public override string Type
        {
            get { return "Mobile"; }
        }
    }

    public class DistributedProjectModel: ProjectModel
    {
        public override string Type
        {
            get { return "Distributed"; }
        }
    }

    public class ProjectsManagementModel
    {
        public List<IndustryModel> Industries { get; set; }
        public List<ClientModel> Clients { get; set; }
        public List<WebProjectModel> WebProjects { get; set; }
        public List<MobileProjectModel> MobileProjects { get; set; }
        public List<DistributedProjectModel> CloudProjects { get; set; }
        public List<ProjectModel> Projects { get; set; }
        public IList<string> Types { get; set; } 
    }
}