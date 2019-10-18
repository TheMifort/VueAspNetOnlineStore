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
    [Authorize(Roles = "User,Manager")]
    [Area("Items")]
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
            List<Order> orders;
            if (!User.IsInRole("Manager"))
                orders = user.Customer?.Orders?.ToList() ?? new List<Order>();

            else orders = _context.Orders.ToList();

            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderResponseModel>>(orders);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var customer = (user).Customer;
                if (customer == null)
                    return BadRequest();

                var order = new Order {OrderItems = new List<OrderItem>()};

                foreach (var orderItem in model.OrderItems)
                {
                    var item = await _context.Items.FirstOrDefaultAsync(e => e.Id == orderItem.ItemId);
                    if(item == null)continue;

                    var mappedOrderItem = new OrderItem
                    {
                        Item = item,
                        ItemPrice = item.Price - item.Price * customer.Discount,
                        ItemsCount = orderItem.ItemsCount
                    };

                    order.OrderItems.Add(mappedOrderItem);
                }

                order.OrderNumber = (await _context.Orders.OrderByDescending(e => e.OrderNumber).FirstOrDefaultAsync())
                                    ?.OrderNumber + 1 ?? 0;
                order.Status = OrderStatus.New;
                order.OrderDate = DateTime.Now;

                customer.Orders.Add(order);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest(); //TODO Specify errors
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, ConfirmOrderRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(e => e.Id == id);
                if (model.Complete)
                {
                    order.Status = OrderStatus.Completed;
                }
                else
                {
                    order.ShipmentDate = model.ShipmentDate;
                    order.Status = OrderStatus.Performed;
                }

                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest(); //TODO Specify errors
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            var entry = await _context.Orders.FirstOrDefaultAsync(e => e.Id == id);
            if (entry != null)
            {
                var user = await _userManager.GetUserAsync(User);
                if(entry.Customer.Users.Exists(e=>e.Id == user.Id) && entry.Status == OrderStatus.New || User.IsInRole("Manager"))
                    _context.Orders.Remove(entry);

                await _context.SaveChangesAsync();
            }
        }
    }
}