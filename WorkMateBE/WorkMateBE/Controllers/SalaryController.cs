
using Microsoft.AspNetCore.Mvc;
using WorkMateBE.Interfaces;
using WorkMateBE.Responses;

namespace WorkMateBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaryController : Controller
    {
        private readonly ISalaryRepository _salaryRepository;

        public SalaryController(ISalaryRepository salaryRepository)
        {
            _salaryRepository = salaryRepository;
        }
        [HttpPost]
        public IActionResult CreateSalary([FromQuery] int employeeId, [FromQuery] int month, [FromQuery] int year)
        {
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
            var salaries = _salaryRepository.GetSalarySheet(employeeId);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Salary Sheet success",
                Data = salaries
            });
        }

    }
}
