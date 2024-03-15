using AutoMapper;
using log4net;
using Microsoft.AspNetCore.Mvc;
using QCSMobile2024.Shared.Models;
using QCSMobile2024.Shared.Models.CustomModels;
using SixLabors.ImageSharp;
using System.Runtime.CompilerServices;

namespace QCSMobile2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILog Log;

        public FileController(ILog logger)
         { 
            Log = logger;
        }
        static string MethodName([CallerMemberName] string name = null) => name;

        [HttpPost("FnolAttachment")]
        public async Task<ActionResult<List<UploadResult>>> UploadFnolFile(List<IFormFile> files)
        {
            Log.Info($"FileController{MethodName()}:START. Uploading {files.Count} files");

            List<UploadResult> result = new List<UploadResult>();
            string? filePath;
            bool uploadStatus;

            foreach (var file in files)
            {
                filePath = Path.Combine($"\\\\192.168.29.94\\qcs_Files\\Fnol_Vehicle", file.FileName);
                uploadStatus = false;

                try
                {
                    await using FileStream fs = new(filePath, FileMode.Create);
                    await file.CopyToAsync(fs);

                    uploadStatus = true;
                }
                catch (Exception ex)
                {
                    Log.Error($"We encountered an error uploading file {file.FileName} to {filePath}. Exception: {ex.Message}.");
                    uploadStatus = false;
                }
                finally
                {
                    result.Add(new UploadResult()
                    {
                        Uploaded = uploadStatus,
                        FileName = file.FileName,
                        StoredFileName = filePath,
                        ErrorCode = 0
                    });
                }
            }
            Log.Info($"FileController{MethodName()}:END. Uploaded {result.Count} files");

            return Ok(result);
        }

        [HttpPost("PhotosExpressAttachment")]
        public async Task<ActionResult<List<UploadResult>>> UploadPhotosExpressFile(List<IFormFile> files)
        {
            Log.Info($"FileController{MethodName()}:START. Uploading {files.Count} files");

            List<UploadResult> result = new List<UploadResult>();
            string? filePath;
            bool uploadStatus;

            foreach (var file in files)
            {
                filePath = Path.Combine($"\\\\192.168.29.94\\qcs_Files\\PhotosExpress", file.FileName);
                uploadStatus = false;

                try
                {
                    await using FileStream fs = new(filePath, FileMode.Create);
                    await file.CopyToAsync(fs);

                    uploadStatus = true;
                }
                catch (Exception ex)
                {
                    Log.Error($"We encountered an error uploading file {file.FileName} to {filePath}. Exception: {ex.Message}.");
                    uploadStatus = false;
                }
                finally
                {
                    result.Add(new UploadResult()
                    {
                        Uploaded = uploadStatus,
                        FileName = file.FileName,
                        StoredFileName = filePath,
                        ErrorCode = 0
                    });
                }
            }
            Log.Info($"FileController{MethodName()}:END. Uploaded {result.Count} files");

            return Ok(result);
        }


        [HttpGet("{filePath}")]
        public async Task<IActionResult> DownloadFile(string filePath)
        {
            Log.Info($"FileController{MethodName()}:START.");

            Image image;

            // Get around passing slashes in the URL
            filePath = filePath.Replace("__", "\\");

            var fullPath = Path.Combine($"\\\\192.168.29.94\\qcs_Files", filePath);

            var memoryStream = new MemoryStream();
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
            }

            memoryStream.Position = 0;
            Log.Info($"FileController{MethodName()}:END.");

            return File(memoryStream, "application/octet-stream", Path.GetFileName(fullPath));
        }
    }
}
