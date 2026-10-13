using EF_DatabaseInheritance.Data;
using EF_DatabaseInheritance.DTOs;
using EF_DatabaseInheritance.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EF_DatabaseInheritance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EF_DatabaseInheritanceDbContext _context;

        public UsersController(EF_DatabaseInheritanceDbContext context)
        {
            _context = context;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO createUser)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = createUser.UserType.ToLower() switch
            {
                "customer" => new Customer
                {
                    Name = createUser.Name,
                    Email = createUser.Email,
                    Username = createUser.Username,
                    PasswordHash = createUser.PasswordHash,
                    PhoneNumber = createUser.PhoneNumber,
                    ShippingAddress = createUser.ShippingAddress,
                    BillingAddress = createUser.BillingAddress
                },

                "seller" => new Seller
                {
                    Name = createUser.Name,
                    Email = createUser.Email,
                    Username = createUser.Username,
                    PasswordHash = createUser.PasswordHash,
                    BusinessName = createUser.BusinessName!,
                    BusinessLicenseNo = createUser.BusinessLicenseNo!,
                    BankAccountNumber = createUser.BankAccountNumber!,
                    GSTNumber = createUser.GSTNumber!,
                    PANNumber = createUser.PANNumber!,
                    WarehouseAddress = createUser.WarehouseAddress!,
                    SupportPhone = createUser.SupportPhone,
                    IsVerified = createUser.IsVerified ?? false
                },

                "admin" => new AdminUser
                {
                    Name = createUser.Name,
                    Email = createUser.Email,
                    Username = createUser.Username,
                    PasswordHash = createUser.PasswordHash,
                    RoleName = createUser.RoleName!,
                    Department = createUser.Department!,
                    Permissions = createUser.Permissions
                },
                _ => throw new Exception("Invalid user type")
            };
            
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            return Ok(new { message = $"{createUser.UserType} created successfully", userId = user.Id });
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
