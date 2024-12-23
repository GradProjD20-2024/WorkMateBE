using WorkMateBE.Data;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;

namespace WorkMateBE.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;

        public EmployeeRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateEmployee(Employee employee)
        {
            _context.Add(employee);
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool DeleteEmployee(int employeeId)
        {
            var employee = _context.Employees.Where(p => p.Id == employeeId).FirstOrDefault();
            var account = _context.Accounts.Where(p => p.EmployeeId == employeeId).FirstOrDefault();
            _context.Remove(employee);
            _context.Remove(account);
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public ICollection<Employee> GetAll()
        {
            var employees = _context.Employees.ToList();
            return employees;
        }

        public Employee GetEmployeeById(int employeeId)
        {
            var employee = _context.Employees.Find(employeeId);
            employee.Department = _context.Departments.Find(employee.DepartmentId);
            return employee;
        }

        public bool UpdateEmployee(int employeeId, Employee employeeUpdate)
        {
            var employee = _context.Employees.Where(p => p.Id == employeeId).FirstOrDefault();
            employee.FullName = employeeUpdate.FullName;
            employee.ImageUrl = employeeUpdate.ImageUrl;
            employee.Gender = employeeUpdate.Gender;
            employee.Phone = employeeUpdate.Phone;
            employee.Birthday = employeeUpdate.Birthday;
            employee.IdentificationId = employeeUpdate.IdentificationId;
            employee.Position = employeeUpdate.Position;
            employee.Address = employeeUpdate.Address;
            employee.Status = employeeUpdate.Status;
            employee.BaseSalary = employeeUpdate.BaseSalary;
            employee.DepartmentId = employeeUpdate.DepartmentId;
            _context.Update(employee);
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public List<Employee> GetEmployeesByDepartment(int departmentId)
        {
            var employees = _context.Employees.Where(p => p.DepartmentId == departmentId).ToList();
            return employees;
        }
    }
}
