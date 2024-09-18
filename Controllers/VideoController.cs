using Microsoft.AspNetCore.Mvc;

namespace ProveedorVideo.Controllers
{
    [ApiController]
    [Route("videos")]
    public class VideoController : ControllerBase
    {
        private readonly string _videoDirectory = Path.Combine(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()), "Sytec", "publicaciones", "dashServer");

        [HttpGet("manifest/{videoId}")]
        public IActionResult GetManifest(string videoId)
        {
            var manifestPath = Path.Combine(_videoDirectory, videoId, "output.mpd");
            if (!System.IO.File.Exists(manifestPath))
                return NotFound();

            var stream = new FileStream(manifestPath, FileMode.Open, FileAccess.Read);
            return File(stream, "application/dash+xml");
        }

        [HttpGet("/init-stream{videoId}.m4s")]
        public IActionResult GetInitializationSegment(string videoId, string fileName)
        {
            var segmentPath = Path.Combine(_videoDirectory, videoId, fileName);
            if (!System.IO.File.Exists(segmentPath))
                return NotFound();

            var stream = new FileStream(segmentPath, FileMode.Open, FileAccess.Read);
            return File(stream, "video/mp4");
        }

        [HttpGet("/chunk-stream{videoId}-{fileName}.m4s")]
        public IActionResult GetSegment(string videoId, string fileName)
        {
            var segmentPath = Path.Combine(_videoDirectory, videoId, fileName);
            if (!System.IO.File.Exists(segmentPath))
                return NotFound();

            var stream = new FileStream(segmentPath, FileMode.Open, FileAccess.Read);
            return File(stream, "video/mp4");
        }
    }
}
