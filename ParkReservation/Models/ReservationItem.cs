using System;
using System.Collections.Generic;
using System.Text;

namespace ParkReservation.Models
{
    public class ReservationItem : BaseItem
    {
        public int SiteId { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
