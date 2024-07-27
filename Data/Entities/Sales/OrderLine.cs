using Library.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Sales
{
    public class OrderLine: BaseEntity
    {
        public int LineNo { get; set; } = 0;
        public string ProductCode { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public Decimal ProductCostPrice { get; set; } = 0;

        [Column(TypeName = "decimal(10,2)")]
        public Decimal ProductSalesPrice { get; set; } = 0;

        [Column(TypeName = "decimal(10,2)")]
        public Decimal Quantity { get; set; } =0;
    }
}
