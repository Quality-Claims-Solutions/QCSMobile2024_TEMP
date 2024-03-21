using log4net;
using Microsoft.AspNetCore.Mvc;
using QCSMobile2024.Server.Kafka;
using QCSMobile2024.Shared.Models.CustomModels;
using QCSMobile2024.Shared.Models.EntityModels;
using System.Runtime.CompilerServices;

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
        static string MethodName([CallerMemberName] string name = null) => name;

        [HttpPost]
        public async Task<ActionResult> PostKafkaUpdate(PhotosExpress photosExpress)
        {
            Log.Info($"KafkaController_{MethodName()} START: Post Kafka Update for PhotosExpress with Id: {photosExpress.PhotosExpressID}");

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
            Log.Info($"KafkaController_{MethodName()} RETURN: Post Kafka Update successful for PhotosExpress with Id: {photosExpress.PhotosExpressID}");
            return Ok();
        }
    }
}
