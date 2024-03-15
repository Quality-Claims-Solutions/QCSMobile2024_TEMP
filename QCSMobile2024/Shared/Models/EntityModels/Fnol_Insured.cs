using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCSMobile2024.Shared.Models.EntityModels
{
    public class Fnol_Insured
    {
        public decimal FnolInsuredVehicleID { get; set; }

        public decimal FnolID { get; set; }

        public decimal? DispatchID { get; set; }

        public int? VehicleOwnerSameAsID { get; set; }

        public bool VehicleOwnerInjured { get; set; }

        public string? VehicleOwnerFirstName { get; set; }

        public string? VehicleOwnerMiddleName { get; set; }

        public string? VehicleOwnerLastName { get; set; }

        public int? VehicleOwnerGender { get; set; }

        public int? VehicleOwnerLanguage { get; set; }

        public string? VehicleOwnerAddress { get; set; }

        public string? VehicleOwnerCity { get; set; }

        public string? VehicleOwnerState { get; set; }

        public string? VehicleOwnerZip { get; set; }

        public string? VehicleOwnerPrimaryPhone { get; set; }

        public string? VehicleOwnerPrimaryPhoneExt { get; set; }

        public int? VehicleOwnerPrimaryPhoneTypeID { get; set; }

        public string? VehicleOwnerSecondaryPhone { get; set; }

        public string? VehicleOwnerSecondaryPhoneExt { get; set; }

        public int? VehicleOwnerSecondaryPhoneTypeID { get; set; }

        public string? VehicleOwnerPrimaryEmail { get; set; }

        public string? VehicleOwnerSecondaryEmail { get; set; }

        public string? VehicleOwnerSSN { get; set; }

        public string? InsuredVehicleYear { get; set; }

        public int? InsuredVehicleMake { get; set; }

        public string? InsuredVehicleModel { get; set; }

        public string? InsuredVehicleBodyType { get; set; }

        public string? InsuredVehicleVin { get; set; }

        public string? InsuredVehicleLicensePlate { get; set; }

        public string? InsuredVehicleLicensePlateState { get; set; }

        public string? InsuredVehicleVehicleNumber { get; set; }

        public int? DriverSameAsID { get; set; }

        public bool DriverInjured { get; set; }

        public string? DriverFirstName { get; set; }

        public string? DriverMiddleName { get; set; }

        public string? DriverLastName { get; set; }

        public int? DriverGender { get; set; }

        public int? DriverLanguage { get; set; }

        public bool? DriverVehicleIsDrivable { get; set; }

        public string? DriverAddress { get; set; }

        public string? DriverCity { get; set; }

        public string? DriverState { get; set; }

        public string? DriverZip { get; set; }

        public string? DriverPrimaryPhone { get; set; }

        public string? DriverPrimaryPhoneExt { get; set; }

        public int? DriverPrimaryPhoneTypeID { get; set; }

        public string? DriverSecondaryPhone { get; set; }

        public string? DriverSecondaryPhoneExt { get; set; }

        public int? DriverSecondaryPhoneTypeID { get; set; }

        public string? DriverPrimaryEmail { get; set; }

        public string? DriverSecondaryEmail { get; set; }

        public string? DriverRelationshipToInsured { get; set; }

        public DateTime? DriverDateOfBirth { get; set; }

        public string? DriverLicenseNumber { get; set; }

        public string? DriverLicenseNumberState { get; set; }

        public string? DriverPurposeOfUse { get; set; }

        public bool DriverUsedWithPermission { get; set; }

        public bool NonVehicle { get; set; }

        public string? NonVehicleDescription { get; set; }

        public int? DriverPointOfImpactID { get; set; }

        public string? DriverDescribeDamage { get; set; }

        public decimal? DriverEstimateAmount { get; set; }

        public int? DriverVehicleSeenSameAsID { get; set; }

        public string? DriverVehicleSeenAddress { get; set; }

        public string? DriverVehicleSeenCity { get; set; }

        public string? DriverVehicleSeenState { get; set; }

        public string? DriverVehicleSeenZip { get; set; }

        public DateTime? DriverVehicleSeenDate { get; set; }

        public string? DriverVehicleSeenTime { get; set; }

        public string? DriverVehicleSeenStockNumber { get; set; }

        public string? DriverVehicleSeenLocationName { get; set; }

        public string? DriverVehicleSeenContactName { get; set; }

        public string? DriverOtherInsurance { get; set; }

        public string? DriverPolicyNumber { get; set; }

        public decimal? EstimateSourceID { get; set; }

        public int? EstimateSourceTypeID { get; set; }

        public bool? RequestInsurance { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsArchived { get; set; }

        public bool IsScrapeSent { get; set; }

        public string? Remarks { get; set; }

        public bool IsClaimant { get; set; }

        public bool DriverSameAsOwner { get; set; }

        public bool DriverSameAsInsured { get; set; }

        public string? ClaimantPolicyNumber { get; set; }

        public string? ClaimantInsuranceCarrier { get; set; }

        public DateTime? VehicleOwnerDateOfBirth { get; set; }

        public string? VehicleOwnerLicenseNumber { get; set; }

        public string? VehicleOwnerLicenseNumberState { get; set; }

        public decimal? EstimateAmount { get; set; }

        public int? OwnerSameAsId { get; set; }

    }
}
