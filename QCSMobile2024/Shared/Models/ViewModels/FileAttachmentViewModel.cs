namespace QCSMobile2024.Shared.Models.ViewModels
{
    public class FileAttachmentViewModel
    {
        public int? FnolId { get; set; }

        public int? PhotosExpressId { get; set; }

        public string? Title { get; set; }

        public string? ThumbnailHtmlId 
        {
            get
            {
                return Title + "-thumbnail";
            } 
        }

        public string? ThumbnailHtmlClass { get; set; }

        public string? Path { get; set; }

        public byte[]? Stream { get; set; }

        public string? FileName { get; set; }

        public string? ContentType { get; set; }

        public DateTime? DateEntered { get; set; }

        public int? PhotosExpressAttachmentTypeID { get; set; }

        public int? ChangeTypeId { get; set; }
    }
}
