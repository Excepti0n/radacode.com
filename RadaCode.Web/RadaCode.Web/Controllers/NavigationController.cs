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
                        resHTML = "<a href=\"#\" id=\"setRus\">РУС</a>   <span id=\"setEng\">ENG</span>";
                        break;
                    case "ru":
                        resHTML = "<span id=\"setRus\">РУС</span>   <a href=\"#\" id=\"setEng\">ENG</a>";
                        break;
                    default:
                        resHTML = "<span id=\"setRus\">РУС</span>   <a href=\"#\" id=\"setEng\">ENG</a>";
                        break;
                }
            }
            else
            {
                switch (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "en":
                        resHTML = "<a href=\"#\" id=\"setRus\">РУС</a>   <span id=\"setEng\">ENG</span>";
                        break;
                    default:
                        resHTML = "<a href=\"#\" class=\"selected\" id=\"setRus\">РУС</a>   <a href=\"#\" id=\"setEng\">ENG</a>";
                        break;
                }

            }

            return resHTML;
        }

    }
}
