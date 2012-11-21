using System;
using System.Web.Mvc;
using RadaCode.Web.Models;

namespace RadaCode.Web.Controllers
{
    public class PortfolioController : Controller
    {
        //
        // GET: /Portfolio/

        public ActionResult Index(string selectedType)
        {
            var portfolioViewModel = new PortfolioViewModel();


            if(!String.IsNullOrEmpty(selectedType))
            {
                portfolioViewModel.SelectedProjectType = selectedType;
            } else
            {
                portfolioViewModel.SelectedProjectType = "web";
            }

            return View(portfolioViewModel);
        }

    }
}
