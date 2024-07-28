using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.DropdownList;
using Library.Dtos;
using Library.Utility;
using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUI.Extensions.Helpers;

namespace WebUI.Areas.Sales.Models
{
    public class OrderDto : BaseDto<OrderLineDto>
    {
        [Required]
        [DisplayName("Order No")]
        public string No { get; set; } = string.Empty;

        [Required]
        [DisplayName("Order Type")]
        [UIHint("option")]
        public string Type { get; set; } = string.Empty;

        [Required]
        [UIHint("option")]
        public string Status { get; set; } = string.Empty; //changes automcatically when order moves from one stage to the other.

        [Required]
        public string CustomerName { get; set; } = string.Empty;
        public DateTime? OrderDate { get; set; } = DateTime.Now;


        [AdaptMember("OrderLine")]
        [AdaptIgnore(MemberSide.Source)]
        public override IList<OrderLineDto>? DtoLines { get; set; } = [];

        #region Lists
        public IEnumerable<SelectListItem> OrderTypeList { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> OrderStatusList { get; set; } = Enumerable.Empty<SelectListItem>(); // For the sake of the demo    =
        #endregion
    }
}
