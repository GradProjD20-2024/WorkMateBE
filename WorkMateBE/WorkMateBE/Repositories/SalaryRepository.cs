using Microsoft.AspNetCore.Mvc;
using WorkMateBE.Data;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;

namespace WorkMateBE.Repositories
{
    public class SalaryRepository:ISalaryRepository
    {
        private readonly DataContext _context;

        public SalaryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateSalarySheet(int employeeId, int month, int year)
        {
            var employee = _context.Employees.Where(p => p.Id == employeeId).FirstOrDefault();
            int baseSalary = employee.BaseSalary;
            int bonus = CalculateBonus(baseSalary, CountWorkingHoursReality(employeeId, month,year), CountWorkingHoursStandard(month, year));
            int realitySalary = baseSalary / CountWorkingHoursStandard(month, year) * CountWorkingHoursReality(employeeId, month, year);
            int deduction = CalculateDeduction(employeeId, month, year);
            var Salary = new Salary
            {
                BaseSalary = baseSalary,
                Bonus = bonus,
                Deduction = deduction,
                Total = realitySalary + bonus - deduction,
                Month = month,
                Year = year,
                EmployeeId = employeeId,
                Status = 0,
            };
            _context.Add(Salary);

            return Save();
        }
        public int CountWorkingHoursStandard(int month, int year)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);

            int workingDays = 0;

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime currentDate = new DateTime(year, month, day);

                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++; 
                }
            }
            return workingDays * 8;
        }

        public int CountWorkingHoursReality(int employeeId, int month, int year)
        {
            var account = _context.Accounts.Where(p => p.EmployeeId == employeeId).FirstOrDefault();
            var attendances = _context.Attendances
                .Where(p => p.AccountId == account.Id && p.CreatedAt.Month == month && p.CreatedAt.Year == year && p.Status == 1)
                .ToList();

            int hoursCounter = 0;
            TimeSpan lunchBreakStart = new TimeSpan(12, 0, 0);  // Nghỉ trưa bắt đầu từ 12:00
            TimeSpan lunchBreakEnd = new TimeSpan(13, 30, 0);   // Nghỉ trưa kết thúc lúc 13:30
            TimeSpan lunchBreakDuration = new TimeSpan(1, 30, 0);  // 1.5 giờ nghỉ trưa

            foreach (var attendance in attendances)
            {
                if (attendance.CheckOut != null)
                {
                    TimeSpan checkInTime = attendance.CheckIn.TimeOfDay;
                    TimeSpan checkOutTime = attendance.CheckOut.Value.TimeOfDay;
                    TimeSpan workDuration = attendance.CheckOut.Value - attendance.CheckIn;

                    // Kiểm tra xem thời gian làm việc có giao với thời gian nghỉ trưa không
                    if (checkInTime < lunchBreakEnd && checkOutTime > lunchBreakStart)
                    {
                        // Tính phần thời gian nghỉ trưa mà nhân viên không làm việc
                        TimeSpan lunchTimeOverlap = TimeSpan.Zero;

                        if (checkInTime <= lunchBreakStart && checkOutTime >= lunchBreakEnd)
                        {
                            // Toàn bộ thời gian nghỉ trưa (1.5 giờ)
                            lunchTimeOverlap = lunchBreakDuration;
                        }
                        else if (checkInTime > lunchBreakStart && checkInTime < lunchBreakEnd)
                        {
                            // Nhân viên bắt đầu làm trong khoảng nghỉ trưa
                            lunchTimeOverlap = lunchBreakEnd - checkInTime;
                        }
                        else if (checkOutTime > lunchBreakStart && checkOutTime < lunchBreakEnd)
                        {
                            // Nhân viên kết thúc làm việc trong khoảng nghỉ trưa
                            lunchTimeOverlap = checkOutTime - lunchBreakStart;
                        }

                        // Trừ phần thời gian nghỉ trưa khỏi thời gian làm việc
                        workDuration -= lunchTimeOverlap;
                    }

                    int hours = (int)workDuration.TotalHours;
                    hoursCounter += hours;
                }
            }
            return hoursCounter;
        }


        public int CalculateBonus(int baseSalary, int realHours, int standardHours)
        {
            int salaryHours = (baseSalary / standardHours) * 3 / 2;
            if (realHours > standardHours)
            {
                return salaryHours * (realHours - standardHours);
            }
            return 0;
        }
        
        public int CalculateDeduction(int employeeId, int month, int year)
        {
            var account = _context.Accounts.Where(p => p.EmployeeId == employeeId).FirstOrDefault();
            var attendances = _context.Attendances
                .Where(p => p.AccountId == account.Id && p.CreatedAt.Month == month && p.CreatedAt.Year == year && p.Late == 1 &&  p.Status == 1)
                .ToList();
            int lateCounter = attendances.Count;
            return lateCounter * 100000;
        }

        public ICollection<Salary> GetSalarySheet(int employeeId)
        {
            var salaries = _context.Salaries.Where(p => p.EmployeeId  == employeeId).ToList();
            return salaries;

        }

        public Salary GetSalary(int employeeId, int month, int year)
        {
            return _context.Salaries.Where(p => p.EmployeeId == employeeId && p.Month == month && p.Year == year).FirstOrDefault();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
