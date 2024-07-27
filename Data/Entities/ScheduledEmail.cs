using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public partial class ScheduledEmail : BaseEntity
    {
        [MaxLength(50)]
        public string Module { get; set; } = string.Empty;

        public string ReferenceId { get; set; } = string.Empty;

        public string ReferenceDescription { get; set; } = string.Empty;

        [MaxLength]
        public string EmailTo { get; set; } = string.Empty;

        [MaxLength]
        public string EmailCC { get; set; } = string.Empty;

        [MaxLength]
        public string Subject { get; set; } = string.Empty;

        [MaxLength]
        public string HtmlBody { get; set; } = string.Empty;

        public bool? IsHtml { get; set; }

        public bool? Sent { get; set; }

        public int? RetryCount { get; set; }

        [MaxLength]
        public string ErrorMessage { get; set; } = string.Empty;

        [MaxLength(128)]
        public string CreatedBy { get; set; } = string.Empty;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
    }
}
