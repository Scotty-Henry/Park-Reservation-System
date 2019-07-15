using ParkReservation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkReservation.DAO
{
    public interface IParkDAO
    {
        #region CampgroundItem

        CampgroundItem GetCampgroundItem(int Id);
        List<CampgroundItem> GetCampgroundItemsByPark(int ParkId);

        #endregion

        #region ParkItem

        ParkItem GetParkItem(int Id);
        //ParkItem GetParkItem(string username);
        List<ParkItem> GetParkItems();

        #endregion

        #region Reservation

        List<ReservationItem> GetReservationsInDayWindow(DateTime fromDate, int parkId, int days);
        List<ReservationItem> GetReservationItems(int userId);
        int AddReservationItem(ReservationItem item);
        //ParkItem GetParkItem(string username);

        #endregion

        #region SiteItem

        //ParkItem GetParkItem(string username);
        //List<SiteItem> GetSiteItems();
        List<SiteItem> GetAvailableSiteItems(int campgroundId, DateTime arrival, DateTime departure);

        #endregion

        #region UserReservationItem

        int MakeReservation(ReservationItem item, int userId);
        int AddUserReservationItem(int reservationId, int userId);
        //ParkItem GetParkItem(string username);        

        #endregion


    }
}
