using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByggemarkedLibrary.Model
{
    public class Tool
    {
        public int ToolId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Depositum { get; set; }
        public double PricePrDay { get; set; }

        public override string ToString()
        {
            return $"{Name} - Depositum: {Depositum} kr - Døgnpris: {PricePrDay} kr";
        }
    }
}
