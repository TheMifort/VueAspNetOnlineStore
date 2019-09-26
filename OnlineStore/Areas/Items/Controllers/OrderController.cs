using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Areas.Items.Models.Request.Order;
using OnlineStore.Areas.Items.Models.Response.Order;
using OnlineStore.Database;
using OnlineStore.Models.Database;
using OnlineStore.Models.Enums;

namespace OnlineStore.Areas.Items.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public OrderController(DatabaseContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<OrderResponseModel>> Get()
        {
            var user = await _userManager.GetUserAsync(User);

            var orders = user.Customer?.Orders?.ToList() ?? new List<Order>();

            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderResponseModel>>(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = (await _userManager.GetUserAsync(User)).Customer;
                if (customer == null)
                    return BadRequest();

                var order = _mapper.Map<OrderRequestModel, Order>(model);
                order.OrderItems = new List<OrderItem>();
                foreach (var modelItem in model.OrderItems)
                {
                    var item = await _context.Items.FirstOrDefaultAsync(e => e.Id == modelItem.ItemId);
                    if (item == null) continue;
                    var discount = customer.Discount;

                    var orderItem = new OrderItem
                    {
                        Item = item,
                        ItemPrice = item.Price - discount > 0 ? item.Price * discount : 0,
                        ItemsCount =  modelItem.ItemsCount
                    };

                    order.OrderItems.Add(orderItem);
                }

                order.Status = OrderStatus.New;

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest(); //TODO Specify errors
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Confirm(Guid id, ConfirmOrderRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(e => e.Id == id);
                order.ShipmentDate = model.ShipmentDate;
                order.Status = OrderStatus.Performed;

                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest(); //TODO Specify errors
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            var entry = await _context.Orders.FirstOrDefaultAsync(e => e.Id == id);
            if (entry != null)
            {
                _context.Orders.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }
    }
}