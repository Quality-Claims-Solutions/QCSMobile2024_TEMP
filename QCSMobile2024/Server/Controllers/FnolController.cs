using AutoMapper;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QCSMobile2024.Shared.Models;
using QCSMobile2024.Shared.Models.EntityModels;
using QCSMobile2024.Shared.Models.ViewModels;
using System.Runtime.CompilerServices;
namespace QCSMobile2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FnolController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILog Log;
        private Db _db;
        public FnolController(Db db, IMapper mapper, ILog logger)
        {
            _db = db;
            _mapper = mapper;
            Log = logger;
        }

        static string MethodName([CallerMemberName] string name = null) => name;


        [HttpGet("{id}")] //Returns FnolViewModel from given Fnol Id
        public async Task<ActionResult<FnolViewModel>> Get(decimal id)
        {
            Log.Info($"FnolController_{MethodName()} START: Getting FnolViewModel with Id: {id}");
            try
            {
                if (_db.Fnol == null)
                {
                    Log.Error($"FnolController_{MethodName()} ERROR: Fnol table is null.");
                    return NotFound();
                }

                IQueryable<FnolViewModel> query = (
                     from fnol in _db.Fnol
                     join fnolInsured in _db.Fnol_Insured
                         on fnol.FnolID equals fnolInsured.FnolID into insuredRecords
                     from insuredRecord in insuredRecords.DefaultIfEmpty() // Perform left join with Fnol_Insured
                     where fnol.FnolID == id
                     join vehicleMake in _db.VehicleMake
                         on insuredRecord.InsuredVehicleMake equals vehicleMake.id into vehicleMakeRecords
                     from vehicleMakeRecord in vehicleMakeRecords.DefaultIfEmpty() // Perform left join with VehicleMake

                     join carrierProgramCode in _db.CarrierProgramCode
                         on fnol.ProgramCode equals carrierProgramCode.Code into programCodeRecords
                     from programCodeRecord in programCodeRecords.DefaultIfEmpty()
                     where fnol.FnolID == id
                     
                     select new FnolViewModel
                     {
                        FnolID = fnol.FnolID,
                        ProgramCode = fnol.ProgramCode,
                        ClaimantFirstName = fnol.ClaimantFirstName,
                        ClaimantLastName = fnol.ClaimantLastName,
                        ClaimantPhone = fnol.ClaimantPhone,
                        ClaimantEmail = fnol.ClaimantEmail,
                        ClaimantAddress = fnol.ClaimantAddress,
                        ClaimantCity = fnol.ClaimantCity,
                        ClaimantState = fnol.ClaimantState,
                        ClaimantZip = fnol.ClaimantZip,
                        ContactFirstName = fnol.ContactFirstName,
                        ContactLastName = fnol.ContactLastName,
                        ContactPrimaryPhone = fnol.ContactPrimaryPhone,
                        ContactPrimaryEmail = fnol.ContactPrimaryEmail,
                        ContactAddress = fnol.ContactAddress,
                        ContactCity = fnol.ContactCity,
                        ContactState = fnol.ContactState,
                        ContactZip = fnol.ContactZip,
                        InsuredFirstName = fnol.InsuredFirstName,
                        InsuredLastName = fnol.InsuredLastName,
                        InsuredPrimaryPhone = fnol.InsuredPrimaryPhone,
                        InsuredPrimaryEmail = fnol.InsuredPrimaryEmail,
                        InsuredAddress = fnol.InsuredAddress,
                        InsuredCity = fnol.InsuredCity,
                        InsuredState = fnol.InsuredState,
                        InsuredZip = fnol.InsuredZip,
                        InsuredVehicleMake = insuredRecord != null ? insuredRecord.InsuredVehicleMake : null,
                        InsuredVehicleModel = insuredRecord != null ? insuredRecord.InsuredVehicleModel : null,
                        InsuredVehicleYear = insuredRecord != null ? insuredRecord.InsuredVehicleYear : null,
                        InsuredVehicleVin = insuredRecord != null ? insuredRecord.InsuredVehicleVin : null,
                        InsuredVehicleMakeName = vehicleMakeRecord != null ? vehicleMakeRecord.Make : null,
                        FastTrackFeatureTier = programCodeRecord != null ? programCodeRecord.FastTrackFeatureTier : null,
                        FastTrackEmailAddress = programCodeRecord != null ? programCodeRecord.FastTrackEmailAddress : null,
                        CarrierProgramCodeIconPath = programCodeRecord != null ? programCodeRecord.Icon : null,
                        StatusTypeID = fnol.StatusTypeID.Value
                    });

                FnolViewModel FnolViewModel = await query.FirstOrDefaultAsync();

                if (FnolViewModel != null)
                {
                    if (FnolViewModel.CarrierProgramCodeIconPath != null)
                    {
                        FnolViewModel.CarrierProgramCodeIcon = await System.IO.File.ReadAllBytesAsync(FnolViewModel.CarrierProgramCodeIconPath);
                    }

                    Log.Info($"FnolController_{MethodName()} RETURN: Returning FnolViewModel with Id: {id}");
                    return Ok(FnolViewModel);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error($"FnolController_{MethodName()} ERROR: Error creating FnolViewModel with Id: {id}. Exception: {ex.Message}");
                return BadRequest();
            }
        }


        [HttpPut]
        public async Task<ActionResult> UpdateContactInfo(Fnol fnol)
        {
            Log.Info($"FnolController_{MethodName()} START: Update Fnol with Id: {fnol.FnolID}");
            try
            {
                if (_db.Fnol == null)
                {
                    Log.Error($"FnolController_{MethodName()} ERROR: Fnol table is null.");
                    return NotFound();
                }

                Fnol entity = await _db.Fnol.FirstOrDefaultAsync(f => f.FnolID == fnol.FnolID);

                if (entity != null)
                {
                    entity.ContactFirstName = fnol.ContactFirstName;
                    entity.ContactLastName = fnol.ContactLastName;
                    entity.ContactPrimaryPhone = fnol.ContactPrimaryPhone;
                    entity.ContactPrimaryEmail = fnol.ContactPrimaryEmail;
                    entity.ContactAddress = fnol.ContactAddress;
                    entity.ContactCity = fnol.ContactCity;
                    entity.ContactState = fnol.ContactState;
                    entity.ContactZip = fnol.ContactZip;
                    entity.StatusTypeID = fnol.StatusTypeID;
                    _db.Fnol.Update(entity);
                    await _db.SaveChangesAsync();

                }

                Log.Info($"FnolController_{MethodName()} RETURN: Updated Fnol with Id: {fnol.FnolID}");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error($"FnolController_{MethodName()} ERROR: Error updating FnolViewModel with Id: {fnol.FnolID}. Exception: {ex.Message}");
                return BadRequest();
            }
        }
    }
}
