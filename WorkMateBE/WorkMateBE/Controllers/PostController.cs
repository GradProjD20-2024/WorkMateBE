using Microsoft.AspNetCore.Mvc;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;
using WorkMateBE.Responses;

namespace WorkMateBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        // GET: api/Post
        [HttpGet]
        public IActionResult GetPosts()
        {
            var posts = _postRepository.GetPosts();
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
        public IActionResult CreatePost([FromBody] Post post)
        {
            if (post == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Post data is null",
                    Data = null
                });
            }

            if (!_postRepository.CreatPost(post))
            {
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "An error occurred while creating the post",
                    Data = null
                });
            }

            if (!_postRepository.Saved())
            {
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "An error occurred while saving the post",
                    Data = null
                });
            }

            return CreatedAtAction(nameof(GetPostById), new { postId = post.Id }, new ApiResponse
            {
                StatusCode = 201,
                Message = "Post created successfully",
                Data = post
            });
        }

        // PUT: api/Post/{postId}
        [HttpPut("{postId}")]
        public IActionResult UpdatePost(int postId, [FromBody] Post post)
        {
            if (post == null || post.Id != postId)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Post data is invalid or does not match the ID",
                    Data = null
                });
            }

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

            if (!_postRepository.UpdatePost(post))
            {
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "An error occurred while updating the post",
                    Data = null
                });
            }

            if (!_postRepository.Saved())
            {
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "An error occurred while saving the updated post",
                    Data = null
                });
            }

            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = "Post updated successfully",
                Data = post
            });
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

            if (!_postRepository.DeletePost(postId))
            {
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "An error occurred while deleting the post",
                    Data = null
                });
            }

            if (!_postRepository.Saved())
            {
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "An error occurred while saving changes after deletion",
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
    }
}
