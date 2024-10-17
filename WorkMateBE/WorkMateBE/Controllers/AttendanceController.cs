using Microsoft.AspNetCore.Mvc;
using WorkMateBE.Interfaces;
using System.Threading.Tasks;
using WorkMateBE.Responses;
using WorkMateBE.Models;

namespace WorkMateBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : Controller
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IAccountRepository _accountRepository;

        public AttendanceController(IAttendanceRepository attendanceRepository, IAccountRepository accountRepository)
        {
            _attendanceRepository = attendanceRepository;
            _accountRepository = accountRepository;
        }

        [HttpPost("check-in/{accountId}")]
        public async Task<IActionResult> CheckIn(int accountId, IFormFile file)
        {
            // Kiểm tra tài khoản có tồn tại không
            var account = _accountRepository.GetAccountById(accountId);
            if (account == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Account ID not exists",
                    Data = null
                });
            }

            // Kiểm tra file tải lên
            if (file == null || file.Length == 0)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "No file uploaded",
                    Data = null
                });
            }

            // Đọc tệp tin thành byte[]
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            // Gọi hàm GetResultAsync để xử lý tệp
            var result = await _attendanceRepository.CheckIn(accountId, fileBytes);
            if(result == -1)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Something went wrong while Checkin",
                    Data = null
                });
            }
            if (result == 0)
            {
                return Ok(new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Face ID is invalid",
                    Data = null
                });
            }

            // Trả về kết quả check-in
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Check-in success",
                Data = null
            });
        }

        [HttpPost("check-out/{attendanceId}")]
        public async Task<IActionResult> CheckOut(int attendanceId, IFormFile file)
        {
            // Kiểm tra tài khoản có tồn tại không
            var attendance = _attendanceRepository.GetAttendanceById(attendanceId);
            if (attendance == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Attendance ID not exists",
                    Data = null
                });
            }

            // Kiểm tra file tải lên
            if (file == null || file.Length == 0)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "No file uploaded",
                    Data = null
                });
            }

            // Đọc tệp tin thành byte[]
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            // Gọi hàm GetResultAsync để xử lý tệp
            var result = await _attendanceRepository.CheckOut(attendanceId, fileBytes);
            if (result == -2)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "You already check-out",
                    Data = null
                });
            }
            if (result == -1)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Something went wrong while Checkout",
                    Data = null
                });
            }
            if (result == 0)
            {
                return Ok(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Face ID is invalid",
                    Data = null
                });
            }

            // Trả về kết quả check-in
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Check-out success",
                Data = null
            });
        }
        [HttpGet("{accountId}")]
        public IActionResult GetAttendanceByAccountId (int accountId)
        {
            var account = _accountRepository.GetAccountById(accountId);
            if (account == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Account ID not exists",
                    Data = null
                });
            }
            var attendances = _attendanceRepository.GetAttendancesByAccountId(accountId);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Attendance Success",
                Data = attendances
            });
        }


    }
}
