using System;
using System.Net;
using System.Web.Mvc;
using ByggemarkedLibrary.Model;
using ModelController = ByggemarkedLibrary.Controllers;

namespace Byggemarked_Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly ModelController.Controller controller = ModelController.Controller.GetInstance();

        // GET: Booking
        public ActionResult Index()
        {
            if (Session["CustomerID"] != null)
            {
                int id = Convert.ToInt32(Session["CustomerID"]);
                var bookings = controller.FindBookingsForCustomer(id);
                return View(bookings);
            }
            return RedirectToAction("Index", "Login");
        }

        // GET: Booking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = controller.FindBooking((int)id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Booking/Create
        public ActionResult Create(int toolId, string toolName)
        {
            TempData["ToolId"] = toolId;
            ViewBag.ToolName = toolName;
            return View();
        }

        // POST: Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StartDate,EndDate")] Booking booking)
        {
            if (!ModelState.IsValid) return View(booking);

            int customerId = int.Parse(Session["CustomerID"].ToString());
            int toolId = int.Parse(TempData["ToolId"].ToString());

            controller.CreateBooking(customerId, toolId, booking.StartDate, booking.EndDate);

            return RedirectToAction("index");
        }
    }
}
