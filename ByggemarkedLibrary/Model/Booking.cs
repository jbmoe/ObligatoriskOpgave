using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByggemarkedLibrary.Model
{
    public class Booking
    {
        public int BookingId { get; set; }
        public State State { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public Customer Customer { get; set; }
        public Tool Tool { get; set; }
        public double Price { get; set; }
    }
}
