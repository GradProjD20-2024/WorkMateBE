using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkMateBE.Dtos.DepartmentDto;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;
using WorkMateBE.Responses;

namespace WorkMateBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        // GET: api/department
        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            var departments = _departmentRepository.GetAll();
            var departmentDtos = _mapper.Map<List<DepartmentGetDto>>(departments);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Department List success",
                Data = departmentDtos
            });
        }

        // GET: api/department/{id}
        [HttpGet("{id}")]
        public IActionResult GetDepartmentById(int id)
        {
            var department = _departmentRepository.GetDepartmentById(id);
            if (department == null)
                return NotFound(new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Department ID not exists",
                    Data = null
                });
            var departmentDto = _mapper.Map<DepartmentGetDto>(department);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Department List success",
                Data = departmentDto
            });
        }

        // POST: api/department
        /*[Authorize(Roles = "Admin")]*/
        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult CreateDepartment([FromBody] DepartmentCreateDto departmentDto)
        {
            if (departmentDto == null)
                return BadRequest(ModelState);
            var department = _mapper.Map<Department>(departmentDto);
            if (!_departmentRepository.CreateDepartment(department))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(new ApiResponse
            {
                StatusCode = 201,
                Message = "Create Department Success",
                Data = department
            });
        }

        // PUT: api/department/{id}
        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, [FromBody] DepartmentCreateDto departmentDto)
        {
            if (departmentDto == null)
                return BadRequest(ModelState);

            var existingDepartment = _departmentRepository.GetDepartmentById(id);
            if (existingDepartment == null)
                return NotFound(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Department ID not found",
                    Data = null
                });
            var department = _mapper.Map<Department>(departmentDto);
            if (!_departmentRepository.UpdateDepartment(id, department))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Update Department Success",
                Data = null
            });
        }
        
        // DELETE: api/department/{id}
        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var existingDepartment = _departmentRepository.GetDepartmentById(id);
            if (existingDepartment == null)
                return NotFound(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Department ID not found",
                    Data = null
                });

            if (!_departmentRepository.DeleteDepartment(id))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Delete Department Success",
                Data = null
            });
        }
    }
}
