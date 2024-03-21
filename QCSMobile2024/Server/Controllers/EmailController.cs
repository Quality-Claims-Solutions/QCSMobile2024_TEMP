using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QCSMobile2024.Shared.Models.CustomModels;
using log4net;
using System.Net;
using System.Text;
using System.Runtime.CompilerServices;

namespace QCSMobile2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        public HttpClient _http { get; }
        private readonly ILog Log;


        public EmailController(HttpClient http, ILog logger)
        {
            _http = http;
            Log = logger;
        }

        static string MethodName([CallerMemberName] string name = null) => name;

        [HttpPost]
        public async Task<ActionResult> SendEmail(ZeptoEmail zeptoEmail)
        {
            Log.Info($"EmailController_{MethodName()} START: Sending Email");

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var baseAddress = "https://api.zeptomail.com/v1.1/email";

            // Configuration of HTTP Environment
            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";
            http.PreAuthenticate = true;
            http.Headers.Add("Authorization", "Zoho-enczapikey wSsVR61//kWkW6p6mzT4J7o+m1pWB1r1Rx9+jQOmv3GvSK3A/Mc7xhCbDQajSfhJE2FsRzUTprx7nhcC2jAL2d0szw0JCSiF9mqRe1U4J3x17qnvhDzKW2tckReKJY8JzgVtnWlkFs0j+g==");

            // Convert file to Base64 and add to the model.
            string base64Content;
            try
            {
                string filePath = zeptoEmail.Attachments.FirstOrDefault().FilePath; 
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                base64Content = Convert.ToBase64String(fileBytes);
                Log.Info($"EmailController_{MethodName()}  Converted File to Base64");

            }
            catch (Exception ex)
            {
                Log.Error($"EmailController_{MethodName()} ERROR: Retreiving a file. Exception: {ex.Message}.");
                return BadRequest();
            }
            zeptoEmail.Attachments[0].Content = base64Content;
            
            // Convert the model to a JSON object and then to a byte array.
            JObject parsedContent = JObject.Parse(JsonConvert.SerializeObject(zeptoEmail, Formatting.Indented));
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(parsedContent.ToString());

            // Write Stream
            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();
            Log.Info($"EmailController_{MethodName()} Wrote Stream,");

            try
            {
                // Send the Request
                var response = http.GetResponse();
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                Log.Info($"EmailController_{MethodName()} RETURN: Email Sent");
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error($"EmailController_{MethodName()} ERROR: Sending Email, Exception: {ex.Message}.");
            }

            return BadRequest();
        }
    }
}
