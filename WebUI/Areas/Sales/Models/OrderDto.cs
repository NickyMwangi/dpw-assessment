using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Dtos;
using Mapster;

namespace WebUI.Areas.Sales.Models
{
    public class OrderDto : BaseDto<OrderLineDto>
    {
        [Required]
        [DisplayName("Order No")]
        public string No { get; set; } = string.Empty;

        [Required]
        [DisplayName("Order Type")]
        public string Type { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        public string CustomerName { get; set; } = string.Empty;
        //[AdaptMember("OrderLineDto")]
        //[AdaptIgnore(MemberSide.Source)]
        //public override IList<OrderLineDto>? DtoLines { get; set; }
    }
}
