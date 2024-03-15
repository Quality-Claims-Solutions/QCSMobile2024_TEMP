namespace QCSMobile2024.Shared.Models.EntityModels
{
    public class Fnol
    {
        public decimal FnolID { get; set; }

        public int? CarrierCompanyID { get; set; }

        public int? CarrierLocationId { get; set; }

        public int? CarrierAdjusterUserID { get; set; }

        public DateTime? CarrierReportDate { get; set; }

        public DateTime? CarrierDateOfLoss { get; set; }

        public string? CarrierDateOfLossTime { get; set; }

        public string? CarrierPolicyNumber { get; set; }

        public string? CarrierPolicyType { get; set; }

        public string? CarrierClaimNumber { get; set; }

        public string? CarrierABAReferenceNumber { get; set; }

        public string? AgencyContactName { get; set; }

        public string? AgencyPhone { get; set; }

        public string? AgencyPhoneExt { get; set; }

        public int? AgencyPhoneTypeID { get; set; }

        public string? AgencyFax { get; set; }

        public string? AgencyEmail { get; set; }

        public string? AgencyCode { get; set; }

        public string? AgencySubcode { get; set; }

        public string? AgencyCustomerID { get; set; }

        public string? InsuredFirstName { get; set; }

        public string? InsuredMiddleName { get; set; }

        public string? InsuredLastName { get; set; }

        public int? InsuredGender { get; set; }

        public int? InsuredLanguage { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }

        public int? InsuredMaritalStatusID { get; set; }

        public string? InsuredPrimaryPhone { get; set; }

        public string? InsuredPrimaryPhoneExt { get; set; }

        public int? InsuredPrimaryPhoneTypeID { get; set; }

        public string? InsuredSecondaryPhone { get; set; }

        public string? InsuredSecondaryPhoneExt { get; set; }

        public int? InsuredSecondaryPhoneTypeID { get; set; }

        public string? InsuredPrimaryEmail { get; set; }

        public string? InsuredSecondaryEmail { get; set; }

        public string? InsuredAddress { get; set; }

        public string? InsuredCity { get; set; }

        public string? InsuredState { get; set; }

        public string? InsuredZip { get; set; }

        public bool? ContactSameAsInsured { get; set; }

        public string? ContactFirstName { get; set; }

        public string? ContactMiddleName { get; set; }

        public string? ContactLastName { get; set; }

        public int? ContactGender { get; set; }

        public int? ContactLanguage { get; set; }

        public string? ContactPrimaryPhone { get; set; }

        public string? ContactPrimaryPhoneExt { get; set; }

        public int? ContactPrimaryPhoneTypeID { get; set; }

        public string? ContactSecondaryPhone { get; set; }

        public string? ContactSecondaryPhoneExt { get; set; }

        public int? ContactSecondaryPhoneTypeID { get; set; }

        public string? ContactPrimaryEmail { get; set; }

        public string? ContactSecondaryEmail { get; set; }

        public string? ContactAddress { get; set; }

        public string? ContactCity { get; set; }

        public string? ContactState { get; set; }

        public string? ContactZip { get; set; }

        public DateTime? ContactWhenToDate { get; set; }

        public string? ContactWhenToDateTime { get; set; }

        public int? StatusTypeID { get; set; }

        public bool? IsArchived { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DateEntered { get; set; }

        public DateTime? DateCompleted { get; set; }

        public string? RemarksBy { get; set; }

        public string? RemarksTo { get; set; }

        public string? Remarks { get; set; }

        public bool IsScrapeSent { get; set; }

        public string? AspNetUserId { get; set; }

        public string? UploadId { get; set; }

        public string? AspAdjusterIdV2 { get; set; }

        public string? CancellationReason { get; set; }

        public string? AgencyName { get; set; }

        public string? ProgramCode { get; set; }

        public string? QcsRepresentative { get; set; }

        public DateTime? EntryCompleted { get; set; }

        public DateTime? Cancelled { get; set; }

        public string? CancelledBy { get; set; }

        public DateTime? AdjusterAssigned { get; set; }

        public bool Escalation { get; set; }

        public string? ClaimantFirstName { get; set; }

        public string? ClaimantLastName { get; set; }

        public string? ClaimantPolicyNumber { get; set; }

        public string? ClaimantInsuranceCarrier { get; set; }

        public string? ClaimantPhone { get; set; }

        public string? ClaimantEmail { get; set; }

        public string? ClaimantAddress { get; set; }

        public string? ClaimantCity { get; set; }

        public string? ClaimantState { get; set; }

        public string? ClaimantZip { get; set; }

        public bool MobileInbound { get; set; }

        public string? InsuredDriversLicense { get; set; }

        public DateTime? DateEmployerNotified { get; set; }

        public bool FuelSpilled { get; set; }

        public bool UnderDispatch { get; set; }

        public bool HaulingCargo { get; set; }

        public bool AuthoritiesContacted { get; set; }

        public string? PoliceReportNumber { get; set; }

        public string? PoliceDepartmentName { get; set; }

        public string? InsuredDriversLicenseState { get; set; }

        public string? PreferredContactMethod { get; set; }

        public string? LastEdit { get; set; }

        public bool? CarrierEmailSent { get; set; }

        public bool? CarrierFtpSent { get; set; }

        public string? CheckNumber { get; set; }

        public bool? FastTrackSmsSent { get; set; }
#nullable disable
    }
}
