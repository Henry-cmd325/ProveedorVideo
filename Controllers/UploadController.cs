using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProveedorVideo.Services;

namespace ProveedorVideo.Controllers
{
    [Route("uploads")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly string _uploadDirectory = Path.Combine(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()), "Sytec", "publicaciones", "dashServer");
        private readonly UploadService _service;

        public UploadController(UploadService service)
        {
            if (!Directory.Exists(_uploadDirectory)) 
                Directory.CreateDirectory(_uploadDirectory);

            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded");

                var filePath = Path.Combine(_uploadDirectory, file.FileName);

                // Save the uploaded file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Process the file (e.g., segment and create MPD)
                BackgroundJob.Enqueue(() => _service.ProcessVideo(filePath));

                return Ok(new { FilePath = filePath });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
