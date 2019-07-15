using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public static class CLIHelper
    {
        /// <summary>
        /// Get string from user
        /// </summary>
        /// <param name="message"></param>
        /// <returns>string value</returns>
        public static string GetString(string message)
        {
            Console.Write(message);
            string value = Console.ReadLine();
            return value;
        }

        /// <summary>
        /// Repeat until user enters a valid int
        /// </summary>
        /// <param name="message"></param>
        /// <returns>int value</returns>
        public static int GetInt(string message)
        {
            Console.Write(message);
            int value = 0;
            string input = Console.ReadLine();

            while (!int.TryParse(input, out value))
            {
                Console.Write(message);
                input = Console.ReadLine();
            }
            return value;
        }

        /// <summary>
        /// Repeat until user enters a valid DateTime
        /// </summary>
        /// <param name="message"></param>
        /// <returns>DateTime value</returns>
        public static DateTime GetDate(string message)
        {
            Console.Write(message);
            DateTime value = new DateTime();
            while(!DateTime.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine(message);
            }
            return value;
        }

        /// <summary>
        /// Turns Month entered numerically into a string value
        /// </summary>
        /// <param name="month"></param>
        /// <returns>String month</returns>
        public static string GetMonth(int month)
        {
            Dictionary<int, string> months = new Dictionary<int, string>
            {
                [1] = "January",
                [2] = "February",
                [3] = "March",
                [4] = "April",
                [5] = "May",
                [6] = "June",
                [7] = "July",
                [8] = "August",
                [9] = "September",
                [10] = "October",
                [11] = "November",
                [12] = "December"
            };
            return months.GetValueOrDefault(month);
         
        }

        /// <summary>
        /// Gets a valid reservation window
        /// </summary>
        /// <returns>Tuple (DateTime start, Datetime finish)</returns>
        public static Tuple<DateTime, DateTime> GetValidReservationWindow()
        {


            DateTime ArrivalDate = GetDate("What is the arrival date? YYYY/MM/DD: ");

            while (ArrivalDate <= DateTime.Now)
            {
                Console.WriteLine("Cannot make a reservation before today.");
                ArrivalDate = GetDate("What is the arrival date? YYYY/MM/DD: ");
            }

            DateTime DepartureDate = GetDate("What is the departure date? YYYY/MM/DD: ");

            while (DepartureDate < ArrivalDate)
            {
                Console.WriteLine("Cannot schedule an end date before your arrival date.");
                DepartureDate = GetDate("What is the departure date? YYYY/MM/DD: ");
            }

            return new Tuple<DateTime, DateTime>(ArrivalDate, DepartureDate);
        }
    }
}
