using log4net;
using Microsoft.AspNetCore.Mvc;
using QCSMobile2024.Server.Kafka;
using QCSMobile2024.Shared.Models.CustomModels;
using QCSMobile2024.Shared.Models.EntityModels;

namespace QCSMobile2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaController : ControllerBase
    {
        private readonly ILog Log;
        public KafkaController(ILog logger)
        {
            Log = logger;
        }
        [HttpPost]
        public async Task<ActionResult> PostKafkaUpdate(PhotosExpress photosExpress)
        {
            Log.Info($"START: Post Kafka Update.");

            KafkaUpdate? kafkaUpdate = new KafkaUpdate
            {
                AuditorEmail = photosExpress.AuditorEmail,
                CarrierName = photosExpress.InsuranceCompanyName,
                ClaimNumber = photosExpress.ClaimNumber,
                EventTime = DateTime.Now,
                Id = (long)photosExpress.PhotosExpressID,
                Message = "TEST TEST TEST TEST",
                Source = "Owner",
                StatusEnum = AdminEventStatus.Open,
                StatusType = photosExpress.StatusType,
                TypeEnum = AssignmentType.Photos_Express,
                QcsRep = photosExpress.QcsRepresentative,
            };

            await KafkaClient.SendUpdate(kafkaUpdate);

            Log.Info($"RETURN: Post Kafka Update successful.");

            return Ok();
        }
    }
}
