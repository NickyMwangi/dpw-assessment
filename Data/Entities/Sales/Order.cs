using Library.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Sales
{
    public class Order: BaseEntity
    {
        public string No { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        [InverseProperty(nameof(OrderLine.Order))]
        public virtual ICollection<OrderLine> orderLines { get; set; }
    }
}
