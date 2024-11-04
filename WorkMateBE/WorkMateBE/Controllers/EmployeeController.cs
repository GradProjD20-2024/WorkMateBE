using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkMateBE.Dtos.EmployeeDto;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;
using WorkMateBE.Responses;

namespace WorkMateBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _departmentRepository = departmentRepository;
        }

        // GET: api/employee
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = _employeeRepository.GetAll();
            var employeeDtos = _mapper.Map<List<EmployeeGetDto>>(employees);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Employee List success",
                Data = employeeDtos
            });
        }

        // GET: api/employee/{id}
        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _employeeRepository.GetEmployeeById(id);
            if (employee == null)
                return NotFound(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Employee ID not exists",
                    Data = null
                });

            var employeeDto = _mapper.Map<EmployeeGetDto>(employee);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Employee Success",
                Data = employeeDto
            });
        }

        // POST: api/employee
        [HttpPost]
        public IActionResult CreateEmployee([FromBody] EmployeeCreateDto employeeDto)
        {
            if (employeeDto == null)
                return BadRequest(ModelState);

            var employee = _mapper.Map<Employee>(employeeDto);
            if(_departmentRepository.GetDepartmentById(employee.DepartmentId) == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Department Not Exist",
                    Data = null
                });
            }
            if (!_employeeRepository.CreateEmployee(employee))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(new ApiResponse
            {
                StatusCode = 201,
                Message = "Create Employee Success",
                Data = employee
            });
        }

        // PUT: api/employee/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] EmployeeCreateDto employeeDto)
        {
            if (employeeDto == null)
                return BadRequest(ModelState);

            var existingEmployee = _employeeRepository.GetEmployeeById(id);
            if (existingEmployee == null)
                return NotFound(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Employee ID not found",
                    Data = null
                });

            var employee = _mapper.Map<Employee>(employeeDto);
            if (!_employeeRepository.UpdateEmployee(id, employee))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Update Employee Success",
                Data = null
            });
        }
        [Authorize(Roles = "1")]
        [HttpGet("department/{departmentId}")]
        public IActionResult GetEmployeesByDeparment(int departmentId)
        {
            if (_departmentRepository.GetDepartmentById(departmentId) == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Department Not Exist",
                    Data = null
                });
            }
            var employees = _employeeRepository.GetEmployeesByDepartment(departmentId);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Employees success",
                Data = employees
            });
        }
        // DELETE: api/employee/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var existingEmployee = _employeeRepository.GetEmployeeById(id);
            if (existingEmployee == null)
                return NotFound(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Employee ID not found",
                    Data = null
                });

            if (!_employeeRepository.DeleteEmployee(id))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Delete Employee Success",
                Data = null
            });
        }
    }
}
