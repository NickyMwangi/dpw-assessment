using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("GeneralSettings")]
    public partial class GeneralSetting : BaseEntity
    {
        [MaxLength(128)]
        public string SettingType { get; set; } = string.Empty;
        [MaxLength(128)]
        public string Description { get; set; } = string.Empty;
        [MaxLength]
        public string SettingValue { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        [StringLength(128)]
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; } = DateTime.Now;
        [StringLength(128)]
        public string ModifiedBy { get; set; } = string.Empty;
    }
}
