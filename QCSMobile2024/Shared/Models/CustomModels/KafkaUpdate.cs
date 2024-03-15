using Avro;
using Avro.Generic;

namespace QCSMobile2024.Shared.Models.CustomModels
{
    /// <summary>
    /// Data model for Kafka events
    /// </summary>
    public class KafkaUpdate
    {
        public long Id { get; set; }
        public string Type
        {
            get => TypeEnum?.ToString() ?? type;
            set
            {
                type = value;
                if (System.Enum.TryParse<AssignmentType>(value, true, out var result))
                    TypeEnum = result;
                else
                    TypeEnum = null;
            }
        }
        string type;
        public AssignmentType? TypeEnum { get; set; } // e.g. DeskReview, PhotosExpress, etc.
        public string Message { get; set; }
        public string Status
        {
            get => StatusEnum.ToString();
            set
            {
                if (System.Enum.TryParse<AdminEventStatus>(value, true, out var result))
                    StatusEnum = result;
                else
                    StatusEnum = null;
            }
        }
        public AdminEventStatus? StatusEnum { get; set; }
        public string StatusType { get; set; }
        public string? QcsRep { get; set; }
        public string Source { get; set; }
        public string ClaimNumber { get; set; }
        public string AuditorEmail { get; set; }
        public int CarrierCompanyId { get; set; }
        public string CarrierName { get; set; }
        public string? CarrierLocation { get; set; }
        public DateTime EventTime { get; set; }

        public KafkaUpdate Clone()
        {
            return new KafkaUpdate
            {
                AuditorEmail = AuditorEmail,
                CarrierCompanyId = CarrierCompanyId,
                CarrierLocation = CarrierLocation,
                CarrierName = CarrierName,
                ClaimNumber = ClaimNumber,
                EventTime = EventTime,
                Id = Id,
                Message = Message,
                QcsRep = QcsRep,
                StatusEnum = StatusEnum,
                Type = Type,
            };
        }

        public static GenericRecord GetGenericRecord(KafkaUpdate updateEvent)
        {
            return updateEvent.PackRecord();
        }

        public static KafkaUpdate UnpackGenericRecord(GenericRecord record)
        {
            return UpdateEventExtension.UnpackRecord(record);
        }
    }
    /// <summary>
    /// helper methods for kafka update events
    /// </summary>
    public static class UpdateEventExtension
    {
        public static string GetSummary(this IEnumerable<KafkaUpdate> updates)
        {
            var summary = "";
            foreach (var update in updates)
                summary += update.Message + ", ";
            summary = summary.Trim(' ', ',');
            return summary;
        }

        /// <summary>
        /// gets the latest update for any action 
        /// </summary>
        /// <param name="updates"></param>
        /// <returns></returns>
        public static IEnumerable<KafkaUpdate> GetLatestUpdates(this IEnumerable<KafkaUpdate> updates)
        {
            var comparer = new UpdateComparer();
            updates = updates.OrderByDescending(p => p.EventTime);
            foreach (var update in updates)
            {
                if (updates.FirstOrDefault(p => comparer.Equals(update, p)) == update)
                    yield return update;
            }
        }

        /// <summary>
        /// packs the update event into a generic record 
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public static GenericRecord PackRecord(this KafkaUpdate update)
        {
            var record = new GenericRecord(UpdateEventSchema as RecordSchema);
            record.Add("Id", update.Id);
            record.Add("Type", update.Type);
            record.Add("Message", update.Message);
            record.Add("Status", update.Status);
            record.Add("StatusType", update.StatusType);
            record.Add("QcsRep", update.QcsRep ?? "");
            record.Add("Source", update.Source);
            record.Add("ClaimNumber", update.ClaimNumber);
            record.Add("AuditorEmail", update.AuditorEmail ?? "");
            record.Add("CarrierCompanyId", update.CarrierCompanyId);
            record.Add("CarrierName", update.CarrierName);
            record.Add("CarrierLocation", update.CarrierLocation ?? "");
            record.Add("EventTime", update.EventTime);

            return record;
        }

