using WorkMateBE.Dtos.LeaveRequestDto;
using WorkMateBE.Models;

namespace WorkMateBE.Interfaces
{
    public interface ILeaveReqRepository
    {
        int CreateLeaveRequest(int accountId, PostRequestDto model);
        bool Approve(int id);
        bool Reject(int id);
        List<LeaveRequest> GetLeaveRequestByAccountId(int accountId);
        int DeleteRequest(int id, int accountId);
        List<LeaveRequest> GetAllRequest();
    }
}
