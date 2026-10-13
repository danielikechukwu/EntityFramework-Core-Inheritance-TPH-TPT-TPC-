using EF_DatabaseInheritance.Data;
using EF_DatabaseInheritance.DTOs;
using EF_DatabaseInheritance.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            var result = users.Select(user => new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Username,
                UserType = user.GetType().Name
            });

            return Ok(result);
        }

        // ---------------- GET BY TYPE ----------------
        [HttpGet("GetByType/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            IEnumerable<User> list = type.ToLower() switch
            {
                "customer" => await _context.Customers.ToListAsync(),
                "seller" => await _context.Sellers.ToListAsync(),
                "admin" => await _context.AdminUsers.ToListAsync(),
                _ => new List<User>()
            };
            return Ok(list);
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

    }
}
