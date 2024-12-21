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
        public async Task<int> CheckIn(int accountId, byte[] photo)
        {
            int late = 0;
            DateTime now = DateTime.Now;
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
        public async Task<int> GetResultAsync(byte[] photoBytes)
        {
            // Kiểm tra dữ liệu byte có hợp lệ không
            if (photoBytes == null || photoBytes.Length == 0)
            {
                return -1; // Lỗi: Không có dữ liệu ảnh
            }

            try
            {
                using (var client = new HttpClient())
                {
                    // Tạo HttpContent từ byte[]
                    var byteContent = new ByteArrayContent(photoBytes);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream"); // Đặt đúng Content-Type

                    // Gửi POST request
                    var response = await client.PostAsync("http://127.0.0.1:5000/find_face", byteContent);

                    // Đọc kết quả trả về từ server dưới dạng JSON
                    var resultString = await response.Content.ReadAsStringAsync();

                    // Phân tích chuỗi JSON thành object
                    var resultJson = Newtonsoft.Json.Linq.JObject.Parse(resultString);

                    // Kiểm tra status code trong JSON trả về
                    var statusCode = (int)resultJson["status_code"];

                    if (statusCode == 200)
                    {
                        // Nếu thành công, trả về ID là tên file không có phần mở rộng
                        var userIdString = (string)resultJson["data"]["id"]; // Cập nhật key nếu cần
                        return int.TryParse(userIdString, out int userId) ? userId : -1; // Trả về userId nếu có thể, -1 nếu không thể
                    }
                    else if (statusCode == 404)
                    {
                        // Không tìm thấy khuôn mặt trùng khớp
                        return 0; // Không tìm thấy khuôn mặt
                    }
                    else
                    {
                        // Lỗi từ server hoặc không tìm thấy khuôn mặt trong ảnh tải lên
                        return -1; // Lỗi
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (nếu có)
                Console.WriteLine($"Error: {ex.Message}");
                return -1; // Lỗi
            }
        }



        public async Task<int> CheckOut(int attendanceId, byte[] photo)
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
