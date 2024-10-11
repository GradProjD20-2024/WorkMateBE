using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult GetAccountById(int id)
        {
            var account = _accountRepository.GetAccountById(id);
            if (account == null)
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Account ID not exists",
                    Data = null
                });
            var accountDto = _mapper.Map<AccountGetDto>(account);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Account success",
                Data = accountDto
            });
        }

        // POST: api/account
        [HttpPost]
        public IActionResult CreateAccount([FromBody] AccountCreateDto accountDto)
        {
            if (accountDto == null)
                return BadRequest(ModelState);
            if (_employeeRepository.GetEmployeeById(accountDto.EmployeeId)==null)
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
                return StatusCode(500, ModelState);
            }

            return Ok(new ApiResponse
            {
                StatusCode = 201,
                Message = "Create Account Success",
                Data = account
            });
        }

        // PUT: api/account/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateAccount(int id, [FromBody] AccountCreateDto accountDto)
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
        public IActionResult Login([FromBody]AccountLogin accountLogin)
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
        private string GenerateJwtToken(Account account)
        {
            // Tạo các claims chứa thông tin role và id
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.Role.ToString())
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

    }
}
