using AutoMapper;
using QCSMobile2024.Shared.Enums;
using QCSMobile2024.Shared.Models.CustomModels;
using QCSMobile2024.Shared.Models.EntityModels;
using QCSMobile2024.Shared.Models.ViewModels;
using QCSMobile2024.Shared.Utilities;
using Serilog;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
namespace QCSMobile2024.Shared.Services
{
    public class FastTrackService
    {
        public HttpClient _http { get; }
        private readonly IMapper _mapper;
        public FileService _fileService { get; }

        public FastTrackService(HttpClient http, IMapper mapper, FileService fileService)
        {
            _http = http;
            _mapper = mapper;
            _fileService = fileService;
        }

        static string MethodName([CallerMemberName] string name = null) => name;


        private async Task AddFnolAttachments(List<FileAttachmentViewModel> attachmentList)
        {
            try
            {
                Log.Information($"FastTrackService_{MethodName()}: Start.");

                using var fileContentList = new MultipartFormDataContent();
                var attachmentTableList = new List<Fnol_Attachments>();

                foreach (var file in attachmentList.Where(file => !string.IsNullOrEmpty(file.FileName)))
                {
                    // Ensure files have an extension, so at least we get an error if they're used.
                    var fileName = file.FileName;
                    if (Path.GetExtension(fileName) == null)
                    {
                        fileName += ".pdf";
                    }

                    // Checks server location avoid name duplication
                    var uniqueFileName = FileNamer.GetUniqueFileName("\\\\192.168.29.94\\qcs_Files\\Fnol_Vehicle", fileName);
                    file.Path = "\\\\192.168.29.94\\qcs_Files\\Fnol_Vehicle\\" + uniqueFileName;

                    // Build list for table insertion
                    var claimFile = new Fnol_Attachments
                    {
                        DateEntered = DateTime.Now,
                        FnolID = file.FnolId.Value,
                        FileName = fileName,
                        Path = uniqueFileName,
                        Description = file.Title
                    };
                    attachmentTableList.Add(claimFile);

                    // Build list for file copy to server location
                    var fileContent = new StreamContent(new MemoryStream(file.Stream));
                    fileContentList.Add
                    (
                        content: fileContent,
                        name: "\"files\"",
                        fileName: uniqueFileName
                    );
                }

                // Step 3: Insert the appropriate Fnol_Attachments
                if (attachmentTableList.Any() && fileContentList.Any())
                {
                    // Copy files to Server Location.
                    var attachmentPostResponse = await _http.PostAsync($"api/file/FnolAttachment", fileContentList).ConfigureAwait(false);
                    var uploadResults = await attachmentPostResponse.Content.ReadFromJsonAsync<List<UploadResult>>();

                    // Filter the list down to only the ones that were successfully uploaded to the server location.
                    List<Fnol_Attachments> successfulAttachments = attachmentTableList
                        .Where(attachment => uploadResults.Any(result => result.FileName == attachment.Path && result.Uploaded))
                        .ToList();

                    // Insert into the Fnol_Attachments table.
                    var attachmentResponse = await _http.PostAsJsonAsync<List<Fnol_Attachments>>($"api/Fnol_Attachments", successfulAttachments).ConfigureAwait(false);
                }
                Log.Information($"FastTrackService_{MethodName()}: END.");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Log.Error($"FastTrackService_{MethodName()}: Encountered an exception. {ex.Message}.");
                throw new Exception("An error occurred while adding FNOL attachments.");
            }

        }


