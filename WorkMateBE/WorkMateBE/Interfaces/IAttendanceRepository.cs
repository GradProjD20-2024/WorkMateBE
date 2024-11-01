using WorkMateBE.Models;

namespace WorkMateBE.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<int> CheckIn(int accountId, byte[] photo);
        Task<int> CheckOut(int accountId, byte[] photo);
        Task<int> GetResultAsync(byte[] photo);
        Attendance GetAttendanceById(int attendanceId);
        ICollection<Attendance> GetAttendancesByAccountId(int accountId);
        int CheckDay(DateTime dateTime, int accountId);

    }
}
