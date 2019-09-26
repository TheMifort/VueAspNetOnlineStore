using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Areas.Admin.Models.Request.User;
using OnlineStore.Areas.Admin.Models.Response.Customer;
using OnlineStore.Areas.Admin.Models.Response.User;
using OnlineStore.Database;
using OnlineStore.Models.Database;

namespace OnlineStore.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(DatabaseContext databaseContext, IMapper mapper, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserResponseModel>> Get()
        {
            var result = new List<UserResponseModel>();
            foreach (var user in _userManager.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserResponseModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = roles.ToList(),
                    Customer = _mapper.Map<Customer, CustomerResponseModel>(user.Customer),

                    AllRoles = _roleManager.Roles.Select(e => e.Name).ToList(),
                    AllCustomers =
                        _mapper.Map<List<Customer>, List<CustomerResponseModel>>(_databaseContext.Customers
                            .ToList()) //TODO IEnumerable?
                });
            }

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.TryAddModelError("Model.Invalid", "Модель не является валидной");
                return BadRequest(ModelState);
            }

            var identityUser = new User
            {
                UserName = model.UserName,
            };
            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                {
                    ModelState.TryAddModelError(e.Code, e.Description);
                }

                return new BadRequestObjectResult(ModelState);
            }

            return new OkResult();
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UserRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                user.Customer = await _databaseContext.Customers.FirstOrDefaultAsync(e => e.Id == model.Customer.Id);

                var userRoles = await _userManager.GetRolesAsync(user);
                var addedRoles = model.Roles.Except(userRoles).ToList();
                var removedRoles = userRoles.Except(model.Roles).ToList();

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                await _databaseContext.SaveChangesAsync();
                return Ok();
            }

            return BadRequest(); //TODO Specify errors
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var entry = await _userManager.FindByIdAsync(id);
            if (entry != null)
            {
                await _userManager.DeleteAsync(entry);
                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}