using System;
using System.Collections.Generic;
using System.Text;

namespace ParkReservation.Models
{
    public class ParkItem : BaseItem
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime EstablishedDate { get; set; }
        public int Area { get; set; }
        public int Visitors { get; set; }
        public string Description { get; set; }
    }
}
