using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelController = ByggemarkedLibrary.Controllers;

namespace Byggemarked_Web.Controllers
{
    public class ToolsController : Controller
    {
        private readonly ModelController.Controller controller = ModelController.Controller.GetInstance();
        // GET: Tools
        public ActionResult Index()
        {
            var tools = controller.GetTools();
            return View(tools);
        }
    }
}