        private async Task AddPhotosExpressAttachments(List<FileAttachmentViewModel> attachmentList)
        {
            Log.Information($"FastTrackService_{MethodName()}: Start.");

            using var fileContentList = new MultipartFormDataContent();
            var attachmentTableList = new List<PhotosExpress_Attachment>();
            foreach (var file in attachmentList.Where(file => !string.IsNullOrEmpty(file.FileName)))
            {
                // Ensure files have an extension, so at least we get an error if they're used.
                var fileName = file.FileName;
                if (Path.GetExtension(fileName) == null)
                {
                    fileName += ".pdf";
                }

                if (fileName.Contains(".pdf"))
                {
                    file.ChangeTypeId = 2; // New
                    file.PhotosExpressAttachmentTypeID = 23; // Document
                }

                // Checks server location avoid name duplication
                var uniqueFileName = FileNamer.GetUniqueFileName("\\\\192.168.29.94\\qcs_Files\\PhotosExpress", fileName);

                // Build list for table insertion
                var claimFile = new PhotosExpress_Attachment
                {
                    DateEntered = DateTime.Now,
                    PhotosExpressID = file.PhotosExpressId.Value,
                    ChangeTypeID = (int)file.ChangeTypeId,
                    PhotosExpressAttachmentTypeID = (int)file.PhotosExpressAttachmentTypeID,
                    FileName = fileName,
                    Path = uniqueFileName,
                    AddedBy = "Owner"
                };
                attachmentTableList.Add(claimFile);

                // Build list for file copy to server location
                var fileContent = new StreamContent(new MemoryStream(file.Stream));
                fileContentList.Add
                (
                    content: fileContent,
                    name: "\"files\"",
                    fileName: uniqueFileName
                );
            }

            if (attachmentTableList.Any() && fileContentList.Any())
            {
                // Copy files to Server Location.
                var attachmentPostResponse = await _http.PostAsync($"api/file/PhotosExpressAttachment", fileContentList).ConfigureAwait(false);
                var uploadResults = await attachmentPostResponse.Content.ReadFromJsonAsync<List<UploadResult>>();

                // Filter the list down to only the ones that were successfully uploaded to the server location.
                List<PhotosExpress_Attachment> successfulAttachments = attachmentTableList
                    .Where(attachment => uploadResults.Any(result => result.FileName == attachment.Path && result.Uploaded))
                    .ToList();

                //Added succesfully uploaded files to PhotosExpress_Attachment table
                var attachmentResponse = await _http.PostAsJsonAsync<List<PhotosExpress_Attachment>>($"api/PhotosExpress_Attachment", successfulAttachments).ConfigureAwait(false);
            }
            Log.Information($"FastTrackService_{MethodName()}: END.");

        }


        public async Task AddRemark(FnolViewModel viewModel)
        {
            Log.Information($"FastTrackService_{MethodName()}: Start.");

            // create note base on remark
            var note = new Note
            {
                TimeStamp = DateTime.Now,
                FnolId = (int)viewModel.FnolID,
                NoteText = viewModel.AdditionalRemarks,
                UserID = 1999, // QCS user FOR NOW
                Author = "Fast Track User"
            };

            viewModel.AdditionalRemarks = string.Empty;

            // post note to server
            string path = $"api/Note";
            var response = await _http.PostAsJsonAsync(path, note);
            var responseContent = response.Content.ReadFromJsonAsync<Note>();

            if (response.IsSuccessStatusCode)
            {
                viewModel.Notes.Add(new NoteViewModel
                {
                    NoteID = responseContent.Result.NoteID,
                    NoteText = responseContent.Result.NoteText,
                    Username = "QCS Admin", // FOR NOW
                    TimeStamp = DateTime.Now,
                });

                Log.Information($"FastTrackService_{MethodName()}: Successfully added note to server.");
            }
            else
            {
                Log.Error($"FastTrackService_{MethodName()}: Failed to add note to server.");
            }
        }


