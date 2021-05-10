using NUnit.Framework;
using ByggemarkedLibrary.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByggemarkedLibrary.Model;

namespace ByggemarkedLibrary.Controllers.Tests
{
    [TestFixture()]
    public class ControllerTests
    {
        private Controller controller = Controller.GetInstance();

        [TestCase(100, 50, 5, 400)]
        [TestCase(80, 40, 0, 120)]
        [TestCase(200, 75, 99, 7700)]
        public void CalculatePriceTest(double deposit, double pricePrDay, int daysToAdd, double expectedPrice)
        {
            Tool tool = new Tool()
            {
                Depositum = deposit,
                PricePrDay = pricePrDay
            };

            DateTime start = DateTime.Now;
            DateTime end = start.AddDays(daysToAdd);

            double price = controller.CalculatePrice(tool, start, end);

            Assert.AreEqual(expectedPrice, price);
        }

        [TestCase(100, 50, -5)]
        [TestCase(80, 40, -1)]
        [TestCase(200, 75, -99)]
        public void CalculatePriceTestThrows(double deposit, double pricePrDay, int daysToAdd)
        {
            Tool tool = new Tool()
            {
                Depositum = deposit,
                PricePrDay = pricePrDay
            };

            DateTime start = DateTime.Now;
            DateTime end = start.AddDays(daysToAdd);

            Assert.That(() => controller.CalculatePrice(tool, start, end), Throws.TypeOf<ArgumentException>());
        }
    }
}