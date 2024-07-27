using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities.Service
{
    public class SmsSettingsModel
    {
        public SmsSettingsModel()
        {
            BaseUrl = "localhost";
        }
        [DisplayName("Base URL")]
        [Required]
        public string BaseUrl { get; set; }

        [DisplayName("Token Api")]
        public string TokenApi { get; set; }

        [DisplayName("Api Key")]
        [DataType(DataType.Password)]
        public string ApiKey { get; set; }

        [DisplayName("Api Secret")]
        [DataType(DataType.Password)]
        public string ApiSecret { get; set; }

        [DisplayName("Sms Api Name")]
        public string ApiName { get; set; }
        [DisplayName("Device Id")]
        public string DeviceId { get; set; }
        [DisplayName("Corporate No")]
        public string CorporateNo { get; set; }
    }
}
