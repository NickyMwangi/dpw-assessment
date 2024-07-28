using Library.Helpers;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dtos
{
    public class BaseLineDto
    {

        [AdaptMember("Id")]
        public string LineId { get; set; } = string.Empty;
        public virtual string HeaderId { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy hh:mm tt}")]
        public DateTime? ModifiedOn { get => DateTime.Now; set { } }
        public string ModifiedBy { get => string.Empty; set { } }
        public bool NewDto { get; set; }
    }
}
