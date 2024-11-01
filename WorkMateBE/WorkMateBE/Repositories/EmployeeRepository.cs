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
            _context.Remove(employee);
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
            var employees = _context.Employees.Find(employeeId);
            return employees;
        }

        public bool UpdateEmployee(int employeeId, Employee employeeUpdate)
        {
            var employee = _context.Employees.Where(p => p.Id == employeeId).FirstOrDefault();
            employee.FullName = employeeUpdate.FullName;
            employee.Phone = employeeUpdate.Phone;
            employee.Birthday = employeeUpdate.Birthday;
            employee.IdentificationId = employeeUpdate.IdentificationId;
            employee.Position = employeeUpdate.Position;
            employee.Address = employeeUpdate.Address;
            employee.Status = employeeUpdate.Status;
            employee.DepartmentId = employeeUpdate.DepartmentId;
            _context.Update(employee);
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
