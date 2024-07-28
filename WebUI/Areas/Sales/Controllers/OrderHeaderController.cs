using Data.Entities;
using Data.Entities.Sales;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
    }
}
