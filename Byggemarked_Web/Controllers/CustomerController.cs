using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ByggemarkedLibrary.Model;
using ModelController = ByggemarkedLibrary.Controllers;

namespace Byggemarked_Web.Controllers
{
    public class CustomerController : Controller
    {
        private ModelController.Controller controller = ModelController.Controller.GetInstance();

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Address,Email,Password")] Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);

            controller.CreateCustomer(customer);
            return RedirectToAction("Index", "Login", customer);


        }


        // GET: Customer/Details/5
        public ActionResult Details()
        {
            if (Session["CustomerID"] == null) return RedirectToAction("index", "login");

            int id = int.Parse(Session["CustomerID"].ToString());
            Customer customer = controller.FindCustomer((int)id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customer/Edit/
        public ActionResult Edit()
        {
            if (Session["CustomerID"] == null) return RedirectToAction("index", "login");

            int id = int.Parse(Session["CustomerID"].ToString());
            Customer customer = controller.FindCustomer((int)id);

            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,Name,Address,Email,Password")] Customer customer)
        {
            if (Session["CustomerID"] == null) return RedirectToAction("index", "login");
            if (ModelState.IsValid)
            {
                controller.UpdateCustomer(customer);
                return RedirectToAction("Details", new { id = customer.CustomerId });
            }
            return View(customer);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
