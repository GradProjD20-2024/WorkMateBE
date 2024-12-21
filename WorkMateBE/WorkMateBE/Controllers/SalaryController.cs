
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkMateBE.Interfaces;
using WorkMateBE.Responses;

namespace WorkMateBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaryController : Controller
    {
        private readonly ISalaryRepository _salaryRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAccountRepository _accountRepository;

        public SalaryController(ISalaryRepository salaryRepository, IEmployeeRepository employeeRepository, IAccountRepository accountRepository)
        {
            _salaryRepository = salaryRepository;
            _employeeRepository = employeeRepository;
            _accountRepository = accountRepository;
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult CreateSalary([FromQuery] int employeeId, [FromQuery] int month, [FromQuery] int year)
        {
            if(_employeeRepository.GetEmployeeById(employeeId) == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Employee ID not found",
                    Data = null
                });
            }
            if(_salaryRepository.GetSalary(employeeId, month, year) != null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "This payroll has been created",
                    Data = null
                });
            }
            DateTime now = DateTime.Now;
            DateTime input = new DateTime(year, month, 1).AddMonths(1);
            if(now < input)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Time invalid",
                    Data = null
                });
            }
            if (!_salaryRepository.CreateSalarySheet(employeeId, month, year))
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Something went wrong",
                    Data = null
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Create Salary Sheet success",
                Data = null
            }); 
        }

        [HttpGet("{employeeId}")]
        public IActionResult GetSalary (int employeeId)
        {
            var account = _accountRepository.GetAccountByEmployeeId(employeeId);
            
            if (account == null)
            {
                return NotFound();
            }
            if (GetRoleFromToken() != 1 && GetAccountIdFromToken() != account.Id)
            {
                return Forbid();
            }
            var salaries = _salaryRepository.GetSalarySheet(employeeId);
            
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Salary Sheet success",
                Data = salaries
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
