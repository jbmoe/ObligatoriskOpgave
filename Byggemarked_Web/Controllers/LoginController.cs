using ByggemarkedLibrary.Model;
using ModelController = ByggemarkedLibrary.Controllers;
using System.Web.Mvc;

namespace Byggemarked_Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ModelController.Controller controller = ModelController.Controller.GetInstance();

        public ActionResult Index(Customer customer)
        {
            return View(customer);
        }

        public ActionResult Login(FormCollection values)
        {
            string email = values["Email"];
            string password = values["Password"];
            Customer customer = controller.Login(email, password);
            if (customer != null)
            {
                Session["CustomerID"] = customer.CustomerId.ToString();
                Session["CustomerName"] = customer.Name.ToString();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Logout()
        {
            if (Session["CustomerID"] != null)
            {
                Session.Abandon();
            }
            return RedirectToAction("Index");
        }
    }
}