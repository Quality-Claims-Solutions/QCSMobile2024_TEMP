using log4net;
using Microsoft.AspNetCore.Mvc;
using QCSMobile2024.Shared.Models;
using QCSMobile2024.Shared.Models.EntityModels;

namespace QCSMobile2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EAS_InboundController : ControllerBase
    {
        private readonly Db _db;
        private readonly ILog Log;


        public EAS_InboundController(Db db, ILog logger)
        {
            _db = db;
            Log = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEASInbound(EAS_Inbound inbound)
        {
            Log.Info($"START: Create EAS Inbound.");

            _db.EAS_Inbound.Add(inbound);
            await _db.SaveChangesAsync();

            Log.Info($"RETURN: Successfully inserted EAS Inbound {inbound.Id}.");
            return Ok(inbound);
        }
    }
}
