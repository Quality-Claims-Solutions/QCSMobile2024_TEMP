using log4net;
using Microsoft.AspNetCore.Mvc;
using QCSMobile2024.Shared.Models;
using QCSMobile2024.Shared.Models.EntityModels;
using System.Runtime.CompilerServices;

namespace QCSMobile2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosExpressController: Controller
    {
        private Db _db;
        private readonly ILog Log;

        static string MethodName([CallerMemberName] string name = null) => name;

        public PhotosExpressController(Db db, ILog logger)
        {
            _db = db;
            Log = logger;

        }

        [HttpPost]
        public async Task<ActionResult> Post(PhotosExpress photosExpress)
        {
            Log.Info($"PhotosExpressController_{MethodName()} START: Posting PhotosExpress with id: {photosExpress.PhotosExpressID}.");
            try
            {
                _db.PhotosExpress.Add(photosExpress);
                await _db.SaveChangesAsync();

                Log.Info($"PhotosExpressController_{MethodName()} RETURN: Added PhotosExpress with Id: {photosExpress.PhotosExpressID}.");
                return Ok(photosExpress.PhotosExpressID);
            }
            catch (Exception ex)
            {
                Log.Error($"PhotosExpressController_{MethodName()} ERROR: We encountered an error in Posting the PhotosExpress with id: {photosExpress.PhotosExpressID}. Exception: {ex.Message}");
                return BadRequest();
            }
        }
    }
}