        public async Task<HttpResponseMessage> ApproveFastTrack(FnolViewModel viewModel)
        {
            Log.Information($"FastTrackService_{MethodName()}: Start.");
            Stopwatch fullSubmissionStopwatch = Stopwatch.StartNew();
            Task addFnolAttachmentsTask = null;
            Task addPhotosExpressAttachmentsTask = null;
            try
            {
                string pdfPath = $"api/Pdf";
                if (viewModel.FastTrackFeatureTier >= 1)
                {
                    // Step 1: Generate the PDF and add to the file list.
                    var pdfResponse1 = await _http.PostAsJsonAsync(pdfPath, viewModel);

                    string? responseAsString = pdfResponse1.Content.ReadAsStringAsync().Result.Replace("\"", string.Empty);
                    byte[] pdfBytes = Convert.FromBase64String(responseAsString);

                    // Build list for table insertion
                    var pdfFile = new FileAttachmentViewModel
                    {
                        DateEntered = DateTime.Now,
                        FnolId = (int)viewModel.FnolID,
                        FileName = $"FastTrack_Summary.pdf",
                        Title = $"FastTrackPdf",
                        Stream = pdfBytes
                    };

                    viewModel.FnolImageList.Add(pdfFile);

                    viewModel.Signature.DateEntered = DateTime.Now;
                    viewModel.FnolImageList.Add(viewModel.Signature);

                    // Step 2: Create lists for upload to server and for insertion into Fnol_Attachments.
                    addFnolAttachmentsTask = Task.Run(async () =>
                    {
                        await AddFnolAttachments(viewModel.FnolImageList);
                    });

                    // Step 3: Update FNOL Status
                    viewModel.StatusTypeID = Convert.ToInt16(FnolStatusEnum.Completed);
                    await SaveFnolChanges(viewModel);
                }

                if (viewModel.FastTrackFeatureTier >= 2)
                {

                    // Step 1: Add a Photos Express to the database based on the FNOL data.
                    PhotosExpress photosExpress = _mapper.Map<PhotosExpress>(viewModel);

                    var response = await _http.PostAsJsonAsync($"api/PhotosExpress", photosExpress);

                    // Step 2a: Add the images to server and PhotosExpress_Attachment
                    if (response.IsSuccessStatusCode)
                    {
                        viewModel.PhotosExpressID = int.Parse(await response.Content.ReadAsStringAsync());

                        viewModel.FnolImageList.ForEach(attachment => attachment.PhotosExpressId = (int)viewModel.PhotosExpressID);

                        addPhotosExpressAttachmentsTask = Task.Run(async () =>
                        {
                            await AddPhotosExpressAttachments(viewModel.FnolImageList);
                        });

                        //Step 2b: Add PhotosExpressID to Additional Remarks
                        var notesResponse = await _http.PutAsJsonAsync($"api/Note/{viewModel.PhotosExpressID}", viewModel.FnolID);
                    }
                    else
                    {
                        // Unable to parse the ID from the response
                        Log.Error($"FastTrackService_{MethodName()}: Unable to parse PhotosExpressId from api/PhotosExpress/Post. ");
                    }

                    // Step 3: Kafka Update
                    var kafkaResponse = await _http.PostAsJsonAsync("api/Kafka", photosExpress);

                    // Step 4: Update the EAS_Inbound table
                    EAS_Inbound inbound = new EAS_Inbound
                    {
                        DateEntered = DateTime.Now,
                        LastUpdate = DateTime.Now,
                        AssignmentId = (int?)photosExpress.PhotosExpressID,
                        ClaimNumber = photosExpress.ClaimNumber,
                        Message = "PhotosExpress FastTrack Draft",
                        Status = "Open",
                        AssignmentType = "Photos_Express",
                        AssignmentStatusType = "Photos_Uploaded",
                        AuditorEmail = photosExpress.AuditorEmail ?? "N/A",
                        QcsRep = photosExpress.QcsRepresentative,
                        Source = "Owner"
                    };

                    var easResponse = await _http.PostAsJsonAsync("api/EAS_Inbound", inbound);
                }

                // Await the task if it's not null, otherwise return a completed Task
                await (addFnolAttachmentsTask != null ? addFnolAttachmentsTask : Task.CompletedTask).ConfigureAwait(false);
                await (addPhotosExpressAttachmentsTask != null ? addPhotosExpressAttachmentsTask : Task.CompletedTask).ConfigureAwait(false);

                // Send the Email
                await SendFastTrackEmail(viewModel);

                Log.Information($"FastTrackService_{MethodName()}: End.");
                fullSubmissionStopwatch.Stop();
                return new HttpResponseMessage(HttpStatusCode.OK);
                // Implement Tier Level 3 when necessary.
            }
            catch (Exception ex)
            {
                Log.Error($"FastTrackService_{MethodName()}: Encountered an exception. {ex.Message}.");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                throw;
            }
        }


