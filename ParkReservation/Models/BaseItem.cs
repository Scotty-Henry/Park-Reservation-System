using System;
using System.Collections.Generic;
using System.Text;

namespace ParkReservation.Models
{
    public class BaseItem
    {
        public const int InvalidId = -1;

        public int Id { get; set; } = InvalidId;
    }
}
