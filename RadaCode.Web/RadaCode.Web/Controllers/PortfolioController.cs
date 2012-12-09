using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using RadaCode.Web.Application.MVC;
using RadaCode.Web.Core.Setttings;
using RadaCode.Web.Data.EF;
using RadaCode.Web.Data.Entities;
using RadaCode.Web.Models;

namespace RadaCode.Web.Controllers
{
    public class PortfolioController : RadaCodeBaseController
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

            if (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName == "en")
            {
                portfolioViewModel.MenuItems = new List<PortfolioSelectorItem>()
                {
                    new PortfolioSelectorItem
                        {
                            ItemId = "web",
                            ItemText = "WEB"
                        },
                    new PortfolioSelectorItem
                        {
                            ItemId = "mobile",
                            ItemText = "MOBILE"
                        },
                    new PortfolioSelectorItem
                        {
                            ItemId = "cloud",
                            ItemText = "CLOUD"
                        }
                };
            } else 
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

            //portfolioViewModel.InitialProjects = GetFakeProjectModels(); 
            portfolioViewModel.InitialProjects = GetProjectModelForType(portfolioViewModel.SelectedProjectTypeId, 0);
            portfolioViewModel.ProjectsPerPageCount = _settings.PortfolioProjectsCount;

            portfolioViewModel.TotalProjectsPerType = new Dictionary<string, int>
                                                          {
                                                              {"cloud", GetTotalProjectsForType("cloud")},
                                                              {"web", GetTotalProjectsForType("web")},
                                                              {"mobile", GetTotalProjectsForType("mobile")}
                                                          };

            // Test stuff
            //portfolioViewModel.TotalProjectsPerType = new Dictionary<string, int>
            //                                              {
            //                                                  {"cloud", 25},
            //                                                  {"web", 13},
            //                                                  {"mobile", 30}
            //                                              };

            return View(portfolioViewModel);
        }

        [HttpGet]
        public ActionResult GetNextProjects(string page, string type)
        {
            List<PortfolioProjectItem> res;
            int pageInt;

            //res = GetFakeProjectModels(); 
            res = GetProjectModelForType(type, int.TryParse(page, out pageInt) ? pageInt : 0);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private List<PortfolioProjectItem> GetFakeProjectModels()
        {
            var generatedEnumerable = new List<SoftwareProject>();

            var etalonProject = _context.SoftwareProjects.Where(pr => pr is WebDevelopmentProject).Take(1).FirstOrDefault();

            for (int i = 0; i < 10; i++)
            {
                etalonProject.Id = Guid.NewGuid();
                generatedEnumerable.Add(etalonProject);
            }

            return TransformProjectsIntoModels(generatedEnumerable);
        }

        private List<PortfolioProjectItem> GetProjectModelForType(string type, int page)
        {
            switch (type)
            {
                case "web":
                    return TransformProjectsIntoModels(_context.SoftwareProjects.Where(pr => pr is WebDevelopmentProject).OrderBy(pr => pr.DateStarted).Skip(page * _settings.PortfolioProjectsCount).Take(_settings.PortfolioProjectsCount).ToList());
                    break;
                case "cloud":
                    return TransformProjectsIntoModels(_context.SoftwareProjects.Where(pr => pr is DistributedDevelopmentProject).OrderBy(pr => pr.DateStarted).Skip(page * _settings.PortfolioProjectsCount).Take(_settings.PortfolioProjectsCount).ToList());
                    break;
                case "mobile":
                    return TransformProjectsIntoModels(_context.SoftwareProjects.Where(pr => pr is MobileDevelopmentProject).OrderBy(pr => pr.DateStarted).Skip(page * _settings.PortfolioProjectsCount).Take(_settings.PortfolioProjectsCount).ToList());
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

        private int GetTotalProjectsForType(string type)
        {
            switch (type)
            {
                case "web":
                    return _context.SoftwareProjects.Count(pr => pr is WebDevelopmentProject);
                    break;
                case "cloud":
                    return _context.SoftwareProjects.Count(pr => pr is DistributedDevelopmentProject);
                    break;
                case "mobile":
                    return _context.SoftwareProjects.Count(pr => pr is MobileDevelopmentProject);
                    break;
                default:
                    throw new Exception("Unknown project type polled");
            }
        }
    }
}
