using Microsoft.AspNetCore.Mvc;
using WorkMateBE.Interfaces;
using System.Threading.Tasks;
using WorkMateBE.Responses;
using WorkMateBE.Models;
using System.Security.Claims;

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

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn()
        {
            var accountId = GetAccountIdFromToken();
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
            var now = DateTime.Now;
            if(_attendanceRepository.CheckDay(now, accountId) == 1)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Have you checked in today?",
                    Data = null
                });
            }
            // Gọi hàm GetResultAsync để xử lý tệp
            var result = await _attendanceRepository.CheckIn(accountId);
            if(result == -1)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Today is day off?",
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
        public async Task<IActionResult> CheckOut(int attendanceId)
        {
            var accountId = GetAccountIdFromToken();
            // Kiểm tra tài khoản có tồn tại không
            var attendance = _attendanceRepository.GetAttendanceById(attendanceId);
           if(attendance.AccountId != accountId)
            {
                return Forbid();
            }
            if (attendance == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Attendance ID not exists",
                    Data = null
                });
            }

            var result = await _attendanceRepository.CheckOut(attendanceId);
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

        #region
        private int GetAccountIdFromToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var accountIdClaim = identity.FindFirst("Id");
                if (accountIdClaim != null)
                {
                    if (int.TryParse(accountIdClaim.Value, out var accountId))
                    {
                        return accountId;
                    }
                }
            }

            throw new UnauthorizedAccessException("AccountId not found in token");
        }
        private int GetRoleFromToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                // Tìm claim có type là "Role"
                var roleClaim = identity.FindFirst("Role");
                if (roleClaim != null)
                {
                    // Chuyển đổi giá trị role từ chuỗi thành int
                    if (int.TryParse(roleClaim.Value, out var role))
                    {
                        return role;
                    }
                    else
                    {
                        throw new UnauthorizedAccessException($"Invalid role value: {roleClaim.Value}");
                    }
                }
            }

            throw new UnauthorizedAccessException("Role not found in token");

        }
        #endregion
    }
}
