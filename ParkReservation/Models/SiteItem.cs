using System;
using System.Collections.Generic;
using System.Text;

namespace ParkReservation.Models
{
    public class SiteItem : BaseItem
    {
        public int CampgroundId { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public bool IsAccessable { get; set; }
        public int MaxRVLength { get; set; }
        public bool HasUtilities { get; set; }
    }
}
