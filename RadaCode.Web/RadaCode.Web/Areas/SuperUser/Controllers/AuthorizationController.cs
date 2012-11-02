using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using RadaCode.Web.Application.Membership;
using RadaCode.Web.Data.Entities;
using putaty.web.Areas.SuperUser.Models;

namespace RadaCode.Web.Areas.SuperUser.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly WebUserMembershipProvider _membershipProvider;

        public AuthorizationController(WebUserMembershipProvider membershipProvider)
        {
            _membershipProvider = membershipProvider;
        }

        public ActionResult Authenticate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authenticate(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_membershipProvider.ValidateUser(model.Name, model.Pazz))
                {

                    var user = _membershipProvider.GetUser(model.Name, true);

                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Projects");
                    }

                    if (!String.IsNullOrEmpty(HttpContext.Request.UrlReferrer.AbsoluteUri))
                        returnUrl = HttpContext.Request.UrlReferrer.AbsoluteUri;

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return new RedirectResult(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "PutatyManagement");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Нет, осталось две попытки!.");
                }
            }

            return View("Authenticate", model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public JsonResult WhereToGo()
        {
            var randomUrl = "http://www.google.com";

            return Json(randomUrl, JsonRequestBehavior.AllowGet);
        }

    }
}
