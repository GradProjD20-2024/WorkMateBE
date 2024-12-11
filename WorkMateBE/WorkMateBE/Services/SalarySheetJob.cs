using Quartz;
using WorkMateBE.Data;
using WorkMateBE.Interfaces;

namespace WorkMateBE.Services
{
    public class SalarySheetJob : IJob
    {
        private readonly DataContext _context;
        private readonly ISalaryRepository _salaryRepository;

        public SalarySheetJob(DataContext context, ISalaryRepository salaryRepository)
        {
            _context = context;
            _salaryRepository = salaryRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var today = DateTime.Today;
                var lastMonth = today.AddMonths(-1);
                int month = lastMonth.Month;
                int year = lastMonth.Year;

                // Lấy danh sách nhân viên
                var employees = _context.Employees.ToList();

                foreach (var employee in employees)
                {
                    // Gọi hàm CreateSalarySheet
                    _salaryRepository.CreateSalarySheet(employee.Id, month, year);
                }

                Console.WriteLine($"Lập bảng lương cho tháng {month}/{year} đã hoàn thành.");
            }
            catch (Exception ex)
            {
                // Kiểm tra InnerException nếu có
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    Console.WriteLine("Inner Exception StackTrace: " + ex.InnerException.StackTrace);
                }
                else
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    Console.WriteLine("StackTrace: " + ex.StackTrace);
                }

                // Ghi lại toàn bộ Exception vào log
               
            }
        }


    }
}
