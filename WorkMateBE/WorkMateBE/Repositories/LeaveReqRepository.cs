using WorkMateBE.Data;
using WorkMateBE.Interfaces;

namespace WorkMateBE.Repositories
{
    public class LeaveReqRepository : ILeaveReqRepository
    {
        private readonly DataContext _context;

        public LeaveReqRepository(DataContext context)
        {
            _context = context;
        }

        
        
    }
}
