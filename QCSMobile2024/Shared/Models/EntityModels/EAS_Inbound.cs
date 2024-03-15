using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QCSMobile2024.Shared.Models.EntityModels
{
    public class EAS_Inbound
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DateEntered { get; set; }

        public DateTime? LastUpdate { get; set; }

        public DateTime? DateClosed { get; set; }

        public int? AssignmentId { get; set; }

        public string? ClaimNumber { get; set; }

        public string? Message { get; set; }

        public string? Status { get; set; }

        public string? AssignmentType { get; set; }

        public string? AssignmentStatusType { get; set; }

        public string? AuditorEmail { get; set; }

        public int? CarrierCompanyId { get; set; }

        public string? CarrierName { get; set; }

        public string? CarrierLocation { get; set; }

        public string? QcsRep { get; set; }

        public string? Source { get; set; }
    }
}