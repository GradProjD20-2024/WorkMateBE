
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
            DateTime now = DateTime.Now;
            DateTime input = new DateTime(year, month, 1).AddMonths(1);
            if(now < input)
            {
                return Ok(new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Time invalid",
                    Data = null
                });
            }
            if (!_salaryRepository.CreateSalarySheet(employeeId, month, year))
            {
                return BadRequest("Something went wrong");
            }
            return Ok("Create Salary success");
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
