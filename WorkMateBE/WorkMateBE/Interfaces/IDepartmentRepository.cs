using WorkMateBE.Models;

namespace WorkMateBE.Interfaces
{
    public interface IDepartmentRepository
    {
        bool CreateDepartment(Department department);
        bool UpdateDepartment(int departmentId, Department department);
        bool DeleteDepartment(int departmentId);
        ICollection<Department> GetAll();
        Department GetDepartmentById(int departmentId);
    }
}
