﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;
using WorkMateBE.Responses;

namespace WorkMateBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        // GET: api/Comment/Post/{postId}
        [HttpGet("Post/{postId}")]
        public IActionResult GetCommentsByPostId(int postId)
        {
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

            var comments = _commentRepository.GetCommentByPostId(postId);

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Get comments by post success",
                Data = comments
            });
        }

        // POST: api/Comment
        [HttpPost]
        public IActionResult CreateComment([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Comment data is null",
                    Data = null
                });
            }

            var post = _postRepository.GetPostById(comment.PostId);
            if (post == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = $"Post with ID {comment.PostId} not found",
                    Data = null
                });
            }

            comment.AccountId = GetAccountIdFromToken(); // Lấy AccountId từ token
            comment.CreatedAt = DateTime.UtcNow;

            if (!_commentRepository.CreateComment(comment))
            {
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "An error occurred while creating the comment",
                    Data = null
                });
            }

            return Ok(new ApiResponse
            {
                StatusCode = 201,
                Message = "Comment created successfully",
                Data = comment
            });
        }

        // DELETE: api/Comment/{commentId}
        [HttpDelete("{commentId}")]
        public IActionResult DeleteComment(int commentId)
        {
            var comment = _commentRepository.GetCommentById(commentId);
            if (comment == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = $"Comment with ID {commentId} not found",
                    Data = null
                });
            }

            var accountId = GetAccountIdFromToken();
            if (accountId != comment.AccountId)
            {
                return Forbid();
            }

            if (!_commentRepository.DeleteComment(commentId))
            {
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "An error occurred while deleting the comment",
                    Data = null
                });
            }

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Comment deleted successfully",
                Data = null
            });
        }

        // Helper: Lấy AccountId từ token
        private int GetAccountIdFromToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var accountIdClaim = identity.FindFirst("AccountId");
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
    }
}
