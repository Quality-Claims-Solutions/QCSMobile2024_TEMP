using Newtonsoft.Json;

namespace QCSMobile2024.Shared.Models.CustomModels
{
    public class ZeptoEmail
    {
        [JsonProperty("from")]
        public FromEmail From { get; set; }

        [JsonProperty("to")]
        public List<ToEmail> To { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("htmlbody")]
        public string HtmlBody { get; set; }

        [JsonProperty("attachments")]
        public List<ZeptoAttachment> Attachments { get; set; }
    }


    public class FromEmail
    {
        [JsonProperty("address")]
        public string Address { get; set; }
    }


    public class ToEmail
    {
        [JsonProperty("email_address")]
        public EmailAddress EmailAddress { get; set; }
    }


    public class EmailAddress
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }


    public class ZeptoAttachment
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string FilePath { get; set; }
    }
}
