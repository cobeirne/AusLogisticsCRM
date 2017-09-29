using AusLogisticsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AusLogisticsWebsite.Controllers
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 2
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       24/04/16
    /// </summary>

    public class MigrationController
    {
        public string AppDataPath{ get; set;}

        public string CsvEmployeeFileName { get; set; }

        public string CsvSalariesFileName { get; set; }

        public string CsvEmployeesToMigrate { get; set; }

        public MigrationController()
        {
            this.AppDataPath = HttpContext.Current.Server.MapPath("~/App_Data/");
            this.CsvEmployeeFileName = Properties.Settings.Default.CsvEmployeeFileName;
            this.CsvSalariesFileName = Properties.Settings.Default.CsvSalariesFileName;
            this.CsvEmployeesToMigrate = Properties.Settings.Default.CsvEmployeesToMigrate;
        }

        public MigrationController(string appDataPath)
        {
            this.AppDataPath = appDataPath;
            this.CsvEmployeeFileName = Properties.Settings.Default.CsvEmployeeFileName;
            this.CsvSalariesFileName = Properties.Settings.Default.CsvSalariesFileName;
            this.CsvEmployeesToMigrate = Properties.Settings.Default.CsvEmployeesToMigrate;
        }

        public EmployeeModel SelectCsvEmployee(int empNo)
        {
            EmployeeModel selectEmployee = null;

            try
            {
                string csvFilePath = this.AppDataPath + this.CsvEmployeeFileName;

                string[] csvLines = System.IO.File.ReadAllLines(csvFilePath);

                IEnumerable<EmployeeModel> query = (from csvLine in csvLines.Skip(1)
                                                    let splitLine = csvLine.Split('|')                                                    
                                                    select new EmployeeModel(splitLine[0], splitLine[1], splitLine[2], splitLine[3], splitLine[4], splitLine[5]));

                selectEmployee = query.First(e => e.EmployeeNo == empNo);
            }
            catch (Exception)
            {
            }

            return selectEmployee;
        }

        public List<int> SelectEmployeesToMigrate()
        {
            List<int> employeeList = new List<int>();

            try
            {
                string csvFilePath = this.AppDataPath + this.CsvEmployeesToMigrate;

                string[] csvLines = System.IO.File.ReadAllLines(csvFilePath);

                IEnumerable<int> query = (from csvLine in csvLines.Skip(1)
                                          select Convert.ToInt32(csvLine));

                employeeList = query.ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Select Employees To Migrate Exception", e.InnerException);
            }

            return employeeList;
        }

        public List<SalaryModel> SelectEmployeeSalaries()
        {
            List<SalaryModel> empSalaries = new List<SalaryModel>();
            
            try
            {
                string csvFilePath = this.AppDataPath + this.CsvSalariesFileName;

                string[] csvLines = System.IO.File.ReadAllLines(csvFilePath);

                IEnumerable<SalaryModel> query = (from csvLine in csvLines.Skip(1)
                                          let splitLine = csvLine.Split('|')                                                    
                                          select new SalaryModel(splitLine[0], splitLine[1], splitLine[2], splitLine[3]));

                empSalaries = query.ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Select Employee Salary Exception", e.InnerException);
            }

            return empSalaries;
        }

        public decimal SelectEmployeeCurrentSalary(int empNo, List<SalaryModel> salaries)
        {
            decimal currentSalary = 0.0M;

            try
            {
                List<SalaryModel> empSalaries = salaries.Where(s => s.EmployeeNo == empNo).ToList();
                empSalaries = empSalaries.OrderByDescending(s => s.ToDate).ToList();
                currentSalary = empSalaries.First().Salary;
            }
            catch
            { }

            return currentSalary;
        }

        public int MigrateCsvEmployees(MemberController memberController)
        {
            int migrations = 0;

            List<int> employees = SelectEmployeesToMigrate();

            List<SalaryModel> salaries = SelectEmployeeSalaries();

            foreach (int employeeId in employees)
            {
                decimal employeeSalary = SelectEmployeeCurrentSalary(employeeId, salaries);

                bool migrationOk = MigrateCsvEmployee(memberController, employeeSalary, employeeId);

                if (migrationOk)
                {
                    migrations += 1;
                }
            }

            return migrations;
        }

        public bool MigrateCsvEmployee(MemberController memberController, decimal employeeSalary, int employeeId)
        {
            bool migrationOk = false;

            EmployeeModel employee = SelectCsvEmployee(employeeId);

            if (employee != null)
            {
                MemberModel newMember = new MemberModel
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    DateJoined = employee.HireDate,
                    DateOfBirth = employee.BirthDate,
                    Salary = employeeSalary,
                    Gender = employee.Gender,
                    MemberClassId = 4
                };

                migrationOk = memberController.AddMember(newMember);

            }

            return migrationOk;
        }
    }
}