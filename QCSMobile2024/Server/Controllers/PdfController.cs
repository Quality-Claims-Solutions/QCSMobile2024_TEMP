using AutoMapper;
using log4net;
using Microsoft.AspNetCore.Mvc;
using QCSMobile2024.Shared.Models;
using QCSMobile2024.Shared.Models.ViewModels;
using QCSMobile2024.Utilities;
using System.Runtime.CompilerServices;

namespace QCSMobile2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : Controller
    {
        private readonly IMapper _mapper;

        private Db _db;
        public HttpClient _http { get; }
        private readonly ILog Log;

        public PdfController(Db db, IMapper mapper, HttpClient http, ILog logger)
        {
            _db = db;
            _mapper = mapper;
            _http = http;
            Log = logger;

        }

        static string MethodName([CallerMemberName] string name = null) => name;

        // Generates PDF and returns as a byte array
        [HttpPost]
        public async Task<ActionResult> Post(FnolViewModel viewModel)
        {
            Log.Info($"PdfController_{MethodName()}:START: Create PDF for Fnol with Id: {viewModel.FnolID}.");
            try
            {
                string htmlString = PDFGenerator.GetFastTrackPdf(viewModel);

                if (htmlString != null)
                {
                    var renderer = new ChromePdfRenderer();
                    PdfDocument? pdf = renderer.RenderHtmlAsPdf(htmlString);
                   
                    byte[] result = pdf.Stream.ToArray();
                    pdf.Stream.Dispose();

                    Log.Info($"RETURN: Created pdf and sent to server at Fnol_Vehicle & PhotosExpress aswell as added to Database in Fnol_Attachments & PhotosExpress_Attachment.");
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error($"We encountered an error in creating a PDF for Fnol with Id: {viewModel.FnolID}. Exception: {ex.Message}");
                return BadRequest();
            }
        }
    }
}
