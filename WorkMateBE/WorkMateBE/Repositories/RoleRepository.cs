using WorkMateBE.Data;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;

namespace WorkMateBE.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context;

        public RoleRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateRole(Role role)
        {
            _context.Add(role);
            return Save();
        }

        public bool DeleteRole(int id)
        {
            var role = GetRoleById(id);
            _context.Remove(role);
            return Save();
        }

        public List<Role> GetRole()
        {
            var roles = _context.Roles.ToList();
            return roles;
        }

        public Role GetRoleById(int id)
        {
            var role = _context.Roles.Where(p => p.Id == id).FirstOrDefault();
            return role;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateRole(int id, Role updateRole)
        {
            var role = GetRoleById(id);
            role.Name = updateRole.Name;
            role.Description = updateRole.Description;
            _context.Roles.Update(role);
            return Save();

        }
    }
}
