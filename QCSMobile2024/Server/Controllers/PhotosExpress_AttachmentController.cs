using AutoMapper;
using log4net;
using Microsoft.AspNetCore.Mvc;
using QCSMobile2024.Shared.Models;
using QCSMobile2024.Shared.Models.EntityModels;
using QCSMobile2024.Shared.Models.ViewModels;
using System.Runtime.CompilerServices;

namespace QCSMobile2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosExpress_AttachmentController : Controller
    {
        private readonly IMapper _mapper;

        private Db _db;
        private readonly ILog Log;

        static string MethodName([CallerMemberName] string name = null) => name;

        public PhotosExpress_AttachmentController(Db db, IMapper mapper, ILog logger)
        {
            _db = db;
            _mapper = mapper;
            Log = logger;

        }

        [HttpPost]
        public async Task<ActionResult> Post(List<FileAttachmentViewModel> images)
        {
            Log.Info($"PhotosExpress_AttachmentController_{MethodName()} START: Posting Attachments.");
            try
            {
                var photosExpresssAttachment =  _mapper.Map<List<PhotosExpress_Attachment>>( images );
                _db.PhotosExpress_Attachment.AddRange(photosExpresssAttachment);

                await _db.SaveChangesAsync();

                Log.Info($"PhotosExpress_AttachmentController_{MethodName()} RETURN: Added PhotosExpressAttachment.");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error($"PhotosExpress_AttachmentController_{MethodName()} ERROR: We encountered an error in Posting PhotosExpress_Attachments. Exception: {ex.Message}");
                return BadRequest();
            }
        }
    }
}
