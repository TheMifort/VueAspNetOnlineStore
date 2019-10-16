using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Areas.Items.Models.Request.Items;
using OnlineStore.Areas.Items.Models.Response.Items;
using OnlineStore.Database;
using OnlineStore.Models.Database;

namespace OnlineStore.Areas.Items.Controllers
{
    [Route("api/[area]")]
    [ApiController]
    [Area("Items")]
    public class ItemController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public ItemController(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        // GET: api/Items
        [HttpGet("{filter?}")]
        public IEnumerable<ItemResponseModel> GetAll() //TODO Filters
        {
            return _mapper.Map<IEnumerable<Item>, IEnumerable<ItemResponseModel>>(_databaseContext.Items.ToList());
        }

        // GET: api/Items/5
        [HttpGet("{id:guid}", Name = "Get")]
        public ItemResponseModel Get(Guid id)
        {
            return _mapper.Map<Item, ItemResponseModel>(_databaseContext.Items.FirstOrDefault(e => e.Id == id));
        }

        // POST: api/Items
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ItemRequestModel value)
        {
            if (ModelState.IsValid)
            {
                await _databaseContext.Items.AddAsync(_mapper.Map<ItemRequestModel, Item>(value));
                await _databaseContext.SaveChangesAsync();
                return Ok();
            }

            return BadRequest(); //TODO Specify errors
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ItemRequestModel value)
        {
            if (ModelState.IsValid)
            {
                value.Id = id;
                var entry = _mapper.Map<ItemRequestModel, Item>(value);
                _databaseContext.Entry(entry).State = EntityState.Modified;
                await _databaseContext.SaveChangesAsync();
                return Ok();
            }

            return BadRequest(); //TODO Specify errors
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entry = await _databaseContext.Items.FirstOrDefaultAsync(e => e.Id == id);
            if (entry != null)
            {
                _databaseContext.Items.Remove(entry);
                await _databaseContext.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }
    }
}