        /// <summary>
        /// creates an update event from the provided generic record - schema of Generic Record must be consistent with update event to work
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static KafkaUpdate UnpackRecord(GenericRecord record)
        {
            long id = 0;
            if (record.TryGetValue("Id", out var IdVal) && IdVal is long)
                id = (long)IdVal;
            string type = "";
            if (record.TryGetValue("Type", out var typeVal))
                type = (string)typeVal;
            string message = "";
            if (record.TryGetValue("Message", out var messageVal))
                message = (string)messageVal;
            string status = "";
            if (record.TryGetValue("Status", out var statusVal))
                status = (string)statusVal;
            string statusType = "";
            if (record.TryGetValue("StatusType", out var statusTypeVal))
                statusType = (string)statusTypeVal;
            string source = "";
            if (record.TryGetValue("Source", out var sourceVal))
                source = (string)sourceVal;
            string qcsRep = "";
            if (record.TryGetValue("QcsRep", out var qcsRepVal))
                qcsRep = (string)qcsRepVal;
            string auditorEmail = "";
            if (record.TryGetValue("AuditorEmail", out var auditorEmailVal))
                auditorEmail = (string)auditorEmailVal;
            string claimNumber = "";
            if (record.TryGetValue("ClaimNumber", out var claimNumberVal))
                claimNumber = (string)claimNumberVal;
            int carrierCompanyId = 0;
            if (record.TryGetValue("CarrierCompanyId", out var carrierCompanyIdVal) && carrierCompanyIdVal is int)
                carrierCompanyId = (int)carrierCompanyIdVal;
            string carrierName = "";
            if (record.TryGetValue("CarrierName", out var carrierNameVal))
                carrierName = (string)carrierNameVal;
            string carrierLocation = "";
            if (record.TryGetValue("CarrierLocation", out var carrierLocationVal))
                carrierLocation = (string)carrierLocationVal;
            DateTime eventTime = epochStart;
            if (record.TryGetValue("EventTime", out var timeValue) && timeValue is DateTime time)
                eventTime = time.ToLocalTime();

            var updateEvent = new KafkaUpdate
            {
                Id = (int)id,
                Type = type,
                Message = message,
                Status = status,
                Source = source,
                StatusType = statusType,
                QcsRep = qcsRep,
                AuditorEmail = auditorEmail,
                ClaimNumber = claimNumber,
                CarrierCompanyId = carrierCompanyId,
                CarrierName = carrierName,
                CarrierLocation = carrierLocation,
                EventTime = eventTime,
            };

            return updateEvent;
        }
        static readonly DateTime epochStart = new DateTime(1970, 1, 1);

        public static Schema UpdateEventSchema = Schema.Parse(@" 
 {
  ""type"": ""record"",
  ""name"": ""EAS_Updates"",
  ""fields"": [
    {
      ""name"": ""Id"",
      ""type"": [
        ""null"",
        ""long""
      ],
      ""default"": null
    },
    {
      ""name"": ""Type"",
      ""type"": [
        ""null"",
        ""string""
      ],
      ""default"": null
    },
    {
      ""name"": ""Message"",
      ""type"": [
        ""null"",
        ""string""
      ],
      ""default"": null
    },
    {
      ""name"": ""Status"",
      ""type"": [
        ""null"",
        ""string""
      ],
      ""default"": null
    },
    {
      ""name"": ""QcsRep"",
      ""type"": [
        ""null"",
        ""string""
      ],
      ""default"": null
    },
    {
      ""name"": ""Source"",
      ""type"": [
        ""null"",
        ""string""
      ],
      ""default"": null
    },
    {
      ""name"": ""ClaimNumber"",
      ""type"": [
        ""null"",
        ""string""
      ],
      ""default"": null
    },
    {
      ""name"": ""StatusType"",
      ""type"": [
        ""null"",
        ""string""
      ],
      ""default"": null
    },
    {
      ""name"": ""AuditorEmail"",
      ""type"": [
        ""null"",
        ""string""
      ],
      ""default"": null
    },
    {
      ""name"": ""CarrierCompanyId"",
      ""type"": [
        ""null"",
        ""int""
      ],
      ""default"": null
    },
    {
      ""name"": ""CarrierName"",
      ""type"": [
        ""null"",
        ""string""
      ],
      ""default"": null
    },
    {
      ""name"": ""CarrierLocation"",
      ""type"": [
        ""null"",
        ""string""
      ],
      ""default"": null
    },
    {
      ""name"": ""EventTime"",
      ""type"": [
        ""null"",
        {
          ""type"": ""long"",
          ""connect.version"": 1,
          ""connect.name"": ""org.apache.kafka.connect.data.Timestamp"",
          ""logicalType"": ""timestamp-millis""
        }
      ],
      ""default"": null
    }
  ],
  ""connect.name"": ""EAS_Updates""
}
        ");
    }

    public enum AdminEventStatus
    {
        Open,
        Claimed,
        Incomplete,
        Closed,
    }

    public class UpdateEventArgs : EventArgs
    {
        public KafkaUpdate Update { get; set; }
    }

    public class UpdateComparer : IEqualityComparer<KafkaUpdate>
    {
        public bool Equals(KafkaUpdate x, KafkaUpdate y)
        {
            return AreEqual(x, y);
        }

        public static bool AreEqual(KafkaUpdate x, KafkaUpdate y)
        {
            return x.Id == y.Id
                && x.TypeEnum == y.TypeEnum
                && string.Compare(x.ClaimNumber, y.ClaimNumber, true) == 0
                && string.Compare(x.AuditorEmail, y.AuditorEmail, true) == 0
                && string.Compare(x.Message, y.Message, true) == 0;
        }

        public int GetHashCode(KafkaUpdate obj)
        {
            return obj.EventTime.GetHashCode();
        }
    }

    public enum AssignmentType
    {
        Desk_Review,
        DRP_Review,
        Photos_Express,
        Hertz_Photo_Estimate,
        FNOL,
        FirstNoticeLiability,
        FirstNoticeProperty,
        Email,
        Dispatch,
        Dispatch_Statusing,
        Dispatch_Editor,
        Shop
    }
}
