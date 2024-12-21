using WorkMateBE.Data;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace WorkMateBE.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly DataContext _context;
        private static readonly HttpClient client = new HttpClient();
        public AttendanceRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<int> CheckIn(int accountId)
        {
            int late = 0;
            DateTime now = DateTime.Now;
            if (now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
            {
                return -1;
            }
            DateTime targetTime = new DateTime(now.Year, now.Month, now.Day, 8, 30, 0);
            if(now > targetTime)
            {
                late = 1;
            }
            var attendance = new Attendance
            {
                CheckIn = DateTime.Now,
                CheckOut = null,
                Status = 0,
                Late = late,
                AccountId = accountId,
            };
            _context.Add(attendance);
            _context.SaveChanges();
            return 1;
        }
        


        public async Task<int> CheckOut(int attendanceId)
        {
            var attendance = _context.Attendances.Where(p => p.Id == attendanceId).FirstOrDefault();
            if (attendance.CheckOut != null)
            {
                return -2;
            }
            var now = DateTime.Now;
            DateTime targetTime = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);
            if (now < targetTime)
            {
                attendance.Late = 1;
            }
            attendance.CheckOut = DateTime.Now;
            attendance.Status = 1;
            _context.Update(attendance);
            _context.SaveChanges();
            return 1;
        }

        public Attendance GetAttendanceById(int attendanceId)
        {
            var attendance = _context.Attendances.Where(p => p.Id == attendanceId).FirstOrDefault();
            return attendance;
        }

        public ICollection<Attendance> GetAttendancesByAccountId(int accountId)
        {
            var  attendances = _context.Attendances.Where(p => p.AccountId == accountId).ToList();
            return attendances;
        }

        public int CheckDay(DateTime dateTime, int accountId)
        {
            var attendance = _context.Attendances.Where(p=> p.CheckIn.Date == dateTime.Date && p.AccountId == accountId && p.Status==0).FirstOrDefault();
            if(attendance == null)
            {
                return 0;
            }
            return 1;
        }

        
    }
}
