using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QCSMobile2024.Shared.Models;
using QCSMobile2024.Shared.Models.EntityModels;
using System.Runtime.CompilerServices;

namespace QCSMobile2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Fnol_AttachmentsController : ControllerBase
    {
        private Db _db;
        private readonly ILog Log;
        static string MethodName([CallerMemberName] string name = null) => name;

        public Fnol_AttachmentsController(Db db, ILog logger)
        {
            _db = db;
            Log = logger;

        }

        [HttpGet("{fnolId}")]
        public async Task<ActionResult> Get(decimal fnolId)
        {
            Log.Info($"Fnol_AttachmentsController_{MethodName()} START: Getting Fnol_Attachments.");

            try
            {
                List<Fnol_Attachments> results = await _db.Fnol_Attachments.Where(attachment => attachment.FnolID == fnolId).ToListAsync();
                Log.Info($"Fnol_AttachmentsController_{MethodName()} RETURN: Got {results.Count} Fnol_Attachments.");

                return Ok(results);
            }
            catch (Exception ex)
            {
                Log.Error($"Fnol_AttachmentsController_{MethodName()} ERROR: Error Getting Fnol_Attachments, Exception: {ex.Message}.");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(List<Fnol_Attachments> fnol_Attachments)
        {
            Log.Info($"Fnol_AttachmentsController_{MethodName()} START: Posting Fnol_Attachments.");

            if (fnol_Attachments.Count == 0)
            {
                Log.Info($"Fnol_AttachmentsController_{MethodName()} RETURN: Empty attachments list.");
                return BadRequest();
            }

            try
            {
                await _db.Fnol_Attachments.AddRangeAsync(fnol_Attachments);
                await _db.SaveChangesAsync();
                Log.Info($"Fnol_AttachmentsController_{MethodName()} RETURN: Added Fnol_Attachments to Fnol.");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error($"Fnol_AttachmentsController_{MethodName()} ERROR: Error Posting Fnol_Attachments, Exception: {ex.Message}.");
                return BadRequest();
            }
        }
    }
}
