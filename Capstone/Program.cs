using Microsoft.Extensions.Configuration;
using ParkReservation.DAO;
using Security.BusinessLogic;
using Security.DAO;
using System;
using System.IO;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("Project");

            IParkDAO db = new ParkDAO(connectionString);
            IUserSecurityDAO security = new UserSecurityDAO(connectionString);
            UserManager userMgr = new UserManager(security);
            ParkReservationCLI cli = new ParkReservationCLI(userMgr, db);
            cli.Run();
        }
    }
}
