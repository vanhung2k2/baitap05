using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppV1
{

    public struct Employee
    {
        public string EmployeeID;
        public string EmployeeName;
        public DateTime DateJoined;
        public double SalaryCoefficient;
        public string JobPosition;

        public Employee(string employeeID, string employeeName, DateTime dateJoined, double salaryCoefficient, string jobPosition)
        {
            EmployeeID = employeeID;
            EmployeeName = employeeName;
            DateJoined = dateJoined;
            SalaryCoefficient = salaryCoefficient;
            JobPosition = jobPosition;
        }
    }

    internal class Program
    {

        static List<Employee> employees = new List<Employee>();
        static void Main(string[] args)
        {
            int choice;
            do
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Nhập danh sách Nhân viên từ bàn phím");
                Console.WriteLine("2. Nhập danh sách nhân viên từ file excel có sẵn");
                Console.WriteLine("3. Hiển thị danh sách nhân viên");
                Console.WriteLine("4. Xuất file excel danh sách nhân viên theo các mốc 5 năm và 10 năm");
                Console.WriteLine("0. Thoát");
                Console.Write("Lựa chọn của bạn: ");

                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        InputEmployeesFromKeyboard();
                        break;
                    case 2:
                        InputEmployeesFromExcel();
                        break;
                    case 3:
                        DisplayEmployees();
                        break;
                    case 4:
                        ExportEmployeesToExcel();
                        break;
                    case 0:
                        Console.WriteLine("Thoát chương trình.");
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                        break;
                }
            } while (choice != 0);
        }
        static void InputEmployeesFromKeyboard()
        {
            Console.Write("Nhập số lượng nhân viên: ");
            int n = int.Parse(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Nhập thông tin nhân viên thứ {i + 1}:");
                Console.Write("Mã nhân viên: ");
                string id = Console.ReadLine();
                Console.Write("Tên nhân viên: ");
                string name = Console.ReadLine();
                Console.Write("Ngày vào công ty (dd/MM/yyyy): ");
                DateTime dateJoined = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                Console.Write("Hệ số lương: ");
                double salaryCoefficient = double.Parse(Console.ReadLine());
                Console.Write("Vị trí công việc: ");
                string jobPosition = Console.ReadLine();

                employees.Add(new Employee(id, name, dateJoined, salaryCoefficient, jobPosition));
            }
        }
        static void InputEmployeesFromExcel()
        {

        }
        static void DisplayEmployees()
        {
            Console.WriteLine("Danh sách nhân viên:");
            foreach (var employee in employees)
            {
                Console.WriteLine($"Mã: {employee.EmployeeID}, Tên: {employee.EmployeeName}, Ngày vào: {employee.DateJoined.ToString("dd/MM/yyyy")}, Hệ số lương: {employee.SalaryCoefficient}, Vị trí: {employee.JobPosition}");
            }

        }
        static void ExportEmployeesToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var employees5Years = employees.Where(e => (DateTime.Now - e.DateJoined).TotalDays >= 5 * 365).ToList();
            var employees10Years = employees.Where(e => (DateTime.Now - e.DateJoined).TotalDays >= 10 * 365).ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet5Years = package.Workbook.Worksheets.Add("5Years");
                var worksheet10Years = package.Workbook.Worksheets.Add("10Years");

                // Header
                string[] headers = { "Mã nhân viên", "Tên nhân viên", "Ngày vào công ty", "Hệ số lương", "Vị trí công việc" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet5Years.Cells[1, i + 1].Value = headers[i];
                    worksheet10Years.Cells[1, i + 1].Value = headers[i];
                }

                // 5 years sheet
                for (int i = 0; i < employees5Years.Count; i++)
                {
                    var emp = employees5Years[i];
                    worksheet5Years.Cells[i + 2, 1].Value = emp.EmployeeID;
                    worksheet5Years.Cells[i + 2, 2].Value = emp.EmployeeName;
                    worksheet5Years.Cells[i + 2, 3].Value = emp.DateJoined.ToString("dd/MM/yyyy");
                    worksheet5Years.Cells[i + 2, 4].Value = emp.SalaryCoefficient;
                    worksheet5Years.Cells[i + 2, 5].Value = emp.JobPosition;
                }

                // 10 years sheet
                for (int i = 0; i < employees10Years.Count; i++)
                {
                    var emp = employees10Years[i];
                    worksheet10Years.Cells[i + 2, 1].Value = emp.EmployeeID;
                    worksheet10Years.Cells[i + 2, 2].Value = emp.EmployeeName;
                    worksheet10Years.Cells[i + 2, 3].Value = emp.DateJoined.ToString("dd/MM/yyyy");
                    worksheet10Years.Cells[i + 2, 4].Value = emp.SalaryCoefficient;
                    worksheet10Years.Cells[i + 2, 5].Value = emp.JobPosition;
                }

                Console.Write("Nhập tên file để lưu (bao gồm đuôi .xlsx): ");
                string fileName = Console.ReadLine();
                var fileInfo = new FileInfo(fileName);
                package.SaveAs(fileInfo);
                Console.WriteLine($"File đã được lưu thành công tại {fileInfo.FullName}");
            }

        }
    }
}