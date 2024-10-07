using WorkMateBE.Models;

namespace WorkMateBE.Interfaces
{
    public interface IEmployeeRepository
    {
        bool CreateEmployee(Employee employee);
        bool UpdateEmployee(int employeeId, Employee employee);
        bool DeleteEmployee(int employeeId);
        ICollection<Employee> GetAll();
        Employee GetEmployeeById(int employeeId);
    }
}
