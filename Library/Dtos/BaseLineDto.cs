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
        public string dtoUserName;
        public BaseLineDto()
        {
            dtoUserName = "";
        }
        [AdaptMember("Id")]
        public string LineId { get; set; }=string.Empty;
        public virtual string HeaderId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy hh:mm tt}")]
        public DateTime? ModifiedOn { get => DateTime.Now; set { } }
        public string ModifiedBy { get => dtoUserName; set { } }
        public bool NewDto { get; set; }
    }
}
