using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkMateBE.Dtos.AccountDto;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;
using WorkMateBE.Responses;

namespace WorkMateBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;

        public AccountController(IAccountRepository accountRepository, IMapper mapper, IEmployeeRepository employeeRepository)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        // GET: api/account
        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            var accounts = _accountRepository.GetAllAccounts();
            var accountDtos = _mapper.Map<List<AccountGetDto>>(accounts);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Account List success",
                Data = accountDtos
            });
        }


        // GET: api/account/{id}
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetAccountById(int id)
        {
            var currentUserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (currentUserRole == "1" || currentUserRole == "2" || currentUserId == id)
            {
                var account = _accountRepository.GetAccountById(id);
                if (account == null)
                {
                    return NotFound(new ApiResponse
                    {
                        StatusCode = 404,
                        Message = "Account ID not exists",
                        Data = null
                    });
                }

                var accountDto = _mapper.Map<AccountGetDto>(account);
                return Ok(new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Get Account success",
                    Data = accountDto
                });
            }

            // Nếu không có quyền truy cập
            return Forbid();
        }
        [Authorize]
        [HttpGet("GetAccountByEmployeeId/{id}")]
        public IActionResult GetAccountByEmployeeId(int id)
        {


            var account = _accountRepository.GetAccountByEmployeeId(id);
            if (GetAccountIdFromToken() != account.Id)
            {
                return Forbid();
            }
            if (account == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Account ID not exists",
                    Data = null
                });
            }

            var accountDto = _mapper.Map<AccountGetDto>(account);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Account success",
                Data = accountDto
            });
        }


        // POST: api/account
        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult CreateAccount([FromBody] AccountCreateDto accountDto)
        {
            if (accountDto == null)
                return BadRequest(ModelState);
            if (_employeeRepository.GetEmployeeById(accountDto.EmployeeId) == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "The employee not found",
                    Data = null
                });
            }
            if (_accountRepository.CheckEmployee(accountDto.EmployeeId))
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "The employee already has an account",
                    Data = null
                });
            }

            // Kiểm tra email đã tồn tại
            if (_accountRepository.CheckEmail(accountDto.Email))
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Email already exists",
                    Data = null
                });
            }

            var account = _mapper.Map<Account>(accountDto);
            if (!_accountRepository.CreateAccount(account))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(400, ModelState);
            }

            return Ok(new ApiResponse
            {
                StatusCode = 201,
                Message = "Create Account Success",
                Data = account
            });
        }

        // PUT: api/account/{id}
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateAccount(int id, [FromBody] AccountUpdateDto accountDto)
        {
            if (accountDto == null)
                return BadRequest(ModelState);

            var existingAccount = _accountRepository.GetAccountById(id);           
            
            if (existingAccount == null)
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Account ID not found",
                    Data = null
                });
            if (GetRoleFromToken() != 1)
            {
                if (id != GetAccountIdFromToken())
                {
                    return Forbid();
                }
            }
            if (_accountRepository.GetAccountByEmail(accountDto.Email) != null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Email exists",
                    Data = null
                });
            }
            var account = _mapper.Map<Account>(accountDto);
            if (!_accountRepository.UpdateAccount(id, account))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Update Account Success",
                Data = null
            });
        }

        // DELETE: api/account/{id}
        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            var existingAccount = _accountRepository.GetAccountById(id);
            if (existingAccount == null)
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Account ID not found",
                    Data = null
                });

            if (!_accountRepository.DeleteAccount(id))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Delete Account Success",
                Data = null
            });
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] AccountLogin accountLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_accountRepository.Login(accountLogin))
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Email or password is incorrect",
                    Data = null
                });
            }
            var account = _accountRepository.GetAccountByEmail(accountLogin.Email);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Login success",
                Data = GenerateJwtToken(account)
            });
        }
        [Authorize]
        [HttpPost("change-password/{accountId}")]
        public IActionResult ChangePassword([FromBody] AccountChangePw pass, int accountId)
        {
            var currentUserId = GetAccountIdFromToken();
            // Kiểm tra quyền: chỉ cho phép cập nhật nếu accountId khớp với post.AccountId
            if (accountId != currentUserId)
            {
                return Forbid();
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = _accountRepository.GetAccountById(accountId);
            if (!BCrypt.Net.BCrypt.Verify(pass.OldPassword, account.Password))
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Old password is incorrect",
                    Data = null
                });

            }
            if (pass.NewPassword != pass.ConfirmPassword)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Password and Confirm Password are not same",
                    Data = null
                });
            }
            if (!_accountRepository.ChangePassword(accountId, pass.NewPassword))
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Something went wrong while change password",
                    Data = null
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Change password success",
                Data = null
            });
        }



        [Authorize(Roles = "1")]
        [HttpPost("reset-password/{accountId}")]
        public IActionResult ResetPassword(int accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_accountRepository.GetAccountById(accountId) == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Account ID not found",
                    Data = null
                });
            }
            var newPassword = _accountRepository.ResetPassword(accountId);
            if (newPassword == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Something went wrong",
                    Data = null
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Reset password success",
                Data = newPassword
            });

        }

        private string GenerateJwtToken(Account account)
        {
            // Tạo các claims chứa thông tin role và id
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim("Id", account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.RoleId.ToString()),
                new Claim("Role", account.RoleId.ToString()),
                new Claim("EmployeeId", account.EmployeeId.ToString())
    };

            // Tạo khóa bảo mật
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("your_longer_secret_key_of_at_least_32_bytes"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // Tạo token
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: creds);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
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
    }
}
 