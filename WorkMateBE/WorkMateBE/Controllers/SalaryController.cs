
using Microsoft.AspNetCore.Mvc;
using WorkMateBE.Interfaces;

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
            if(!_salaryRepository.CreateSalarySheet(employeeId, month, year))
            {
                return BadRequest("Something went wrong");
            }
            return Ok("Create Salary success");
        }
    }
}
