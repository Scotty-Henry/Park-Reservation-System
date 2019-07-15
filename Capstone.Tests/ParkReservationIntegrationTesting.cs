using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkReservation.DAO;
using ParkReservation.Models;
using Security.BusinessLogic;
using Security.DAO;
using Security.Models.Database;
using System;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class ParkReservationIntegrationTesting 
    {
        private const int TEST_SITE_ID = 50;
        private TransactionScope _tran = null;
        private int _reservation_id = 0;
        private int _user_id = 0;
        private IParkDAO _db = null;
        private IUserSecurityDAO db = null;
        string fromDateString = "Wed Jul 22, 2019";
        string toDateString = "Wed Jul 27, 2019";
        private string Password = "a";


        [TestInitialize]
        public void InitializeReservation()
        {
            _db = new ParkDAO("Data Source=localhost\\sqlexpress;Initial Catalog=npcampground;Integrated Security=True");
            _tran = new TransactionScope();

            ReservationItem item = new ReservationItem()
            {
                SiteId = TEST_SITE_ID,
                Name = "Henry Family Vacation",
                FromDate = DateTime.Parse(fromDateString),
                ToDate = DateTime.Parse(toDateString),
                CreateDate = DateTime.Now
            };
            _reservation_id = _db.AddReservationItem(item);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _tran.Dispose();
            _reservation_id = 0;
            _user_id = 0;
        }


        [TestInitialize]
        public void TestUser()
        {
            db = new UserSecurityDAO("Data Source=localhost\\sqlexpress;Initial Catalog=npcampground;Integrated Security=True");
            _tran = new TransactionScope();

            Authentication auth = new Authentication(Password);

            UserItem useritem = new UserItem()
            {
                FirstName = "Scott",
                LastName = "Henry",
                Username = "ash",
                Email = "scott@aol.com",
                Salt = auth.Salt,
                Hash = auth.Hash,
                RoleId = (int)Security.BusinessLogic.Authorization.eRole.StandardUser
            };
            _user_id = db.AddUserItem(useritem);

            var user = db.GetUserItem(_user_id);

            Assert.AreEqual("Scott", user.FirstName);
            Assert.AreEqual("Henry", user.LastName);
            Assert.AreEqual("ash", user.Username);
            Assert.AreEqual("scott@aol.com", user.Email);

        }
    }
}
