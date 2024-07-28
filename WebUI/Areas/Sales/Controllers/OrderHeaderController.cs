using Data.Entities;
using Data.Entities.Sales;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Controllers;
using WebUI.Areas.Sales.Models;

namespace WebUI.Areas.Sales.Controllers
{
    [Area("Sales")]
    public class OrderHeaderController : BaseController<Order, OrderDto, OrderLineDto>
    {

        public OrderHeaderController(IRepoService _repo, IMapperService _mapper, IIdentityService _idService)
            : base(_repo, _mapper)
        {
        }


        public override OrderDto PopulateSelectLists(OrderDto dto)
        {
            // Normally I would create a lookup/OptionsList table to store all the options
            dto.OrderTypeList = [
                new SelectListItem { Text = "Normal", Value = "Normal" },
                new SelectListItem { Text = "Staff", Value = "Staff" },
                new SelectListItem { Text = "Mechanical", Value = "Mechanical" },
                new SelectListItem { Text = "Perishable", Value = "Perishable" }
            ];

            dto.OrderStatusList = [
                new SelectListItem { Text = "New", Value = "Normal" },
                new SelectListItem { Text = "Processsing", Value = "Processsing" },
                new SelectListItem { Text = "Complete", Value = "Complete" },
                new SelectListItem { Text = "On Hold", Value = "On Hold" }
            ];
            return base.PopulateSelectLists(dto);
        }

        public override OrderDto DefaultValuesGet(OrderDto dto, bool isNew, string queryParam)
        {
            dto.Status = "New";
            return base.DefaultValuesGet(dto, isNew, queryParam);
        }
    }
}
