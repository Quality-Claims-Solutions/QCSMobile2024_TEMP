﻿using AutoMapper;
using log4net;
using QCSMobile2024.Shared.Models;
using QCSMobile2024.Shared.Models.ViewModels;
using QCSMobile2024.Shared.Utilities;
using System.Reflection;
using static System.Net.WebRequestMethods;

namespace QCSMobile2024.Server.Utilities
{
    public class PDFGenerator
    {
        private  readonly ILog Log;

        public PDFGenerator( ILog logger)
        {
            Log = logger;

        }
        public string GetFastTrackPdf(FnolViewModel fnol)
        {
            Log.Info($"GetFastTrackPdf:START: First.");

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"wwwroot\Assets");
            Log.Info($"Path to the Assets Folder is {path}");

            Log.Info($"Locating files within Asset Folder.");
            string templateFile = Path.Combine(path, "FastTrackPdf.html");
            string qcsLogo = Path.Combine(path, "Logo.png");
            Log.Info($"Files located.");

            var template = System.IO.File.ReadAllText(templateFile);
            if (template != null)
            {
                template = template.Replace("[QCSLogo]", qcsLogo);
                if (fnol.CarrierProgramCodeIcon != null)
                {
                    string carrierLogoImage = Convert.ToBase64String(fnol.CarrierProgramCodeIcon);
                    template = template.Replace("<img class=\"logo hide\" src=\"[CarrierLogo]\" id=\"logo2\">", $"<img class=\"logo hide\" src=\"data:image/jpeg;base64,{carrierLogoImage}\" id=\"logo2\">");
                }


                //Checks if we have enough Claimant information otherwise displays Insured info
                if (string.IsNullOrEmpty(fnol.ClaimantLastName) && string.IsNullOrEmpty(fnol.ClaimantPhone) && string.IsNullOrEmpty(fnol.ClaimantEmail))
                {
                    template = template.Replace("[ClaimantOrInsured]", "Insured");
                    template = template.Replace("[ClaimantOrInsuredFirstName]", fnol.InsuredFirstName);
                    template = template.Replace("[ClaimantOrInsuredLastName]", fnol.InsuredLastName);
                    template = template.Replace("[ClaimantOrInsuredPhoneNumber]", PhoneFormatter.FormatPhoneNumber(fnol.InsuredPrimaryPhone ?? ""));
                    template = template.Replace("[ClaimantOrInsuredEmail]", fnol.InsuredPrimaryEmail);
                    template = template.Replace("[ClaimantOrInsuredAddress]", fnol.InsuredAddress);
                    template = template.Replace("[ClaimantOrInsuredCity]", fnol.InsuredCity != null ? fnol.InsuredCity + "," : "");
                    template = template.Replace("[ClaimantOrInsuredState]", fnol.InsuredState != null ? fnol.InsuredState + "," : "");
                    template = template.Replace("[ClaimantOrInsuredZip]", fnol.InsuredZip);
                }
                else
                {
                    template = template.Replace("[ClaimantOrInsured]", "Claimant");
                    template = template.Replace("[ClaimantOrInsuredFirstName]", fnol.ClaimantFirstName);
                    template = template.Replace("[ClaimantOrInsuredLastName]", fnol.ClaimantLastName);
                    template = template.Replace("[ClaimantOrInsuredPhoneNumber]", PhoneFormatter.FormatPhoneNumber(fnol.ClaimantPhone ?? ""));
                    template = template.Replace("[ClaimantOrInsuredEmail]", fnol.ClaimantEmail);
                    template = template.Replace("[ClaimantOrInsuredAddress]", fnol.ClaimantAddress);
                    template = template.Replace("[ClaimantOrInsuredCity]", fnol.ClaimantCity != null ? fnol.ClaimantCity + "," : "");
                    template = template.Replace("[ClaimantOrInsuredState]", fnol.ClaimantState != null ? fnol.ClaimantState + "," : "");
                    template = template.Replace("[ClaimantOrInsuredZip]", fnol.ClaimantZip);
                }
                //Contact
                template = template.Replace("[ContactFirstName]", fnol.ContactFirstName);
                template = template.Replace("[ContactLastName]", fnol.ContactLastName);
                template = template.Replace("[ContactPhoneNumber]", PhoneFormatter.FormatPhoneNumber(fnol.ContactPrimaryPhone ?? ""));
                template = template.Replace("[ContactEmail]", fnol.ContactPrimaryEmail);
                template = template.Replace("[ContactAddress]", fnol.ContactAddress);
                template = template.Replace("[ContactCity]", fnol.ContactCity != null ? fnol.ContactCity + "," : "");
                template = template.Replace("[ContactState]", fnol.ContactState != null ? fnol.ContactState + "," : "");
                template = template.Replace("[ContactZip]", fnol.ContactZip);
                //Vehicle
                template = template.Replace("[VehicleYear]", fnol.InsuredVehicleYear);
                template = template.Replace("[VehicleMake]", fnol.InsuredVehicleMakeName);
                template = template.Replace("[VehicleModel]", fnol.InsuredVehicleModel);
                template = template.Replace("[VehicleVin]", fnol.InsuredVehicleVin);

                template = template.Replace("[DateTime]", DateTime.Now.ToString("M/d/yyyy hh:mm"));

                //Once Photos feature gets added
                for (var i = 0; i < fnol.FnolImageList.Count; i++)
                {
                    if (fnol.FnolImageList[i].Stream != null && fnol.FnolImageList[i].Stream.Length != 0)
                    {
                        string base64Image = Convert.ToBase64String(fnol.FnolImageList[i].Stream);
                        template = template.Replace($"[Label{i}]", $"{fnol.FnolImageList[i].Title}");

                        template = template.Replace($"class=\"hide\" src=\"[Photo{i}]\"", $" src=\"data:image/jpeg;base64,{base64Image}\"");
                    }
                }

                string signatureImage = Convert.ToBase64String(fnol.Signature.Stream);
                template = template.Replace($"[Signature]", $"<img class=\"photos\" src=\"data:image/jpeg;base64,{signatureImage}\" />");

            }

            Log.Info($"Sucessfully generated PDF document based on template.");
            return template;
        }
    }
}
