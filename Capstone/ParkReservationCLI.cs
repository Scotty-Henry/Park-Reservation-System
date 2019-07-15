using ParkReservation.DAO;
using ParkReservation.Models;
using Security.BusinessLogic;
using Security.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone
{
    public class ParkReservationCLI
    {

        private const string Option_Login = "1";
        private const string Option_Register = "2";
        private const string Option_Logout = "1";
        private const string Option_Quit = "q";

        private UserManager _userMgr = null;
        private IParkDAO _db = null;
        private ParkItem _selectedPark = null;
        private int _numDaysForSearchWindow = 30;

        /// <summary>
        /// CLI obj constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="db"></param>
        public ParkReservationCLI(UserManager userManager, IParkDAO db)
        {
            _userMgr = userManager;
            _db = db;
        }

        /// <summary>
        /// Start program at main menu
        /// </summary>
        public void Run()
        {
            MainMenu();
        }

        /// <summary>
        /// Enable user login
        /// </summary>
        private void MainMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                if (_userMgr.IsAuthenticated)
                {
                    //Console.WriteLine(" (1) Logout");
                    ParkMainMenu();
                    _userMgr.LogoutUser();
                    continue;
                }
                else
                {
                    Console.WriteLine(" (1) Login");
                    Console.WriteLine(" (2) Register");
                }
                Console.WriteLine(" (Q) Quit");
                Console.Write(" Please make a choice: ");

                string choice = Console.ReadLine().ToLower();

                Console.WriteLine();

                if (_userMgr.IsAuthenticated)
                {
                    if (choice == Option_Logout)
                    {
                        _userMgr.LogoutUser();
                    }
                    else if (choice == Option_Quit)
                    {
                        exit = true;
                    }
                    else
                    {
                        DisplayInvalidOption();
                        Console.ReadKey();
                    }
                }
                else
                {
                    if (choice == Option_Login)
                    {
                        DisplayLogin();
                    }
                    else if (choice == Option_Register)
                    {
                        DisplayRegister();
                    }
                    else if (choice == Option_Quit)
                    {
                        exit = true;
                    }
                    else
                    {
                        DisplayInvalidOption();
                        Console.ReadKey();
                    }
                }
            }
        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        private void DisplayLogin()
        {
            Console.Clear();
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            try
            {
                _userMgr.LoginUser(username, password);
                Console.Clear();
                Console.WriteLine($"Welcome {_userMgr.User.FirstName} {_userMgr.User.LastName}");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Register new user
        /// </summary>
        private void DisplayRegister()
        {
            Console.Clear();

            User user = new User();
            Console.WriteLine("Enter username: ");
            user.Username = Console.ReadLine();
            Console.WriteLine("Enter password: ");
            user.Password = Console.ReadLine();
            Console.WriteLine("Enter password again: ");
            user.ConfirmPassword = Console.ReadLine();
            Console.WriteLine("Enter email: ");
            user.Email = Console.ReadLine();
            Console.WriteLine("Enter first name: ");
            user.FirstName = Console.ReadLine();
            Console.WriteLine("Enter last name: ");
            user.LastName = Console.ReadLine();

            try
            {
                _userMgr.RegisterUser(user);
                Console.Clear();
                Console.WriteLine($"Welcome {_userMgr.User.FirstName} {_userMgr.User.LastName}");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Display invalid option window
        /// </summary>
        private void DisplayInvalidOption()
        {
            Console.WriteLine(" The option you selected is not a valid option.");
            Console.WriteLine();
        }

        /// <summary>
        /// Load Park main menu
        /// </summary>
        private void ParkMainMenu ()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                List<ParkItem> parks = _db.GetParkItems();

                Console.WriteLine("Welcome to the 100% Official National Park Registration Manager.");
                Console.WriteLine();

                try
                {
                    string [] asciiArt = Properties.Resources.ascii_mountain.Split("\n");
                    foreach (var line in asciiArt)
                    {
                        Console.WriteLine(line);
                    }
                    Console.WriteLine();
                }
                catch (Exception)
                {
                }

                int count = 1;
                foreach (var park in parks)
                {
                    Console.WriteLine((count + ") ").ToString().PadLeft(7) + park.Name);
                    count++;
                }
                // View My Reservations
                Console.WriteLine("R)".PadLeft(6) + " View all my reservations");
                Console.WriteLine("Q)".PadLeft(6) + " Quit");
                Console.WriteLine();

                string input = CLIHelper.GetString("Please select a park: ");
                if (input.Equals("Q") || input.Equals("q"))
                {
                    exit = true;
                }

                else if (input.Equals("R") || input.Equals("r"))
                {
                    ShowAllUserReservations();
                }

                else
                {
                    if (int.TryParse(input, out int selection))
                    {
                        if (selection > 0 || selection <= parks.Count)
                        {
                            _selectedPark = parks[selection - 1];
                            ParkInfoMenu();
                        }
                    }
                    else
                    {
                        DisplayInvalidOption();
                    }
                }
            }

        }

        /// <summary>
        /// Displays information about each park
        /// </summary>
        private void ParkInfoMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Park Information\n");
                Console.WriteLine(_selectedPark.Name + "\n");
                Console.WriteLine("Location:".PadRight(17) + _selectedPark.Location);
                Console.WriteLine("Established:".PadRight(17) + _selectedPark.EstablishedDate);
                Console.WriteLine("Area:".PadRight(17) + _selectedPark.Area);
                Console.WriteLine("Annual Visitors:".PadRight(17) + _selectedPark.Visitors);
                Console.WriteLine();
                Console.WriteLine("\n" + _selectedPark.Description + "\n");

                Console.WriteLine("Select a command");
                Console.WriteLine();
                Console.WriteLine("1)".PadLeft(6) + " View Campgrounds");
                Console.WriteLine("2)".PadLeft(6) + " Search for Reservation");
                Console.WriteLine("3)".PadLeft(6) + " See Booked Reservations (next 30 days) for this park"); // Bonus
                Console.WriteLine("4)".PadLeft(6) + " Return to Previous Screen");

                int selection = CLIHelper.GetInt("");

                switch (selection)
                {
                    case 1:
                        CampgroundMenu();
                        break;
                    case 2:
                        DisplayEntireParkAvailability();
                        break;
                    case 3:
                        Display30DayReservationsByPark();
                        break;
                    case 4:
                        exit = true;
                        break;
                    default:
                        DisplayInvalidOption();
                        break;
                }

            }
        }

        /// <summary>
        /// Campground Menu page
        /// </summary>
        private void CampgroundMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Park Campgrounds");
                Console.WriteLine(_selectedPark.Name);
                Console.WriteLine();

                List<CampgroundItem> campgrounds = _db.GetCampgroundItemsByPark(_selectedPark.Id);

                DisplayCampgrounds(campgrounds);

                Console.WriteLine();

                Console.WriteLine("Select a command");
                Console.WriteLine();
                Console.WriteLine("1)".PadLeft(6) + " Search for Available Reservation");
                Console.WriteLine("2)".PadLeft(6) + " Return to Previous Screen");

                int selection = CLIHelper.GetInt("Selection: ");
                switch (selection)
                {
                    case 1:
                        SearchForCampgroundReservation(campgrounds);
                        break;
                    case 2:
                        exit = true;
                        break;
                    default:
                        DisplayInvalidOption();
                        break;
                }
            }
        }

        /// <summary>
        /// Displays information about each campground
        /// </summary>
        /// <param name="campgrounds"></param>
        private void DisplayCampgrounds(List<CampgroundItem> campgrounds)
        {
            Console.Clear();
            Console.WriteLine(" ".PadRight(6) + "Name".PadRight(40) + "Open".PadRight(15) + "Close".PadRight(15) + "Daily Fee");

            int count = 1;

            foreach (var campground in campgrounds)
            {
                Console.WriteLine("#"+count.ToString().PadRight(5) + campground.Name.PadRight(40) + CLIHelper.GetMonth(campground.OpenFromMonth).PadRight(15) +
                    CLIHelper.GetMonth(campground.OpenToMonth).PadRight(15) + string.Format("{0:C}", campground.DailyFee));
                count++;
            }
        }

        /// <summary>
        /// Displays available campgrounds
        /// </summary>
        /// <param name="campgrounds"></param>
        private void SearchForCampgroundReservation(List<CampgroundItem> campgrounds)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Campgrounds: ");
                Console.WriteLine();
                DisplayCampgrounds(campgrounds);
                Console.WriteLine();

                int campgroundSelection = CLIHelper.GetInt("Which campground (enter 0 to cancel)? ");
                if (campgroundSelection == 0)
                {
                    exit = true;
                }
                else if (campgroundSelection > 0 || campgroundSelection <= campgrounds.Count)
                {
                    
                    int campgroundIdValue = campgrounds[campgroundSelection - 1].Id;
                    (DateTime arrival, DateTime departure) = CLIHelper.GetValidReservationWindow();
                    DisplayAvailableSites(campgroundIdValue, arrival, departure);
                }
                else
                {
                    DisplayInvalidOption();
                }
            }
        }

        /// <summary>
        /// Displays available sites
        /// </summary>
        /// <param name="campgroundId"></param>
        /// <param name="arrival"></param>
        /// <param name="departure"></param>
        private void DisplayAvailableSites(int campgroundId, DateTime arrival, DateTime departure)
        {
            Console.Clear();
            List<SiteItem> availSites = _db.GetAvailableSiteItems(campgroundId, arrival, departure);

            if (availSites.Count == 0)
            {
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("No available sites for those dates.");
                    string response = CLIHelper.GetString("Would you like to enter an alternate date range? (Y / N) ");
                    if (response.Equals("Y") || response.Equals("y"))
                    {
                        (DateTime arrivalDate, DateTime departureDate) = CLIHelper.GetValidReservationWindow();
                        DisplayAvailableSites(campgroundId, arrivalDate, departureDate);
                    }
                    else if (response.Equals("N") || response.Equals("n"))
                    {
                        exit = true;
                    }
                    else
                    {
                        DisplayInvalidOption();
                    }
                }
            }
            else
            {
                Console.WriteLine("Results Matching Your Search Criteria");

                DisplaySiteDetail(campgroundId, availSites);

                int siteSelection = SelectSite(availSites);

                if (siteSelection != 0)
                {
                    string reservationName = CLIHelper.GetString("What name should the reservation be made under? ");

                    ReservationItem reservation = new ReservationItem();
                    reservation.SiteId = siteSelection;
                    reservation.Name = reservationName;
                    reservation.FromDate = arrival;
                    reservation.ToDate = departure;
                    reservation.CreateDate = DateTime.Now;

                    int userId = _userMgr.User.Id;

                    int reservationId = -1;

                    try
                    {
                        reservationId = _db.MakeReservation(reservation, userId);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("There was an error in your request, returning to previous screen.");
                        DisplayAvailableSites(campgroundId, arrival, departure);
                    }
                    SuccessFulReservation(reservationName, reservationId);
                }   
                ParkMainMenu();
            }
        }

        /// <summary>
        /// Provides detailed information about each site
        /// </summary>
        /// <param name="campgroundId"></param>
        /// <param name="availSites"></param>
        private void DisplaySiteDetail(int campgroundId, List<SiteItem> availSites)
        {
            Console.WriteLine("Site No.".PadRight(12) + "Max Occup.".PadRight(12) + "Accessible".PadRight(12) +
                                  "Max RV Length".PadRight(18) + "Utilities".PadRight(12) + "Cost");

            int count = 1;
            foreach (var site in availSites)
            {
                Console.WriteLine(site.SiteNumber.ToString().PadRight(12) +
                    site.MaxOccupancy.ToString().PadRight(12) +
                    site.IsAccessable.ToString().PadRight(12) +
                    site.MaxRVLength.ToString().PadRight(18) +
                    site.HasUtilities.ToString().PadRight(12) +
                    string.Format("{0:C}", _db.GetCampgroundItem(campgroundId).DailyFee));
                count++;
                if (count >= 5)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Splash screen for successful reservation
        /// </summary>
        /// <param name="reservationName"></param>
        /// <param name="reservationId"></param>
        private void SuccessFulReservation(string reservationName, int reservationId)
        {
            Console.Clear();
            Console.WriteLine("The reservation has been made for " + reservationName +
                " and the confirmation id is " + reservationId);
            Console.WriteLine("Press any key to continue.");
            Console.ReadLine();
        }

        /// <summary>
        /// Displays all of current user's reservations
        /// </summary>
        private void ShowAllUserReservations()
        {
            Console.Clear();
            List<ReservationItem> userReservations = _db.GetReservationItems(_userMgr.User.Id);
            Console.WriteLine("Campground reservations for " + _userMgr.User.FirstName + " " + _userMgr.User.LastName);
            Console.WriteLine();

            DisplayReservations(userReservations);
        }

        /// <summary>
        /// Helper method to display reservations
        /// </summary>
        /// <param name="reservations"></param>
        private void DisplayReservations(List<ReservationItem> reservations)
        {
            Console.WriteLine("Reservation ID".PadRight(20) + "Site Id".PadRight(10) + "Name".PadRight(30) + "From Date".PadRight(13) + "To Date".PadRight(13));

            foreach (var reservation in reservations)
            {
                Console.WriteLine(reservation.Id.ToString().PadRight(20) +
                    reservation.SiteId.ToString().PadRight(10) +
                    reservation.Name.PadRight(30) +
                    reservation.FromDate.ToShortDateString().PadRight(13) +
                    reservation.ToDate.ToShortDateString().PadRight(13));
            }
            Console.WriteLine();
            Console.Write("Press any key to continue.");
            Console.ReadLine();
        }

        /// <summary>
        /// Display 30-day reservations for entire park
        /// </summary>
        private void Display30DayReservationsByPark()
        {
            Console.Clear();
            List<ReservationItem> reservations = _db.GetReservationsInDayWindow(DateTime.Now, _selectedPark.Id, _numDaysForSearchWindow);
            Console.WriteLine("Here are the reservations for " + _selectedPark.Name + " National Park starting in the next "+ _numDaysForSearchWindow+ " days.");
            Console.WriteLine();
            DisplayReservations(reservations);
        }

        /// <summary>
        /// Displays park / campground / site availability
        /// </summary>
        private void DisplayEntireParkAvailability()
        {
            List<CampgroundItem> campgrounds = _db.GetCampgroundItemsByPark(_selectedPark.Id);

            (DateTime FromDate, DateTime ToDate) = CLIHelper.GetValidReservationWindow();

            Console.Clear();

            foreach (var campground in campgrounds)
            {
                Console.WriteLine(campground.Id + ") " + campground.Name);
                Console.WriteLine();
                DisplaySiteDetail(campground.Id, _db.GetAvailableSiteItems(campground.Id, FromDate, ToDate));
                Console.WriteLine("-------------------------------------------------------------------------");
            }

            // make a campground selection
            
            int selectedCampground = SelectCampground(campgrounds);
            if (selectedCampground != 0)
            {
                List<SiteItem> selectedSites = _db.GetAvailableSiteItems(selectedCampground, FromDate, ToDate);
                int siteSelection = SelectSite(selectedSites);
                if (siteSelection != 0)
                {
                    if (siteSelection != 0)
                    {
                        string reservationName = CLIHelper.GetString("What name should the reservation be made under? ");

                        ReservationItem reservation = new ReservationItem();
                        reservation.SiteId = siteSelection;
                        reservation.Name = reservationName;
                        reservation.FromDate = FromDate;
                        reservation.ToDate = ToDate;
                        reservation.CreateDate = DateTime.Now;

                        int userId = _userMgr.User.Id;

                        int reservationId = -1;

                        try
                        {
                            reservationId = _db.MakeReservation(reservation, userId);
                            SuccessFulReservation(reservationName, reservationId);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("There was an error in your request, returning to previous screen.");
                        }
                        
                    }
                }
            }


        }

        /// <summary>
        /// Reserve campground
        /// </summary>
        /// <param name="availCampgrounds"></param>
        /// <returns></returns>
        private int SelectCampground(List<CampgroundItem> availCampgrounds)
        {
            bool isAvailSite = false;
            int cgSelection = 0;

            while (!isAvailSite)
            {
                cgSelection = CLIHelper.GetInt("Which campground should be reserved (enter 0 to cancel) ");
                if (cgSelection == 0)
                {
                    isAvailSite = true;
                }

                foreach (var campground in availCampgrounds)
                {
                    if (cgSelection.Equals(campground.Id))
                    {
                        isAvailSite = true;
                        break;
                    }
                }
            }
            return cgSelection;
        }

        /// <summary>
        /// Reserve site
        /// </summary>
        /// <param name="availSites"></param>
        /// <returns></returns>
        private int SelectSite(List<SiteItem> availSites)
        {
            bool isAvailSite = false;
            int siteSelection = 0;

            while (!isAvailSite)
            {
                siteSelection = CLIHelper.GetInt("Which site should be reserved (enter 0 to cancel) ");
                if (siteSelection == 0)
                {
                    isAvailSite = true;
                }

                foreach (var site in availSites)
                {
                    if (siteSelection.Equals(site.SiteNumber))
                    {
                        siteSelection = site.Id;
                        isAvailSite = true;
                        break;
                    }
                }
            }
            return siteSelection;
        }
    }
}
