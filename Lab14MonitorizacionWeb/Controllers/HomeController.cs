using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Lab14MonitorizacionWeb.Controllers
{
    public class HomeController : Controller
    {
        [SharePointContextFilter]
        public ActionResult Index()
        {
            #region Comment

            //User spUser = null;

            //var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);

            //using (var clientContext = spContext.CreateUserClientContextForSPHost())
            //{
            //    if (clientContext != null)
            //    {
            //        spUser = clientContext.Web.CurrentUser;

            //        clientContext.Load(spUser, user => user.Title);

            //        clientContext.ExecuteQuery();

            //        ViewBag.UserName = spUser.Title;
            //    }
            //}

            #endregion

            if (Session["sp"] == null)

            {
                Session["sp"] = SharePointAcsContextProvider.Current.GetSharePointContext(HttpContext);
            }
            
            return View();
        }


        public ActionResult Diagnosticos()
        {
            Configuration currentConfig = WebConfigurationManager.OpenWebConfiguration("~");
            TraceSection traceSection = (TraceSection) currentConfig.GetSection("system.web/trace");
            ViewBag.TracingStatus = traceSection.Enabled;

            return View();

        }

        public ActionResult ToggleTracing(bool estado)
        {
            Configuration currentConfig = WebConfigurationManager.OpenWebConfiguration("~");
            TraceSection traceSection = (TraceSection)currentConfig.GetSection("system.web/trace");
            traceSection.Enabled = estado;
            currentConfig.Save();
            ViewBag.TracingStatus = estado;

            return View("Diagnosticos");

        }

        #region Metodos comentados

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}

        #endregion

    }
}
