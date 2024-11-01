using WorkMateBE.Data;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;

namespace WorkMateBE.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataContext _context;

        public DepartmentRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateDepartment(Department department)
        {
            _context.Add(department);
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool DeleteDepartment(int departmentId)
        {
            var department = _context.Departments.Where(p => p.Id == departmentId).FirstOrDefault();
            _context.Remove(department);
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public ICollection<Department> GetAll()
        {
            var departments = _context.Departments.ToList();
            return departments;
        }

        public Department GetDepartmentById(int departmentId)
        {
            var department = _context.Departments.Find(departmentId);
            return department;
        }

        public bool UpdateDepartment(int departmentId, Department departmentUpdate)
        {
            var department = _context.Departments.Where(p => p.Id == departmentId).FirstOrDefault();
            department.Name = departmentUpdate.Name;
            department.Description = departmentUpdate.Description;
            department.Status = departmentUpdate.Status;
            _context.Update(department);
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
