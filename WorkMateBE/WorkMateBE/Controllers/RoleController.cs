using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WorkMateBE.Dtos.RoleDto;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;
using WorkMateBE.Responses;

namespace WorkMateBE.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
        [HttpPost]
        public IActionResult CreateRole([FromBody] NewRoleDto newRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Please complete new role",
                    Data = null
                });
            }
            var role = _mapper.Map<Role>(newRole);
            if (!_roleRepository.CreateRole(role))
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
                Message = "Create Role Success",
                Data = role
            });
        }
        [HttpGet]
        public IActionResult GetAllRole()
        {
            var roles = _roleRepository.GetRole();
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Create All Role Success",
                Data = roles
            });
        }

        [HttpPut]
        public IActionResult UpdateRole([FromQuery] int id, [FromBody] NewRoleDto updateRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Please complete update role",
                    Data = null
                });
            }
            if (_roleRepository.GetRoleById(id) == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Role ID not found",
                    Data = null
                });
            }
            var role = _mapper.Map<Role>(updateRole);
            if (!_roleRepository.UpdateRole(id, role))
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
                Message = "Update Role Success",
                Data = null
            });
        }

        [HttpDelete]
        public IActionResult DeleteRole([FromQuery] int id)
        {
            if (_roleRepository.GetRoleById(id) == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Role ID not found",
                    Data = null
                });
            }
            if (!_roleRepository.DeleteRole(id))
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
                Message = "Delete Role Success",
                Data = null
            });
        }
    }
}
