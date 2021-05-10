using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ByggemarkedLibrary.Model;

namespace ByggemarkedLibrary.Controllers
{
    public class Controller
    {
        #region Singleton
        private Controller() { }

        private static Controller _instance;
        public static Controller GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Controller();
            }
            return _instance;
        }
        #endregion

        public Booking CreateBooking(int customerId, int toolId, DateTime startDate, DateTime endDate)
        {
            Booking booking;
            using (var ctx = new ByggemarkedContext())
            {
                Customer customer = ctx.Customers.Find(customerId);
                Tool tool = ctx.Tools.Find(toolId);
                double price = CalculatePrice(tool, startDate, endDate);

                booking = new Booking()
                {
                    Customer = customer,
                    Tool = tool,
                    StartDate = startDate,
                    EndDate = endDate,
                    Price = price
                };

                ctx.Bookings.Add(booking);
                ctx.SaveChanges();
            }

            return booking;
        }

        public double CalculatePrice(Tool tool, DateTime startDate, DateTime endDate)
        {
            int days = (endDate.Date - startDate.Date).Days + 1;
            if (days < 1) throw new ArgumentException("Start date must be before end date");
            double price = tool.Depositum + (tool.PricePrDay * days);
            return price;
        }

        public Customer CreateCustomer(Customer customer)
        {
            using (var ctx = new ByggemarkedContext())
            {

                ctx.Customers.Add(customer);
                ctx.SaveChanges();
            }

            return customer;
        }

        public Customer UpdateCustomer(Customer customer)
        {
            using (var ctx = new ByggemarkedContext())
            {
                ctx.Entry(customer).State = EntityState.Modified;
                ctx.SaveChanges();
            }
            return customer;
        }

        public Customer Login(string email, string password)
        {
            Customer customer = null;
            using (var ctx = new ByggemarkedContext())
            {
                customer = ctx.Customers.Single(a => a.Email.Equals(email) && a.Password.Equals(password));
            }
            return customer;
        }

        public void SetStateForBooking(int bookingId, State state)
        {
            using (var ctx = new ByggemarkedContext())
            {
                Booking booking = ctx.Bookings.First(b => b.BookingId == bookingId);
                booking.State = state;
                ctx.SaveChanges();
            }
        }

        public void DeleteBooking(int bookingId)
        {
            using (var ctx = new ByggemarkedContext())
            {
                var toRemove = ctx.Bookings.Single(b => b.BookingId == bookingId);
                ctx.Bookings.Remove(toRemove);
                ctx.SaveChanges();
            }
        }

        public List<Tool> GetTools()
        {
            List<Tool> tools = null;
            using (var ctx = new ByggemarkedContext())
            {
                tools = ctx.Tools.ToList();
            }
            return tools;
        }

        public Booking FindBooking(int bookingId)
        {
            Booking booking;
            using (var ctx = new ByggemarkedContext())
            {
                booking = ctx.Bookings.Single(b => b.BookingId == bookingId);
                ctx.Entry(booking)
                   .Reference(b => b.Tool)
                   .Load();
            }
            return booking;
        }

        public Customer FindCustomer(int id)
        {
            Customer customer;
            using (var ctx = new ByggemarkedContext())
            {
                customer = ctx.Customers.Find(id);
            }
            return customer;
        }

        public List<Booking> FindBookingsForCustomer(string customerEmail)
        {
            List<Booking> bookings;
            using (var ctx = new ByggemarkedContext())
            {
                bookings = ctx.Bookings.Where(b => b.Customer.Email == customerEmail)
                                       .Include(b => b.Tool)
                                       .ToList();
            }
            return bookings;
        }

        public List<Booking> FindBookingsForCustomer(int customerId)
        {
            List<Booking> bookings;
            using (var ctx = new ByggemarkedContext())
            {
                bookings = ctx.Bookings.Where(booking => booking.Customer.CustomerId == customerId)
                                       .Include(booking => booking.Tool)
                                       .ToList();
            }

            return bookings;
        }

        public void InitializeDB()
        {
            using (var ctx = new ByggemarkedContext())
            {
                Tool t1 = new Tool() { ToolId = 1, Name = "Havetromler", Depositum = 100, PricePrDay = 50 };
                Tool t2 = new Tool() { ToolId = 2, Name = "Kompostkværn", Depositum = 80, PricePrDay = 40 };
                Tool t3 = new Tool() { ToolId = 3, Name = "Vinkelsliber", Depositum = 40, PricePrDay = 25 };
                Tool t4 = new Tool() { ToolId = 4, Name = "Gulvslibemaskine", Depositum = 200, PricePrDay = 75 };
                Tool t5 = new Tool() { ToolId = 5, Name = "Motorsav", Depositum = 180, PricePrDay = 60 };
                Tool t6 = new Tool() { ToolId = 6, Name = "Gravko", Depositum = 5000, PricePrDay = 1500 };
                Tool t7 = new Tool() { ToolId = 7, Name = "Minilæsser", Depositum = 2400, PricePrDay = 400 };

                IEnumerable<Tool> værktøjer = new List<Tool>() { t1, t2, t3, t4, t5, t6, t7 };
                ctx.Tools.AddRange(værktøjer);
                ctx.SaveChanges();

                Customer c1 = new Customer() { CustomerId = 1, Name = "Jeppe Møller", Address = "Byen 3", Email = "jeppe@email.dk", Password = "jeppe123" };
                Customer c2 = new Customer() { CustomerId = 2, Name = "Rasmus Hansen", Address = "Gaden 33", Email = "rasmus@email.dk", Password = "rasmus123" };
                Customer c3 = new Customer() { CustomerId = 3, Name = "Ulrik Küseler", Address = "Vejen 98", Email = "ulrik@email.dk", Password = "ulrik123" };
                Customer c4 = new Customer() { CustomerId = 4, Name = "Søren Pedersen", Address = "Stedet 8", Email = "søren@email.dk", Password = "søren123" };
                Customer c5 = new Customer() { CustomerId = 5, Name = "Lars Larsen", Address = "Bakken 49", Email = "lars@email.dk", Password = "lars123" };

                IEnumerable<Customer> kunder = new List<Customer>() { c1, c2, c3, c4, c5 };
                ctx.Customers.AddRange(kunder);
                ctx.SaveChanges();

                Booking b0 = CreateBooking(c1.CustomerId, t1.ToolId, new DateTime(2020, 4, 29), new DateTime(2020, 5, 13));
                Booking b1 = CreateBooking(c1.CustomerId, t2.ToolId, new DateTime(2020, 4, 29), new DateTime(2020, 5, 13));
                Booking b2 = CreateBooking(c2.CustomerId, t2.ToolId, new DateTime(2020, 4, 27), new DateTime(2020, 5, 12));
                Booking b3 = CreateBooking(c2.CustomerId, t4.ToolId, new DateTime(2020, 4, 27), new DateTime(2020, 5, 15));
                Booking b4 = CreateBooking(c3.CustomerId, t3.ToolId, new DateTime(2020, 4, 28), new DateTime(2020, 5, 14));
                Booking b5 = CreateBooking(c3.CustomerId, t5.ToolId, new DateTime(2020, 4, 28), new DateTime(2020, 5, 15));
                Booking b6 = CreateBooking(c4.CustomerId, t6.ToolId, new DateTime(2020, 4, 28), new DateTime(2020, 5, 12));
                Booking b7 = CreateBooking(c4.CustomerId, t2.ToolId, new DateTime(2020, 4, 28), new DateTime(2020, 5, 11));
                Booking b8 = CreateBooking(c5.CustomerId, t7.ToolId, new DateTime(2020, 4, 28), new DateTime(2020, 5, 21));
                Booking b9 = CreateBooking(c5.CustomerId, t6.ToolId, new DateTime(2020, 4, 28), new DateTime(2020, 5, 14));

                IEnumerable<Booking> bookinger = new List<Booking>() { b0, b1, b2, b3, b4, b5, b6, b7, b8, b9 };
                ctx.Bookings.AddRange(bookinger);
                ctx.SaveChanges();
            }
        }
    }

    class NoSuchElement : Exception
    {
        public NoSuchElement(string message) : base(message) { }
    }
}
