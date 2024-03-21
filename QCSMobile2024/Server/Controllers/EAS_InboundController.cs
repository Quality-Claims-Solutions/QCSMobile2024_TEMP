using log4net;
using Microsoft.AspNetCore.Mvc;
using QCSMobile2024.Shared.Models;
using QCSMobile2024.Shared.Models.EntityModels;
using System.Runtime.CompilerServices;

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

        static string MethodName([CallerMemberName] string name = null) => name;

        [HttpPost]
        public async Task<IActionResult> CreateEASInbound(EAS_Inbound inbound)
        {
            Log.Info($"EAS_InboundController_{MethodName()} START: Creating EAS Inbound with Id: {inbound.Id}");

            _db.EAS_Inbound.Add(inbound);
            await _db.SaveChangesAsync();

            Log.Info($"EAS_InboundController_{MethodName()} RETURN: Successfully inserted EAS Inbound with Id: {inbound.Id}.");
            return Ok(inbound);
        }
    }
}
