using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RadaCode.Web.Core.Setttings;
using RadaCode.Web.Data.EF;
using RadaCode.Web.Data.Entities;
using RadaCode.Web.Models;

namespace RadaCode.Web.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly IRadaCodeWebSettings _settings;
        private readonly RadaCodeWebStoreContext _context;

        public PortfolioController(RadaCodeWebStoreContext context, IRadaCodeWebSettings settings)
        {
            _context = context;
            _settings = settings;
        }

        public ActionResult Index(string selectedTypeId)
        {
            var portfolioViewModel = new PortfolioViewModel();
            
            if(!String.IsNullOrEmpty(selectedTypeId))
            {
                portfolioViewModel.SelectedProjectTypeId = selectedTypeId;
            } else
            {
                portfolioViewModel.SelectedProjectTypeId = "web";
            }

            portfolioViewModel.MenuItems = new List<PortfolioSelectorItem>()
                {
                    new PortfolioSelectorItem
                        {
                            ItemId = "web",
                            ItemText = "САЙТЫ"
                        },
                    new PortfolioSelectorItem
                        {
                            ItemId = "mobile",
                            ItemText = "МОБИЛЬНЫЕ ПРИЛОЖЕНИЯ"
                        },
                    new PortfolioSelectorItem
                        {
                            ItemId = "cloud",
                            ItemText = "ОБЛАЧНЫЕ РЕШЕНИЯ"
                        }
                };

            portfolioViewModel.InitialProjects = GetProjectModelForType(portfolioViewModel.SelectedProjectTypeId);

            return View(portfolioViewModel);
        }

        private List<PortfolioProjectItem> GetProjectModelForType(string type)
        {
            switch (type)
            {
                case "web":
                    return TransformProjectsIntoModels(_context.SoftwareProjects.Where(pr => pr is WebDevelopmentProject).Take(10).ToList());
                    break;
                case "cloud":
                    return TransformProjectsIntoModels(_context.SoftwareProjects.Where(pr => pr is DistributedDevelopmentProject).Take(10).ToList());
                    break;
                case "mobile":
                    return TransformProjectsIntoModels(_context.SoftwareProjects.Where(pr => pr is MobileDevelopmentProject).Take(10).ToList());
                    break;
                default:
                    return new List<PortfolioProjectItem>();
            }
        }

        private List<PortfolioProjectItem> TransformProjectsIntoModels(IEnumerable<SoftwareProject> softwareProjects)
        {
            return softwareProjects.Select(softwareProject => new PortfolioProjectItem
                                                                  {
                                                                      Id = softwareProject.Id.ToString(), DateFinished = softwareProject.DateFinished.ToString("yyyy-MM-dd"), Markup = softwareProject.ProjectDescriptionMarkup_Usr
                                                                  }).ToList();
        }
    }
}
