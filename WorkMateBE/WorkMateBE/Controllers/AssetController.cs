using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;
using WorkMateBE.Responses;
using WorkMateBE.Dtos.AssetDto;
using Microsoft.AspNetCore.Authorization;

namespace WorkMateBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetController : ControllerBase
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;

        public AssetController(IAssetRepository assetRepository, IMapper mapper, IEmployeeRepository employeeRepository)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        // GET: api/asset
        [Authorize(Roles = "1,2")]
        [HttpGet]
        public IActionResult GetAllAssets()
        {
            var assets = _assetRepository.GetAll();
            var assetDtos = _mapper.Map<List<AssetGetDto>>(assets);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Asset List success",
                Data = assetDtos
            });
        }

        // GET: api/asset/{id}
        [Authorize(Roles = "1,2")]
        [HttpGet("{id}")]
        public IActionResult GetAssetById(int id)
        {
            var asset = _assetRepository.GetAssetById(id);
            if (asset == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Asset ID not found",
                    Data = null
                });
            }

            var assetDto = _mapper.Map<AssetGetDto>(asset);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Asset success",
                Data = assetDto
            });
        }

        // POST: api/asset
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public IActionResult CreateAsset([FromBody] AssetCreateDto assetDto)
        {
            if (assetDto == null)
                return BadRequest(ModelState);

            var asset = _mapper.Map<Asset>(assetDto);
            if (asset.EmployeeId.HasValue && _employeeRepository.GetEmployeeById(asset.EmployeeId.Value) == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Employee not found",
                    Data = null
                });
            }
            if (!_assetRepository.CreateAsset(asset))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            
            return Ok(new ApiResponse
            {
                StatusCode = 201,
                Message = "Create Asset Success",
                Data = asset
            });
        }

        // PUT: api/asset/{id}
        [Authorize(Roles = "1,2")]
        [HttpPut("{id}")]
        public IActionResult UpdateAsset(int id, [FromBody] AssetCreateDto assetDto)
        {
            if (assetDto == null)
                return BadRequest(ModelState);

            var existingAsset = _assetRepository.GetAssetById(id);

            if (existingAsset == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Asset ID not found",
                    Data = null
                });
            }

            var asset = _mapper.Map<Asset>(assetDto);
            if (asset.EmployeeId.HasValue && _employeeRepository.GetEmployeeById(asset.EmployeeId.Value) == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Employee not found",
                    Data = null
                });
            }
            if (!_assetRepository.UpdateAsset(id, asset))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Update Asset Success",
                Data = null
            });
        }

        // DELETE: api/asset/{id}
        [Authorize(Roles = "1,2")]
        [HttpDelete("{id}")]
        public IActionResult DeleteAsset(int id)
        {
            var existingAsset = _assetRepository.GetAssetById(id);
            if (existingAsset == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Asset ID not found",
                    Data = null
                });
            }

            if (!_assetRepository.DeleteAsset(id))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Delete Asset Success",
                Data = null
            });
        }
    }
}
