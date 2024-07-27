using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities.Service
{
    public class SmtpSettingsModel
    {
        public SmtpSettingsModel()
        {
            SmtpServer = "localhost";
            SmtpPort = 25;
            EnableSsl = false;
            SmtpFromDisplayName = "NavPortal";
        }
        [DisplayName("SMTP Server Name")]
        [Required]
        public string SmtpServer { get; set; }

        [DisplayName("SMTP Port")]
        public int? SmtpPort { get; set; }

        [DisplayName("Enable SSL")]
        public bool? EnableSsl { get; set; }

        [DisplayName("SMTP User Name")]
        public string SmtpUserName { get; set; }

        [DisplayName("SMTP Password")]
        [DataType(DataType.Password)]
        public string SmtpPassword { get; set; }

        [DisplayName("SMTP From Address")]
        [Required]
        [EmailAddress]
        public string SmtpFromAddress { get; set; }

        [DisplayName("SMTP From Display Name")]
        [Required]
        public string SmtpFromDisplayName { get; set; }

        [DisplayName("Max. Retry Count")]
        [Required]
        public int MaxRetryCount { get; set; }
    }
}
