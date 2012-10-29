using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace RadaCode.Web.Controllers
{
    public class NavigationController : Controller
    {
        public ActionResult RenderTopMenu()
        {
            return PartialView("_TopMenu");
        }

        public string RenderLanguages()
        {
            string resHTML;

            string langCookie = null;

            try
            {
                langCookie = Request.Cookies["language"].Value;
            }
            catch { }


            if (!string.IsNullOrEmpty(langCookie))
            {
                switch (langCookie)
                {
                    case "en":
                        resHTML = "<a href=\"#\" id=\"setRus\">рус</a> | <span id=\"setUkr\">укр</span>";
                        break;
                    case "ru":
                        resHTML = "<span id=\"setRus\">рус</span> | <a href=\"#\" id=\"setUkr\">укр</a>";
                        break;
                    default:
                        resHTML = "<span id=\"setRus\">рус</span> | <a href=\"#\" id=\"setUkr\">укр</a>";
                        break;
                }
            }
            else
            {
                switch (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "en":
                        resHTML = "<a href=\"#\" id=\"setRus\">рус</a> | <a href=\"#\" class=\"selected\" id=\"setUkr\">укр</a>";
                        break;
                    default:
                        resHTML = "<a href=\"#\" class=\"selected\" id=\"setRus\">рус</a> | <a href=\"#\" id=\"setUrk\">укр</a>";
                        break;
                }

            }

            return resHTML;
        }

    }
}
