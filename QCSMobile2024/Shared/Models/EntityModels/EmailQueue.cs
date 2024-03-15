namespace QCSMobile2024.Shared.Models.CustomModels
{
    public class EmailQueue
    {
        public decimal Id { get; set; }

        public decimal? NoteId { get; set; }

        public string? NoteText { get; set; }

        public DateTime DateEntered { get; set; }

        public DateTime? DateSent { get; set; }

        public string? SendError { get; set; }

        public int SendCount { get; set; }

        public string? Subject { get; set; }

        public string? Sender { get; set; }

        public string? Recipients { get; set; }

        public string? Attachments { get; set; }

        public string? BodyText { get; set; }

        public string? BodyHtml { get; set; }

        public bool Encrypted { get; set; }

        public string? ClaimedBy { get; set; }

        public DateTime? ClaimedDate { get; set; }

    }
}
