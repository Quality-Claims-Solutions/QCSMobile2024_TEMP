using QCSMobile2024.Shared.Models.ViewModels;
using QCSMobile2024.Shared.Utilities;

namespace QCSMobile2024.Utilities
{
    public class PDFGenerator
    {
        public static string GetFastTrackPdf(FnolViewModel fnol)
        {
            string folderName = "Assets";
            string[] subdirectories = Directory.GetDirectories(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, folderName, SearchOption.AllDirectories);
            string fullPathToFolder = subdirectories.FirstOrDefault();

            string templateFile = Path.Combine(fullPathToFolder, "FastTrackPdf.html");
            string qcsLogo = Path.Combine(fullPathToFolder, "Logo.png");
            var template = File.ReadAllText(templateFile);
            if (template != null)
            {
                template = template.Replace("[QCSLogo]", qcsLogo);
                if(fnol.CarrierProgramCodeIcon != null)
                {
                    string carrierLogoImage = Convert.ToBase64String(fnol.CarrierProgramCodeIcon);
                    template = template.Replace("<img class=\"logo hide\" src=\"[CarrierLogo]\" id=\"logo2\">", $"<img class=\"logo hide\" src=\"data:image/jpeg;base64,{carrierLogoImage}\" id=\"logo2\">"); 
                }
               

                //Checks if we have enough Claimant information otherwise displays Insured info
                if (String.IsNullOrEmpty(fnol.ClaimantLastName) && String.IsNullOrEmpty(fnol.ClaimantPhone) && String.IsNullOrEmpty(fnol.ClaimantEmail))
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
                template = template.Replace("[ContactCity]", fnol.ContactCity != null ? fnol.ContactCity +"," : "");
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
                        template = template.Replace($"<div class=\"photo-label\">[Label{i}]</div>", $"<td>{fnol.FnolImageList[i].Title}</td>");

                        template = template.Replace($"<img class=\"hide\" src=\"[Photo{i}]\" alt=\"[Label{i}]\">", $"<td><img class=\"photos\" src=\"data:image/jpeg;base64,{base64Image}\" /></td>");
                    }
                }

                string signatureImage = Convert.ToBase64String(fnol.Signature.Stream);
                template = template.Replace($"[Signature]", $"<img class=\"photos\" src=\"data:image/jpeg;base64,{signatureImage}\" />");

            }
            return template;
        }
    }
}
