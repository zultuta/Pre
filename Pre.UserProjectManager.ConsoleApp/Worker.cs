using Newtonsoft.Json;
using Pre.UserProjectManager.Core.DTO;
using Pre.UserProjectManager.Core.Interfaces.Core;
using Pre.UserProjectManager.Core.Models.Filters;
using System.Globalization;

namespace Pre.UserProjectManager.ConsoleApp
{
    public class Worker
    {
        private long userId = 0;
        public async Task LoginAsync(IAuthenticationService _authService)
        {
            bool loggedIn = false;
            while (!loggedIn)
            {
                Console.Write("\n\nUserName: ");
                string userName = Console.ReadLine();

                Console.Write("Password: ");
                string password = Console.ReadLine();

                Console.WriteLine("Loggin in...");
                var loginRequest = new LoginRequest
                {
                    UserName = userName,
                    Password = password
                };
                var authResponse = await _authService.AuthenticateUserAsync(loginRequest);
                loggedIn = authResponse.Succeeded;
                userId = authResponse.Succeeded ? authResponse.Data.UserId : 0;
                if (!loggedIn)
                    Console.WriteLine("\n" + authResponse.Message);
            }
        }

        public async Task GetAssignedProjectsAsync(IProjectManagementService _projectService)
        {
            Console.WriteLine("\n\n\nYou have logged in successfully. Below is what you can do on this console.");
            Console.WriteLine("\n  1. Get All Projects assigned to you grouped by owned projects");
            Console.WriteLine("       you can filter by assignedDate. enter values below. Press enter without typing value to signify no filter.");

            Console.WriteLine("\n       Press enter to proceed");
            Console.ReadLine();

            Console.Write("Enter minAssigDate in foramt(MM-DD-YYYY): ");
            string filter = Console.ReadLine(); DateTime minAssignedDate = new DateTime();

            while (!string.IsNullOrEmpty(filter) && 
                !DateTime.TryParseExact(filter, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out minAssignedDate))
            {
                Console.WriteLine("  \nPlease enter a valid date");
                Console.Write("\nEnter minAssigDate in foramt(MM-DD-YYYY): ");
                filter = Console.ReadLine();
            }

            Console.Write("Enter maxAssigDate in foramt(MM-DD-YYYY): ");
            filter = Console.ReadLine(); DateTime maxAssignedDate = DateTime.Now;

            while (!string.IsNullOrEmpty(filter) && 
                !DateTime.TryParseExact(filter, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out maxAssignedDate))
            {
                Console.WriteLine("  \nPlease enter a valid date");
                Console.Write("\nEnter maxAssignedDate in foramt(MM-DD-YYYY): ");
                filter = Console.ReadLine();
            }
            var response = await _projectService.GetAssignedProjectsAsync(
                new ProjectFilter { maxDateAssigned = maxAssignedDate, minDateAssigned = minAssignedDate }, userId);

            if (response.Succeeded)
            {
                Console.WriteLine("\n" + JsonConvert.SerializeObject(response.Data, Formatting.Indented));
            }

            Console.WriteLine("\n" + response.Message);
        }

     }
}
