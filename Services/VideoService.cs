using ProveedorVideo.Models;

namespace ProveedorVideo.Services
{
    public class VideoService
    {
        private readonly string _videoDirectory = Path.Combine(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()), "Sytec", "publicaciones", "dashServer");

        public List<Video> GetAllVideos()
        {
            List<Video> videos = [];

            string[] folders = Directory.GetDirectories(_videoDirectory);

            foreach (var folder in folders)
            {
                var folderName = Path.GetFileName(folder);
                videos.Add(new()
                {
                    Nombre = folderName,
                    Url = $"http://192.168.250.233:80/{folderName}/output.mpd"
                });
            }

            return videos;
        }
    }
}
