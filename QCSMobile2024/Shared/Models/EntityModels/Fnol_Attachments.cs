using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QCSMobile2024.Shared.Models.EntityModels
{
    public class Fnol_Attachments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal FnolAttachmentsID { get; set; }

        public decimal FnolID { get; set; }

        public decimal? FnolClaimaintVehicleID { get; set; }

        public decimal? FnolInsuredVehicleID { get; set; }

        public string Path { get; set; }

        public DateTime DateEntered { get; set; }

        public string? FileName { get; set; }

        public string? Description { get; set; }

        public bool IsDeleted { get; set; }

        public string? UploadId { get; set; }

    }
}
