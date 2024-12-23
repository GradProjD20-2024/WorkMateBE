﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using WorkMateBE.Dtos.PostDto;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;
using WorkMateBE.Responses;

namespace WorkMateBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public PostController(IPostRepository postRepository, IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _accountRepository = accountRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        // GET: api/Post
        [HttpGet]
        public IActionResult GetPosts()
        {
            var posts = _postRepository.GetPosts();
            var postGetList = new List<PostGetDto>();
            
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Posts Success",
                Data = posts
            });
            
        }

        // GET: api/Post/{postId}
        [HttpGet("{postId}")]
        public IActionResult GetPostById(int postId)
        {
            var post = _postRepository.GetPostById(postId);
            if (post == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Post ID not found",
                    Data = null
                });
            }
            
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Post success",
                Data = post
            });
        }

        // GET: api/Post/Account/{accountId}
        [HttpGet("Account/{accountId}")]
        public IActionResult GetPostsByAccountId(int accountId)
        {
            var posts = _postRepository.GetPostByAccountId(accountId);
            if (posts == null || !posts.Any())
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "No posts found for the given Account ID",
                    Data = null
                });
            }

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get Posts by Account ID success",
                Data = posts
            });
        }

        // POST: api/Post
        [HttpPost]
        public IActionResult CreatePost([FromBody] PostCreateDto postCreate)
        {
            var accountId = GetAccountIdFromToken();          
            if (postCreate == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Post data is null",
                    Data = null
                });
            }          
            if (!_postRepository.CreatePost(accountId, postCreate.Content, postCreate.ImageUrl))
            {
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "An error occurred while creating the post",
                    Data = null
                });
            }


            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Create Post successfully",
                Data = null
            });
        }

        // PUT: api/Post/{postId}
        [HttpPut("{postId}")]
        public IActionResult UpdatePost(int postId, [FromBody] PostUpdateDto postUpdate)
        {
            try
            {
                // Lấy bài viết cần cập nhật
                var post = _postRepository.GetPostById(postId);
                if (post == null)
                {
                    return NotFound(new ApiResponse
                    {
                        StatusCode = 404,
                        Message = $"Post with ID {postId} not found",
                        Data = null
                    });
                }

                // Lấy accountId từ token
                var accountId = GetAccountIdFromToken();

                // Kiểm tra quyền: chỉ cho phép cập nhật nếu accountId khớp với post.AccountId
                if (accountId != post.AccountId)
                {
                    return Forbid();
                }

                // Kiểm tra dữ liệu cập nhật
                if (postUpdate == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        StatusCode = 400,
                        Message = "Post data is invalid or does not match the ID",
                        Data = null
                    });
                }

                // Map dữ liệu từ DTO sang model
                var updatedPost = _mapper.Map<Post>(postUpdate);
                updatedPost.Id = postId; // Đảm bảo giữ nguyên ID bài viết cũ
                updatedPost.AccountId = post.AccountId; // Không cho phép thay đổi AccountId

                // Cập nhật bài viết
                if (!_postRepository.UpdatePost(postId, updatedPost))
                {
                    return StatusCode(500, new ApiResponse
                    {
                        StatusCode = 500,
                        Message = "An error occurred while updating the post",
                        Data = null
                    });
                }

                return Ok(new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Post updated successfully",
                    Data = updatedPost
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse
                {
                    StatusCode = 401,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "An unexpected error occurred",
                    Data = ex.Message
                });
            }
        }

        // DELETE: api/Post/{postId}
        [HttpDelete("{postId}")]
        public IActionResult DeletePost(int postId)
        {
            var existingPost = _postRepository.GetPostById(postId);

            if (existingPost == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = $"Post with ID {postId} not found",
                    Data = null
                });
            }
            var accountId = GetAccountIdFromToken();

            if (accountId != existingPost.AccountId && GetRoleFromToken() != 1)
            {
                return Forbid();
            }
            if (!_postRepository.DeletePost(postId))
            {
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "An error occurred while deleting the post",
                    Data = null
                });
            }

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Post deleted successfully",
                Data = null
            });
        }
        private string GetEmployeeName(int accountId)
        {
            var account = _accountRepository.GetAccountById(accountId);
            var employee = _employeeRepository.GetEmployeeById(account.EmployeeId);
            return employee.FullName;
            
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
