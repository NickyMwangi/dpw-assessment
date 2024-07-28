using Library.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dtos
{
    public class BaseDto<T>
         where T : BaseLineDto, new()
    {

        public string Id { get; set; } = string.Empty;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy hh:mm tt}")]
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy hh:mm tt}")]
        public DateTime? ModifiedOn { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = string.Empty;
        public bool ViewOnly { get; set; } = false;
        public bool NewDto { get; set; } = true;
        public virtual IList<T> DtoLines { get; set; } = [];
        public virtual string RowActionsHtml { get; set; } = string.Empty;
    }
}
