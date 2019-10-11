using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Areas.Admin.Models.Request.Customer;
using OnlineStore.Areas.Admin.Models.Response.Customer;
using OnlineStore.Database;
using OnlineStore.Models.Database;

namespace OnlineStore.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Admin")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        public CustomerController(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public IEnumerable<CustomerResponseModel> Get()
        {
            return _mapper.Map<List<Customer>, List<CustomerResponseModel>>(_databaseContext.Customers.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerRequestModel value)
        {
            if (ModelState.IsValid)
            {
                await _databaseContext.Customers.AddAsync(_mapper.Map<CustomerRequestModel, Customer>(value));
                await _databaseContext.SaveChangesAsync();
                return Ok();
            }

            return BadRequest(); //TODO Specify errors
        }

        // PUT: api/Items/5
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CustomerRequestModel value)
        {
            if (ModelState.IsValid)
            {
                value.Id = id;
                var entry = _mapper.Map<CustomerRequestModel, Customer>(value);
                _databaseContext.Entry(entry).State = EntityState.Modified;
                await _databaseContext.SaveChangesAsync();
                return Ok();
            }

            return BadRequest(); //TODO Specify errors
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            var entry = await _databaseContext.Customers.FirstOrDefaultAsync(e => e.Id == id);
            if (entry != null)
            {
                _databaseContext.Customers.Remove(entry);
                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}