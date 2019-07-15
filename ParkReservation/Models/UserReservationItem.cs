using System;
using System.Collections.Generic;
using System.Text;

namespace ParkReservation.Models
{
    public class UserReservationItem : BaseItem
    {
        public int UserId { get; set; }
        public int ReservationId { get; set; }
    }
}
