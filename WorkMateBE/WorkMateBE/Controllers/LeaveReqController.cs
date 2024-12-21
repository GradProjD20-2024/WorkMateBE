using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkMateBE.Dtos.LeaveRequestDto;
using WorkMateBE.Interfaces;
using WorkMateBE.Responses;

namespace WorkMateBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaveReqController : Controller
    {
        private readonly ILeaveReqRepository _leaveReq;

        public LeaveReqController(ILeaveReqRepository leaveReq)
        {
            _leaveReq = leaveReq;
        }

        [HttpGet]
        public IActionResult GetRequest()
        {
            if(GetRoleFromToken() != 1 && GetRoleFromToken() != 2)
            {
                return Forbid();
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Success",
                Data = _leaveReq.GetAllRequest()
            }
            );
        }
        [HttpPost]
        public IActionResult CreateLeaveRequest(PostRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_leaveReq.CreateLeaveRequest(GetAccountIdFromToken(), model) == -1)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Something went wrong",
                    Data = null
                }
                );
            }
            if (_leaveReq.CreateLeaveRequest(GetAccountIdFromToken(), model) == -2)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Invalid Date!",
                    Data = null
                }
                );
            }

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Create Leave Request Success",
                Data = null
            }
            );
        }

        [HttpPut("approve/{id}")]
        public IActionResult ApproveRequest (int id)
        {
            if (GetRoleFromToken() != 1 && GetRoleFromToken() != 2)
            {
                return Forbid();
            }
            if (!_leaveReq.Approve(id))
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Something went wrong",
                    Data = null
                }
                );
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Approve Request Success",
                Data = null
            }
            );

        }
        [HttpPut("reject/{id}")]
        public IActionResult RejectRequest(int id)
        {
            if(GetRoleFromToken() != 1 && GetRoleFromToken() != 2)
            {
                return Forbid();
            }
            if (!_leaveReq.Reject(id))
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Something went wrong",
                    Data = null
                }
                );
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Reject Request Success",
                Data = null
            }
            );
        }
        [HttpGet("GetRequestByEmployee")]
        public IActionResult GetRequestByEmployee()
        {
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Success",
                Data= _leaveReq.GetLeaveRequestByAccountId(GetAccountIdFromToken())
            }
            );
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteRequest(int id)
        {
            var result = _leaveReq.DeleteRequest(id, GetAccountIdFromToken());
            if (result == -2)
            {
                return Forbid();
            }
            else if(result == -1)
            {
                return BadRequest();
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Delete Request Success",
                Data = null
            }
            );
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
