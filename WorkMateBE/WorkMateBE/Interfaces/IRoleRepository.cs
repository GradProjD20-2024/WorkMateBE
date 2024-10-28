using WorkMateBE.Models;

namespace WorkMateBE.Interfaces
{
    public interface IRoleRepository
    {
        bool CreateRole(Role role);
        List<Role> GetRole();
        Role GetRoleById(int id);
        bool UpdateRole(int id, Role role);
        bool DeleteRole(int id);
        
    }
}
