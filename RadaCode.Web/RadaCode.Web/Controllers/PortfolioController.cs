using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RadaCode.Web.Models;

namespace RadaCode.Web.Controllers
{
    public class PortfolioController : Controller
    {
        //
        // GET: /Portfolio/

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

            return View(portfolioViewModel);
        }

    }
}
