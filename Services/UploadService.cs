using System.Diagnostics;

namespace ProveedorVideo.Services
{
    public class UploadService
    {
        public void ProcessVideo(string filePath)
        {
            // Ensure FFmpeg is installed and accessible
            var ffmpegPath = Path.Combine(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()), "FFmpeg", "bin", "ffmpeg"); // Adjust if necessary

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
                    Arguments = $"-i \"{filePath}\" -map 0 -c:v libx264 -crf 18 -preset slow -c:a aac -b:a 192k -f dash \"{outputDirectory}/output.mpd\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Console.WriteLine(args.Data); // Log output if needed
                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Console.Error.WriteLine(args.Data); // Log error if needed
                }
            };

            process.Start();

            // Start async read
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Wait for the process to exit
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"FFmpeg process failed. Check the logs for more information.");
            }
        }

        public void GenerateThumbnail(string videoPath, TimeSpan time, string thumbnailPath)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i \"{videoPath}\" -ss {time.TotalSeconds} -vframes 1 \"{thumbnailPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                // Manejar errores si es necesario
            }
        }
    }
}
