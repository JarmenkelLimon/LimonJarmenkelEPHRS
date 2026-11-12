using System;
using System.Collections.Generic;
using System.Text;

namespace FinalsAct_2
{
    public class Employee
    {
        public int id { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string dept { get; set; } = string.Empty;
        public string jobTitle { get; set; } = string.Empty;
        public decimal monthlySalary { get; set; }
        public double taxRate { get; set; }
        public DateTime hireDate { get; set; }
        public bool isActive { get; set; }
        public string email { get; set; } = string.Empty;
    }

    class Program
    {
        static List<Employee> employees = new List<Employee>();
        static int nextId = 1;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("---- Employee Payroll & HR Management System ----");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. View Employees & Payroll");
                Console.WriteLine("3. Update Employee");
                Console.WriteLine("4. Delete Employee");
                Console.WriteLine("5. Exit");
                Console.Write("\nSelect an Option: ");

                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        CreateEmployee();
                        break;
                    case "2":
                        ViewEmployees();
                        break;
                    case "3":
                        UpdateEmployee();
                        break;
                    case "4":
                        DeleteEmployee();
                        break;
                    case "5":
                        Console.WriteLine("--- Closing Program ---");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid Choice.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ReadKey();
                }
            }
        }

        static void CreateEmployee()
        {
            Console.WriteLine("--- Add a New Employee ---");

            Console.Write("First Name: ");
            string firstName = Console.ReadLine() ?? "Unknown";

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine() ?? "Unknown";

            Console.Write("Department: ");
            string dept = Console.ReadLine() ?? "General";

            Console.Write("Job Title: ");
            string jobTitle = Console.ReadLine() ?? "Staff";

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "none@company.com";

            Console.Write("Monthly Salary (₱): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal monthlySalary) || monthlySalary < 0)
            {
                Console.WriteLine("Invalid salary entered. Employee creation cancelled.");
                return;
            }

            Console.Write("Tax Rate (enter percentage, e.g. 15 for 15%): ");
            if (!double.TryParse(Console.ReadLine(), out double taxPercent) || taxPercent < 0 || taxPercent > 100)
            {
                Console.WriteLine("Invalid tax rate entered. Employee creation cancelled.");
                return;
            }
            double taxRate = taxPercent / 100.0;

            Console.Write("Hire Date (YYYY-MM-DD): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime hireDate))
            {
                Console.WriteLine("Invalid date format. Employee creation cancelled.");
                return;
            }

            Console.Write("Is Active? (y/n): ");
            string activeInput = (Console.ReadLine() ?? "y").Trim().ToLower();
            bool isActive = activeInput == "y" || activeInput == "yes" || activeInput == "true";

            employees.Add(new Employee
            {
                id = nextId++,
                firstName = firstName,
                lastName = lastName,
                dept = dept,
                jobTitle = jobTitle,
                monthlySalary = monthlySalary,
                taxRate = taxRate,
                hireDate = hireDate,
                isActive = isActive,
                email = email
            });

            Console.WriteLine("\nEmployee added successfully!");
        }

        static void ViewEmployees()
        {
            Console.WriteLine("--- Employee & Payroll List ---");

            if (employees.Count == 0)
            {
                Console.WriteLine("No employees on record.");
                return;
            }

            foreach (var emp in employees)
            {
                decimal taxAmount = emp.monthlySalary * (decimal)emp.taxRate;
                decimal netPay = emp.monthlySalary - taxAmount;
                string status = emp.isActive ? "Active" : "Inactive";

                Console.WriteLine($"ID: {emp.id} | {emp.firstName} {emp.lastName} ({status})");
                Console.WriteLine($"   Role: {emp.jobTitle} - {emp.dept} | Email: {emp.email}");
                Console.WriteLine($"   Hired: {emp.hireDate:yyyy-MM-dd} | Gross: ₱{emp.monthlySalary:N2} | Tax ({emp.taxRate * 100:F0}%): ₱{taxAmount:N2} | Net Pay: ₱{netPay:N2}");
                Console.WriteLine(new string('-', 85));
            }
        }

        static void UpdateEmployee()
        {
            ViewEmployees();
            if (employees.Count == 0) return;

            Console.Write("\nEnter the ID of the employee to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Employee? empToUpdate = employees.Find(e => e.id == id);
            if (empToUpdate == null)
            {
                Console.WriteLine("Employee not found.");
                return;
            }

            Console.WriteLine("\nLeave a field blank to keep its current value.");

            Console.Write($"New First Name ({empToUpdate.firstName}): ");
            string? newFirst = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newFirst)) empToUpdate.firstName = newFirst;

            Console.Write($"New Last Name ({empToUpdate.lastName}): ");
            string? newLast = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newLast)) empToUpdate.lastName = newLast;

            Console.Write($"New Department ({empToUpdate.dept}): ");
            string? newDept = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDept)) empToUpdate.dept = newDept;

            Console.Write($"New Job Title ({empToUpdate.jobTitle}): ");
            string? newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle)) empToUpdate.jobTitle = newTitle;

            Console.Write($"New Email ({empToUpdate.email}): ");
            string? newEmail = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newEmail)) empToUpdate.email = newEmail;

            Console.Write($"New Monthly Salary ({empToUpdate.monthlySalary:F2}): ");
            string? newSalary = Console.ReadLine();
            if (decimal.TryParse(newSalary, out decimal parsedSalary) && parsedSalary >= 0)
                empToUpdate.monthlySalary = parsedSalary;

            Console.Write($"New Tax Rate % ({empToUpdate.taxRate * 100:F0}): ");
            string? newTax = Console.ReadLine();
            if (double.TryParse(newTax, out double parsedTax) && parsedTax >= 0 && parsedTax <= 100)
                empToUpdate.taxRate = parsedTax / 100.0;

            Console.Write($"New Hire Date ({empToUpdate.hireDate:yyyy-MM-dd}): ");
            string? newDate = Console.ReadLine();
            if (DateTime.TryParse(newDate, out DateTime parsedDate))
                empToUpdate.hireDate = parsedDate;

            Console.Write($"Is Active? ({(empToUpdate.isActive ? "y" : "n")}): ");
            string? newActive = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newActive))
            {
                string val = newActive.Trim().ToLower();
                if (val == "y" || val == "yes" || val == "true") empToUpdate.isActive = true;
                else if (val == "n" || val == "no" || val == "false") empToUpdate.isActive = false;
            }

            Console.WriteLine("\nEmployee updated successfully!");
        }

        static void DeleteEmployee()
        {
            ViewEmployees();
            if (employees.Count == 0) return;

            Console.Write("\nEnter the ID of the employee to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Employee? empToDelete = employees.Find(e => e.id == id);
            if (empToDelete == null)
            {
                Console.WriteLine("Employee not found.");
                return;
            }

            employees.Remove(empToDelete);
            Console.WriteLine($"\nEmployee \"{empToDelete.firstName} {empToDelete.lastName}\" has been deleted.");
        }
    }
}
