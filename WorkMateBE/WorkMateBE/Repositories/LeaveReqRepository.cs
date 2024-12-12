using Microsoft.EntityFrameworkCore;
using WorkMateBE.Data;
using WorkMateBE.Dtos.LeaveRequestDto;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkMateBE.Repositories
{
    public class LeaveReqRepository : ILeaveReqRepository
    {
        private readonly DataContext _context;

        public LeaveReqRepository(DataContext context)
        {
            _context = context;
        }
        public List<LeaveRequest> GetAllRequest()
        {
            return _context.LeaveRequests.ToList();
        }
        public bool CreateLeaveRequest(int accountId, PostRequestDto model)
        {
            if(CheckLeaveRequestExist(accountId, model.Date))
            {
                return false;
            }
            var account = _context.Accounts.Find(accountId);
            var leaveRequest = new LeaveRequest
            {   
                EmployeeId = account.EmployeeId,
                Date = model.Date,
                Reason = model.Reason,
            };
            var attendance = new Attendance
            {
                CheckIn = model.Date.Date.AddHours(8).AddMinutes(30),
                CheckOut = model.Date.Date.AddHours(18),
                Status = 2,
                Late = 0,
                AccountId = accountId,
            };

            
            _context.Add(leaveRequest);
            _context.Add(attendance);
            return Save();
        }
        public bool CheckLeaveRequestExist(int accountId, DateTime date)
        {
            var account = _context.Accounts.Find(accountId);
            var leaveRequest = _context.LeaveRequests
                    .Where(p => p.EmployeeId == account.EmployeeId
                     && EF.Functions.DateDiffDay(p.Date, date) == 0)
                    .FirstOrDefault();
            return leaveRequest != null;
        }
        public bool Approve(int id)
        {
            var leaveRequest = _context.LeaveRequests.Find(id);
            if (leaveRequest == null)
            {
                return false;
            }
            leaveRequest.Status = 1;
            _context.Update(leaveRequest);
            return Save();
        }
        public bool Reject(int id)
        {
            var leaveRequest = _context.LeaveRequests.Find(id);
            if(leaveRequest == null)
            {
                return false;
            }
            leaveRequest.Status = 2;
            _context.Update(leaveRequest);
            return Save();
        }
        public List<LeaveRequest> GetLeaveRequestByAccountId(int accountId)
        {
            var account = _context.Accounts.Find(accountId);
            var leaveRequests = _context.LeaveRequests.Where(p => p.EmployeeId == account.EmployeeId).ToList();
            return leaveRequests;
        }

        public int DeleteRequest(int id, int accountId)
        {
            var request = _context.LeaveRequests.Find(id);
            var account = _context.Accounts.Find(accountId);
            if(request.EmployeeId != account.EmployeeId)
            {
                return -2;
            }
            
            if (request == null)
            {
                return -1;
            }
            if (request.Status != 0)
            {
                return -1;
            }
            _context.Remove(request);
            Save();
            return 1;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
