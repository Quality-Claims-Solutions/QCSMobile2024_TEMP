using QCSMobile2024.Shared.Models.ViewModels;
using Serilog;
using System.Runtime.CompilerServices;

namespace QCSMobile2024.Shared.Services
{
    public class FileService
    {
        public HttpClient _http { get; }

        public FileService(HttpClient http)
        {
            _http = http;
        }

        static string MethodName([CallerMemberName] string name = null) => name;

        public async Task<Stream> DownloadFile(FileAttachmentViewModel file, string claimType)
        {
            Log.Information($"FileService_{MethodName()}: Start.");

            if(!file.Path.Contains(claimType+"/"))
            {
                file.Path = Path.Combine(claimType, file.Path);
            }

            // Get around passing slashes in the URL
            var cleanedFilePath = file.Path.Replace("\\", "__").Replace("/", "__");
            var response = await _http.GetAsync($"api/file/{cleanedFilePath}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                return stream;
            }

            Log.Information($"FileService_{MethodName()}: END.");

            return null;
        }
    }
}
