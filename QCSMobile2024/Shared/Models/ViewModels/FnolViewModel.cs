using QCSMobile2024.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace QCSMobile2024.Shared.Models.ViewModels
{
    public class FnolViewModel
    {
        public decimal? FnolID { get; set; }

        public decimal? PhotosExpressID { get; set; }

        public string? ProgramCode { get; set; }

        public int StatusTypeID { get; set; }

        //additional fields for PhotoesExpress convertion

        public int? FastTrackFeatureTier { get; set; }

        //Claimant Info
        public string? ClaimantFirstName { get; set; }

        public string? ClaimantLastName { get; set; }

        public string? ClaimantPhone { get; set; }

        public string? ClaimantEmail { get; set; }

        public string? ClaimantAddress { get; set; }

        public string? ClaimantCity { get; set; }

        public string? ClaimantState { get; set; }

        public string? ClaimantZip { get; set; }

        //Contact Info
        [Required(ErrorMessage = "First Name is required")]
        public string? ContactFirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string? ContactLastName { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string? ContactPrimaryPhone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string? ContactPrimaryEmail { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? ContactAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string? ContactCity { get; set; }

        [NotEmptyOrZero(ErrorMessage = "State is required")]
        public string? ContactState { get; set; }

        [Required(ErrorMessage = "Zip is required")]
        public string? ContactZip { get; set; }


        //Insured Info
        public string? InsuredFirstName { get; set; }

        public string? InsuredLastName { get; set; }

        public string? InsuredPrimaryPhone { get; set; }

        public string? InsuredPrimaryEmail { get; set; }

        public string? InsuredAddress { get; set; }

        public string? InsuredCity { get; set; }

        public string? InsuredState { get; set; }

        public string? InsuredZip { get; set; }

        //Vehicle Fields
        public int? InsuredVehicleMake { get; set; }

        public string? InsuredVehicleMakeName { get; set; }

        public string? InsuredVehicleModel { get; set; }

        public string? InsuredVehicleYear { get; set; }

        public string? InsuredVehicleVin { get; set; }

        public string? AdditionalRemarks { get; set; }

        // Additional Extraneous Fields
        public string? FastTrackEmailAddress { get; set; }

        public byte[]? CarrierProgramCodeIcon { get; set; }

        public string? CarrierProgramCodeIconPath { get; set; }

        public List<NoteViewModel> Notes { get; set; }

        public List<FileAttachmentViewModel>? FnolImageList { get; set; } 

        public FileAttachmentViewModel Signature { get; set; }

        public FileAttachmentViewModel FastTrackSummaryPdf { get; set; }

        public void InitializeImages()
        {
            //FnolImageList =
            //[
            //    new FileAttachmentViewModel()
            //    {
            //        Title = "Front Right",
            //        FnolId = (int)this.FnolID,
            //        ThumbnailHtmlClass = "image-thumbnail no-image front-right-default",
            //        ChangeTypeId = 2,
            //        PhotosExpressAttachmentTypeID = Convert.ToInt16(PhotosExpressAttachmentTypeEnum.Right_Front)
            //    },
            //    new FileAttachmentViewModel()
            //    {
            //        Title = "Front Left",
            //        FnolId = (int)this.FnolID,
            //        ThumbnailHtmlClass = "image-thumbnail no-image front-left-default",
            //        ChangeTypeId = 2,
            //        PhotosExpressAttachmentTypeID = Convert.ToInt16(PhotosExpressAttachmentTypeEnum.Left_Front)
            //    },
            //    new FileAttachmentViewModel()
            //    {
            //        Title = "Rear Right",
            //        FnolId = (int)this.FnolID,
            //        ThumbnailHtmlClass = "image-thumbnail no-image rear-right-default",
            //        ChangeTypeId = 2,
            //        PhotosExpressAttachmentTypeID = Convert.ToInt16(PhotosExpressAttachmentTypeEnum.Right_Rear)
            //    },
            //    new FileAttachmentViewModel()
            //    {
            //        Title = "Rear Left",
            //        FnolId = (int)this.FnolID,
            //        ThumbnailHtmlClass = "image-thumbnail no-image rear-left-default",
            //        ChangeTypeId = 2,
            //        PhotosExpressAttachmentTypeID = Convert.ToInt16(PhotosExpressAttachmentTypeEnum.Left_Rear)
            //    },
            //    new FileAttachmentViewModel()
            //    {
            //        Title = "VIN",
            //        FnolId = (int)this.FnolID,
            //        ThumbnailHtmlClass = "image-thumbnail no-image vin-default",
            //        ChangeTypeId = 2,
            //        PhotosExpressAttachmentTypeID = Convert.ToInt16(PhotosExpressAttachmentTypeEnum.VIN)
            //    },
            //    new FileAttachmentViewModel()
            //    {
            //        Title = "Odometer",
            //        FnolId = (int)this.FnolID,
            //        ThumbnailHtmlClass = "image-thumbnail no-image odometer-default",
            //        ChangeTypeId = 2,
            //        PhotosExpressAttachmentTypeID = Convert.ToInt16(PhotosExpressAttachmentTypeEnum.Odometer)
            //    },
            //    new FileAttachmentViewModel()
            //    {
            //        Title = "Damage Photo 1",
            //        FnolId = (int)this.FnolID,
            //        ThumbnailHtmlClass = "image-thumbnail no-image damage-default",
            //        ChangeTypeId = 2,
            //        PhotosExpressAttachmentTypeID = Convert.ToInt16(PhotosExpressAttachmentTypeEnum.Damage_Area)
            //    },
            //    new FileAttachmentViewModel()
            //    {
            //        Title = "Damage Photo 2",
            //        FnolId = (int)this.FnolID,
            //        ThumbnailHtmlClass = "image-thumbnail no-image damage-default",
            //        ChangeTypeId = 2,
            //        PhotosExpressAttachmentTypeID = Convert.ToInt16(PhotosExpressAttachmentTypeEnum.Damage_Area)
            //    },
            //    new FileAttachmentViewModel()
            //    {
            //        Title = "Damage Photo 3",
            //        FnolId = (int)this.FnolID,
            //        ThumbnailHtmlClass = "image-thumbnail no-image damage-default",
            //        ChangeTypeId = 2,
            //        PhotosExpressAttachmentTypeID = Convert.ToInt16(PhotosExpressAttachmentTypeEnum.Damage_Area)
            //    }
            //];
        }
    }

    public class NotEmptyOrZero : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            if (value is string str && (string.IsNullOrWhiteSpace(str) || str.Trim() == "0"))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
