using BusinessLogic.DataBasesContext;
using Bogus;
using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;
using BusinessLogic.Enums;

namespace DevelopmentOnly
{
    public class DatabaseSeeder(
        VacationManagerDbContext db,
        UserManager<User> userManager
        )
    {
        public async Task Seed(int employersAmount, (int, int) employeePerEmployerRange, (int, int) vacationPerEmployeeRange)
        {
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


    }
}
