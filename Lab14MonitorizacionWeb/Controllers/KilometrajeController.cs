using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab14MonitorizacionWeb.Models;
using Microsoft.SharePoint.Client;

namespace Lab14MonitorizacionWeb.Controllers
{
    public class KilometrajeController : Controller
    {
        // GET: Kilometraje
        public PartialViewResult Index()
        {
            List<Kilometraje> claimsToDisplay= new List<Kilometraje>();

            var spContext = Session["sp"] as SharePointContext;

            using (var clientContext=spContext.CreateUserClientContextForSPAppWeb())
            {
                Web web = clientContext.Web;
                clientContext.Load(web);
                clientContext.ExecuteQuery();

                ListCollection lists = web.Lists;
                clientContext.Load<ListCollection>(lists);
                clientContext.ExecuteQuery();

                var kms = lists.GetByTitle("Kilometros");
                clientContext.Load(kms);
                clientContext.ExecuteQuery();

                CamlQuery query= new CamlQuery();
                ListItemCollection kmsItems = kms.GetItems(query);
                clientContext.Load(kmsItems);
                clientContext.ExecuteQuery();

                foreach (var km in kmsItems)
                {
                    Kilometraje currentClaim=new Kilometraje();
                    currentClaim.Destino = km["Destino"].ToString();

                    var kilometros = km["Distancia"];

                    //currentClaim.Kilometros = km["Distancia"] != null ? Convert.ToInt32(km["Distancia"]) : 0;

                    if (kilometros==null)
                    {
                        currentClaim.Kilometros = 0;
                    }

                    else
                    {
                        currentClaim.Kilometros = Convert.ToInt32(kilometros);
                    }


                    claimsToDisplay.Add(currentClaim);

                }


}


            return PartialView("Index", claimsToDisplay);
        }


        [HttpGet]
        public ActionResult Create()
        {
            Kilometraje newClaim= new Kilometraje();
            
                return View("Crear", newClaim);
        }
        [HttpPost]
        public ActionResult Create(Kilometraje claim)
        {
            if (!ModelState.IsValid)
            {
                return View("Crear", claim);
            }

            else
            {
                var spContext = Session["sp"] as SharePointContext;

                using (var context=spContext.CreateAppOnlyClientContextForSPAppWeb())
                {
                    List claimsList = context.Web.Lists.GetByTitle("Kilometros");
                    context.Load(claimsList);

                    ListItemCreationInformation creationInfo= new ListItemCreationInformation();
                    ListItem newClaim = claimsList.AddItem(creationInfo);
                    newClaim["Destino"] = claim.Destino;
                    newClaim["Distancia"] = Convert.ToInt32(claim.Kilometros);
                    newClaim.Update();

                    context.ExecuteQuery();


                }

                return RedirectToAction("Index", "Home");
            }
        }

    }
}