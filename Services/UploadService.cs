namespace ProveedorVideo.Services
{
    public class UploadService
    {
        public void ProcessVideo(string filePath)
        {
            // Ensure FFmpeg is installed and accessible
            var ffmpegPath = "ffmpeg"; // Adjust if necessary

            // Create a folder for segmented files
            var videoId = Path.GetFileNameWithoutExtension(filePath);
            var outputDirectory = Path.Combine(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()), "Sytec", "publicaciones", "dashServer", videoId);

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            // Run FFmpeg to segment the video and create MPD
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = $"-i \"{filePath}\" -map 0 -c:v libx264 -b:v 500k -c:a aac -b:a 128k -f dash \"{outputDirectory}/output.mpd\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            if (process.ExitCode != 0)
            {
                throw new Exception($"FFmpeg process failed: {error}");
            }
        }
    }
}
