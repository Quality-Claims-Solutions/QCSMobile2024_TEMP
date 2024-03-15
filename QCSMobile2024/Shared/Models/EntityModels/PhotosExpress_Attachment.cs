using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCSMobile2024.Shared.Models.EntityModels
{
    public class PhotosExpress_Attachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal PhotosExpressAttachmentID { get; set; }

        public decimal PhotosExpressID { get; set; }

        public DateTime DateEntered { get; set; }

        public string? FileName { get; set; }

        public string Path { get; set; }

        public int PhotosExpressAttachmentTypeID { get; set; }

        public int ChangeTypeID { get; set; }

        public bool? Deleted { get; set; }

        public string? AddedBy { get; set; }

        public int? PhotosExpressSupplementId { get; set; }

        public bool InternalOnly { get; set; }

        public bool WebServicePickedUp { get; set; }

    }
}
