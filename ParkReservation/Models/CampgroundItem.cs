using System;
using System.Collections.Generic;
using System.Text;

namespace ParkReservation.Models
{
    public class CampgroundItem : BaseItem
    {
        public int ParkId { get; set; }
        public string Name { get; set; }
        public int OpenFromMonth { get; set; }
        public int OpenToMonth { get; set; }
        public decimal DailyFee { get; set; }
    }
}
