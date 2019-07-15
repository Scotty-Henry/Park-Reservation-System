using System;
using System.Collections.Generic;
using System.Text;
using ParkReservation.Models;
using System.Data.SqlClient;
using System.Transactions;

namespace ParkReservation.DAO
{
    public class ParkDAO : IParkDAO
    {
        #region Variables

        private const string _getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";
        private string _connectionString;

        #endregion

        #region Constructors

        public ParkDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region CampgroundItems


        public CampgroundItem GetCampgroundItem(int Id)
        {
            CampgroundItem item = null;

            const string sql = "Select * From [campground] Where campground_id = @Id;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", Id);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    item = GetCampgroundItemFromReader(reader);
                }
            }
            return item;
        }

        public List<CampgroundItem> GetCampgroundItemsByPark(int ParkId)
        {
            List<CampgroundItem> campgrounds = new List<CampgroundItem>();
            const string sql = "Select * From [campground] Where park_id = @ParkId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ParkId", ParkId);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    campgrounds.Add(GetCampgroundItemFromReader(reader));
                }
            }
            return campgrounds;
        }

        private CampgroundItem GetCampgroundItemFromReader(SqlDataReader reader)
        {
            CampgroundItem item = new CampgroundItem();

            item.Id = Convert.ToInt32(reader["campground_id"]);
            item.Name = Convert.ToString(reader["name"]);
            item.ParkId = Convert.ToInt32(reader["park_id"]);
            item.OpenFromMonth = Convert.ToInt16(reader["open_from_mm"]);
            item.OpenToMonth = Convert.ToInt16(reader["open_to_mm"]);
            item.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

            return item;
        }

        #endregion

        #region ParkItems

        public ParkItem GetParkItem(int Id)
        {
            throw new NotImplementedException();
        }

        public List<ParkItem> GetParkItems()
        {
            List<ParkItem> parks = new List<ParkItem>();
            const string sql = "Select * From [park] Order By name;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    parks.Add(GetParkItemFromReader(reader));
                }
            }

            return parks;
        }

        private ParkItem GetParkItemFromReader(SqlDataReader reader)
        {
            ParkItem item = new ParkItem();

            item.Id = Convert.ToInt32(reader["park_id"]);
            item.Name = Convert.ToString(reader["name"]);
            item.Location = Convert.ToString(reader["Location"]);
            item.EstablishedDate = Convert.ToDateTime(reader["establish_date"]);
            item.Area = Convert.ToInt32(reader["area"]);
            item.Visitors = Convert.ToInt32(reader["visitors"]);
            item.Description = Convert.ToString(reader["description"]);

            return item;
        }

        #endregion

        #region SiteItems
        public List<SiteItem> GetAvailableSiteItems(int campgroundId, DateTime arrival, DateTime departure)
        {
            List<SiteItem> availSites = new List<SiteItem>();
            const string sql = "SELECT * FROM site " +
                                "WHERE campground_id = @campgroundId " +
                                "AND site_id NOT IN " +
                                "(Select site.site_id " +
                                "FROM site " +
                                "Join reservation On reservation.site_id = site.site_id " +
                                "Where (from_date <= @arrival AND to_date >= @arrival) " +
                                "OR(from_date < @departure AND to_date >= @departure) " +
                                "OR(@arrival <= from_date AND @departure >= from_date));";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                
                cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                cmd.Parameters.AddWithValue("@arrival", arrival);
                cmd.Parameters.AddWithValue("@departure", departure);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    availSites.Add(GetSiteItemFromReader(reader));
                }
            }

            return availSites;
        }

        public SiteItem GetSiteItemFromReader(SqlDataReader reader)
        {
            SiteItem item = new SiteItem();

            item.Id = Convert.ToInt32(reader["site_id"]);
            item.CampgroundId = Convert.ToInt32(reader["campground_id"]);
            item.SiteNumber = Convert.ToInt32(reader["site_number"]);
            item.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            item.IsAccessable = Convert.ToBoolean(reader["accessible"]);
            item.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
            item.HasUtilities = Convert.ToBoolean(reader["utilities"]);

            return item;
        }

        #endregion

        #region ReservationItems

        public int MakeReservation(ReservationItem item, int userId)
        {
            int reservationId = BaseItem.InvalidId;

            using (TransactionScope scope = new TransactionScope())
            {
                reservationId = AddReservationItem(item);
                AddUserReservationItem(reservationId, userId);
                scope.Complete();
            }

            return reservationId;
        }

        public int AddReservationItem(ReservationItem reservation)
        {
            const string sql = "INSERT [reservation] (site_id, name, from_date, to_date, create_date) " +
                               "VALUES (@SiteId, @Name, @FromDate, @ToDate, @CreateDate);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@SiteId", reservation.SiteId);
                cmd.Parameters.AddWithValue("@Name", reservation.Name);
                cmd.Parameters.AddWithValue("@FromDate", reservation.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", reservation.ToDate);
                cmd.Parameters.AddWithValue("@CreateDate", reservation.CreateDate);

                reservation.Id = (int)cmd.ExecuteScalar();
            }

            return reservation.Id;
        }

        public List<ReservationItem> GetReservationItems(int userId)
        {
            List<ReservationItem> reservations = new List<ReservationItem>();
            const string sql = "Select * " +
                                "from reservation " +
                                "join UserReservation ON reservation.reservation_id = UserReservation.reservation_id " +
                                "where [user_id] = @userId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    reservations.Add(GetReservationItemFromReader(reader));
                }
            }
            return reservations;
        }

        public List<ReservationItem> GetReservationsInDayWindow(DateTime fromDate, int parkId, int days)
        {
            List<ReservationItem> reservations = new List<ReservationItem>();
            const string sql = "Select * " +
                                "FROM reservation " +
                                "JOIN site ON reservation.site_id = site.site_id " +
                                "JOIN campground ON site.campground_id = campground.campground_id " +
                                "JOIN park ON park.park_id = campground.park_id " +
                                "where from_date BETWEEN @FromDate AND @ToDate AND park.park_id = @ParkId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", fromDate.AddDays(days));
                cmd.Parameters.AddWithValue("@ParkId", parkId);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    reservations.Add(GetReservationItemFromReader(reader));
                }
            }
            return reservations;
        }

        private ReservationItem GetReservationItemFromReader(SqlDataReader reader)
        {
            ReservationItem item = new ReservationItem();

            item.Id = Convert.ToInt32(reader["reservation_id"]);
            item.SiteId = Convert.ToInt32(reader["site_id"]);
            item.Name = Convert.ToString(reader["name"]);
            item.FromDate = Convert.ToDateTime(reader["from_date"]);
            item.ToDate = Convert.ToDateTime(reader["to_date"]);
            item.CreateDate = Convert.ToDateTime(reader["create_date"]);

            return item;
        }

        #endregion

        #region UserReservation

        public int AddUserReservationItem(int reservationId, int userId)
        {
            const string sql = "INSERT [UserReservation] (user_id, reservation_id) " +
                               "VALUES (@user_id, @reservation_id);";

            int userReservationId = BaseItem.InvalidId;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@reservation_id", reservationId);

                userReservationId = (int)cmd.ExecuteScalar();
            }

            return userReservationId;
        }



        #endregion
    }
}
