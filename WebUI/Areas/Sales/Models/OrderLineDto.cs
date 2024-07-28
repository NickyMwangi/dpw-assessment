using Library.Dtos;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUI.Areas.Sales.Models
{
    public class OrderLineDto: BaseLineDto
    {
        [AdaptMember("Transaction_No")]
        public override string HeaderId { get; set; } = string.Empty;
        public int LineNo { get; set; } = 0;
        [Required]
        [DisplayName("Order No")]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        public string ProductType { get; set; } = string.Empty;

        [Required]
        public decimal? ProductCostPrice { get; set; } = 0;

        [Required]
        public decimal? ProductSalesPrice { get; set; } = 0;

        [Required]
        public decimal? Quantity { get; set; } = 0;
    }
}
