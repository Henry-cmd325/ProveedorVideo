using Microsoft.AspNetCore.Mvc;
using ProveedorVideo.Models;
using ProveedorVideo.Services;

namespace ProveedorVideo.Controllers
{
    [ApiController]
    [Route("videos")]
    public class VideoController : ControllerBase
    {
        
        private readonly VideoService _service;

        public VideoController(VideoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllVideos()
        {
            return Ok(_service.GetAllVideos());
        }
    }
}
