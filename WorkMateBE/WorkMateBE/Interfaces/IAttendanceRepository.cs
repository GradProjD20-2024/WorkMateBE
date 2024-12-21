using WorkMateBE.Models;

namespace WorkMateBE.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<int> CheckIn(int accountId);
        Task<int> CheckOut(int accountId);
        Attendance GetAttendanceById(int attendanceId);
        ICollection<Attendance> GetAttendancesByAccountId(int accountId);
        int CheckDay(DateTime dateTime, int accountId);

    }
}