        public async Task<FnolViewModel> GetFnolViewModels(decimal fnolId)
        {
            try
            {
                //Log.Information($"FastTrackService_{MethodName()}: Start.");
                string path = $"api/Fnol/{fnolId}";

                FnolViewModel FnolViewModel = await _http.GetFromJsonAsync<FnolViewModel>(path);

                if (FnolViewModel == null)
                {
                    return null;
                }

                FnolViewModel.InitializeImages();

                FnolViewModel.Signature = new FileAttachmentViewModel()
                {
                    Title = "Signature",
                    PhotosExpressAttachmentTypeID = 22,
                    ChangeTypeId = 2,
                    FileName = "Signature.png",
                    FnolId = (int?)FnolViewModel.FnolID
                };

                // Get all fnolAttachments
                List<Fnol_Attachments> fnolAttachments = await _http.GetFromJsonAsync<List<Fnol_Attachments>>($"api/Fnol_Attachments/{fnolId}");

                // Set Signature to the signature attachment
                FnolViewModel.Signature = _mapper.Map(fnolAttachments.Where(file => file.Description == "Signature").FirstOrDefault(), FnolViewModel.Signature);

               

                // Try to match Fnol_Attachments to their corresponding place in the image list.
                if (fnolAttachments != null)
                {
                    // Set FastTrackSummaryPdf to the FastTrackSummary attachment
                    var fastTrackAttachment = fnolAttachments?.FirstOrDefault(file => file.Description != null && file.Description.Contains("FastTrack"));
                    FnolViewModel.FastTrackSummaryPdf = _mapper.Map(fastTrackAttachment, FnolViewModel.FastTrackSummaryPdf);

                    foreach (Fnol_Attachments fileAttachment in fnolAttachments)
                    {
                        var matchingImage = FnolViewModel.FnolImageList.FirstOrDefault(image => image.Title == fileAttachment.Description);

                        if (matchingImage != null)
                        {
                            int imagesIndex = FnolViewModel.FnolImageList.IndexOf(matchingImage);
                            FnolViewModel.FnolImageList[imagesIndex] = _mapper.Map(fileAttachment, matchingImage);
                        }
                    }
                }

                // Get Existing Notes
                FnolViewModel.Notes = await _http.GetFromJsonAsync<List<NoteViewModel>>($"api/Note/fnol/{fnolId}");

                Log.Information($"FastTrackService_{MethodName()}:END: Returning Fnol with Id: {FnolViewModel.FnolID}.");
                return FnolViewModel;
            }
            catch (Exception ex)
            {
                Log.Error($"FastTrackService_{MethodName()}: Encountered an exception. {ex.Message}.");
                throw;
            }
        }


        public async Task<HttpResponseMessage> SaveFnolChanges(FnolViewModel viewModel)
        {
            try
            {
                Log.Information($"FastTrackService_{MethodName()}: Start.");

                Fnol fnol = new Fnol();
                _mapper.Map(viewModel, fnol);
                string path = $"api/Fnol";
                var response = await _http.PutAsJsonAsync(path, fnol);

                Log.Information($"FastTrackService_{MethodName()}:END: Updating contact information for Fnol with Id: {viewModel.FnolID}");
                return response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Log.Error($"FastTrackService_{MethodName()}: Encountered an exception. {ex.Message}.");
                throw;
            }
        }


        public async Task SendFastTrackEmail(FnolViewModel viewModel)
        {
            FileAttachmentViewModel fastTrackSummaryPdf = viewModel.FnolImageList.Where(file => file.Title == "FastTrackPdf").FirstOrDefault();

            if (fastTrackSummaryPdf == null)
            {
                Log.Error($"FastTrackService_{MethodName()}: Unable to find FastTrackPdf in the file list.");
                return;
            }

            ZeptoEmail zeptoEmail = new ZeptoEmail
            {
                From =
                    new FromEmail
                    {
                        Address = "noreply@qcsdirect.com"
                    },
                To =
                    new List<ToEmail>
                    {
                        new ToEmail
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = viewModel.ContactPrimaryEmail,
                                Name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(viewModel.ContactFirstName + " " + viewModel.ContactLastName.ToLower().Trim())
                            }
                        }
                    },
                Subject = $"FastTrack Initiated, {System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(viewModel.ContactFirstName.ToLower().Trim())}!",
                HtmlBody =
                    @$"<div>Your FastTrack claim {viewModel.InsuredVehicleVin} has been initiated! Download the attachment to see a summary.</div><br /><br /><br /><br />
<div><i>Questions or comments?  Contact Quality Claims Solutions at FILL IN CONTACT INFORMATION </i></div>
<div><i>Please do not respond to this email, as the inbox is not tracked.</i></div>",
                Attachments =
                    new List<ZeptoAttachment>
                    {
                new ZeptoAttachment
                {
                    Content = "", // Base64 string, filled out in controller
                    MimeType = "application/pdf",
                    Name = "FastTrackSummary.pdf",
                    FilePath = fastTrackSummaryPdf.Path
                }
                    }
            };

            List<string> ProgramCodeEmailAddresses = viewModel.FastTrackEmailAddress.Split(',').ToList();

            foreach (var email in ProgramCodeEmailAddresses)
            {
                zeptoEmail.To.Add(new ToEmail
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = email,
                        Name = email.Substring(0, email.IndexOf("@"))
                    }
                });
            }

            await _http.PostAsJsonAsync("api/Email", zeptoEmail);
        }



    }
}
