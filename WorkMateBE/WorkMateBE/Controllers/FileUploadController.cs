using Microsoft.AspNetCore.Mvc;

namespace WorkMateBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly string _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        public FileUploadController()
        {
            // Tạo thư mục lưu trữ nếu chưa tồn tại
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        /// <summary>
        /// Upload file ảnh và trả về URL của ảnh
        /// </summary>
        /// <param name="file">File ảnh được gửi trong request</param>
        /// <returns>URL của file trên localhost</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded or file is empty.");
            }

            try
            {
                // Tạo tên file duy nhất
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_uploadsFolder, uniqueFileName);

                // Lưu file vào server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Tạo URL trả về
                var fileUrl = $"{Request.Scheme}://{Request.Host}/images/{uniqueFileName}";
                return Ok(new { url = fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
