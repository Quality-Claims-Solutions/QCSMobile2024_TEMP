using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QCSMobile2024.Shared.Models.EntityModels
{
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal NoteID { get; set; }

        public decimal? DispatchID { get; set; }

        public string? NoteText { get; set; }

        public DateTime? TimeStamp { get; set; }

        public int UserID { get; set; }

        public bool EstimateSourceNote { get; set; }

        public bool EstimateSourceIsRead { get; set; }

        public bool IsArchived { get; set; }

        public int? User_AssociationMatchup_PKID { get; set; }

        public bool IsRead { get; set; }

        public bool WebServicePickedup { get; set; }

        public decimal? PhotosExpressId { get; set; }

        public int? DispatchV2Id { get; set; }

        public string? Author { get; set; }

        public int? FnolId { get; set; }

        public int? FirstNoticePropertyId { get; set; }

        public int? FirstNoticeSuretyId { get; set; }

        public int? FirstNoticeLiabilityId { get; set; }

        public int? DeskReviewId { get; set; }

        public int? TotalLossId { get; set; }

        public int? HertzRentalPhotoId { get; set; }

        public int? HertzOnSiteRepairId { get; set; }

        public int? HertzVIR_Id { get; set; }

        public string? EmailResult { get; set; }

        public int? FirstNoticeSDI_Id { get; set; }

        public int? HertzClosingChecklistId { get; set; }

        public int? HertzSalvageId { get; set; }

        public int? HertzClosingChecklistTNC_Id { get; set; }

        public int? CarSalePhotoId { get; set; }
    }
}
