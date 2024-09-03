using BusinessLogic.DataBasesContext;
using Bogus;
using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;
using BusinessLogic.Enums;
using System.Drawing.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using static DevelopmentOnly.DatabaseSeeder;
using Microsoft.Identity.Client;

namespace DevelopmentOnly
{
    public class DatabaseSeeder(
        VacationManagerDbContext db,
        UserManager<User> userManager
        //ILogger<DatabaseSeeder> logger
        )
    {
        private readonly string settingsPath = "C:\\Users\\iwo\\Source\\Repos\\IwsonHD\\VacationManagementApp\\DevelopmentOnly\\devsettings.json";
        
        public async Task Seed(int employersAmount, (int, int) employeePerEmployerRange, (int, int) vacationPerEmployeeRange)
        {
            var defaultEmployees = new Dictionary<Employee, string>();
            var defaultEmployers = new Dictionary<Employer, string>();

            GenerateDefaultUsers(out defaultEmployers, out defaultEmployees);

            foreach (var employer in defaultEmployers)
            {
                if (await userManager.FindByEmailAsync(employer.Key.Email) == null)
                    await userManager.CreateAsync(employer.Key, employer.Value);
            }

            foreach (var employee in defaultEmployees)
            {
                if (await userManager.FindByEmailAsync(employee.Key.Email) == null)
                    await userManager.CreateAsync(employee.Key, employee.Value);
            }

            if (db.Employees.Count() >= 50) return;

            var employers = GenerateEmployers(employersAmount);

            foreach (var employer in employers)
            {
                await userManager.CreateAsync(employer,"123");
            }

            var employees = GenerateEmployees(employers, employeePerEmployerRange);

            foreach (var employee in employees)
            {
                await userManager.CreateAsync(employee,"123");
            }

            var employeeEmails = employees.Select(e => e.Email).ToList();
            var employeeIds = db.Employees
                .Where(e => employeeEmails.Contains(e.Email))
                .Select(e => e.Id)
                .ToList();

            var vacations = GenerateVacations(employeeIds, vacationPerEmployeeRange);

            await db.Vacations.AddRangeAsync(vacations);

            await db.SaveChangesAsync();
        }


        private static List<Employer> GenerateEmployers(int count)
        {
            var employerFaker = new Faker<Employer>()
                .RuleFor(e => e.UserName, f => f.Internet.Email())
                .RuleFor(e => e.Email, f => f.Internet.Email())
                .RuleFor(e => e.FirstName, f => f.Person.FirstName)
                .RuleFor(e => e.LastName, f => f.Person.LastName)
                .RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber("######"))
                .RuleFor(e => e.CompanyName, f => f.Company.CompanyName())
                .RuleFor(e => e.EmailConfirmed, f => true);
            

            return employerFaker.Generate(count);
        }

        private static List<Employee> GenerateEmployees(List<Employer> employers, (int,int) employeeRange)
        {
            List<Employee> employeeOut = new List<Employee>();
            var randomGenerator = new Random(); 


            foreach (var employer in employers)
            {
                var employeeFaker = new Faker<Employee>()
                .RuleFor(e => e.UserName, f => f.Internet.Email())
                .RuleFor(e => e.Email, f => f.Internet.Email())
                .RuleFor(e => e.FirstName, f => f.Person.FirstName)
                .RuleFor(e => e.LastName, f => f.Person.LastName)
                .RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber("########"))
                .RuleFor(e => e.EmployersEmail, f => employer.Email)
                .RuleFor(e => e.EmployeeConfirmed, f => true)
                .RuleFor(e => e.EmailConfirmed, f => true);
                int employeeAm = randomGenerator.Next(employeeRange.Item1, employeeRange.Item2);
                employeeOut.AddRange(employeeFaker.Generate(employeeAm));
            }

            return employeeOut;
        }

        private static List<Vacation> GenerateVacations(List<string> employeeIds, (int,int) vacationRange)
        {
            List<Vacation> vacationOut = new List<Vacation>();
            var randomGenerator = new Random();

            foreach (var employeeId in employeeIds)
            {
                var vacationFaker = new Faker<Vacation>()
                    .RuleFor(v => v.HowManyDays, f => f.Random.Int(2, 30)) // Start daty jest w przeszłości
                    .RuleFor(v => v.When, f => f.Date.Past(1).AddDays(f.Random.Int(1, 30))) // Wakacje trwają od 1 do 14 dni
                    .RuleFor(v => v.EmployeeId, f => employeeId) // Przypisanie wakacji do pracownika przez Id
                    .RuleFor(v => v.state, f => f.PickRandom<VacationState>());

                int vacationAm = randomGenerator.Next(vacationRange.Item1, vacationRange.Item2);
                vacationOut.AddRange(vacationFaker.Generate(vacationAm));
            }

            return vacationOut;
        }

        //User and coresponding password
        // User and corresponding password
        private void GenerateDefaultUsers(out Dictionary<Employer, string>? defaultEmployers, out Dictionary<Employee, string>? defaultEmployees)
        {
            string jsonString = File.ReadAllText(settingsPath);

            SeederSettingsContainer? seederSettings = null;
            defaultEmployers = new Dictionary<Employer, string>();
            defaultEmployees = new Dictionary<Employee, string>();

            try
            {
                seederSettings = JsonSerializer.Deserialize<SeederSettingsContainer>(jsonString);

                if (seederSettings?.SeederSettings == null)
                {
                    System.Console.WriteLine("SeederSettings or SeederSettingsContainer is null.");
                    return;
                }

                var defaultUsers = seederSettings.SeederSettings.DefaultUsers;
                if (defaultUsers == null)
                {
                    System.Console.WriteLine("DefaultUsers is null.");
                    return;
                }

                foreach (var user in defaultUsers.Employers)
                {
                    Employer employer = new Employer
                    {
                        Email = user.Email,
                        UserName = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        CompanyName = user.CompanyName,
                        EmailConfirmed = user.EmailConfirmed
                    };

                    defaultEmployers.Add(employer, user.Password);
                }

                foreach (var user in defaultUsers.Employees)
                {
                    Employee employee = new Employee
                    {
                        Email = user.Email,
                        UserName = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        EmployersEmail = user.EmployersEmail,
                        EmployeeConfirmed = user.EmployeeConfirmed,
                        EmailConfirmed = user.EmailConfirmed
                    };

                    defaultEmployees.Add(employee, user.Password);
                }
            }
            catch (JsonException ex)
            {
                System.Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }


        public class EmployeeSetting
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string PhoneNumber { get; set; } // Typ long, jak wyżej
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string EmployersEmail { get; set; } // Nowe pole
            public bool EmployeeConfirmed { get; set; } // Nowe pole
            public bool EmailConfirmed { get; set; }
        }

        public class EmployerSetting
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string PhoneNumber { get; set; }
            public string CompanyName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public bool EmailConfirmed { get; set; }

        }

        private class SeederSettings
        {
            public DefaultUsers DefaultUsers { get; set; }
        }

        private class DefaultUsers
        {
            public List<EmployerSetting> Employers { get; set; }
            public List<EmployeeSetting> Employees { get; set; }
        }

        private class SeederSettingsContainer
        {
            public SeederSettings SeederSettings { get; set; }
        }

    }
}
