using System.ComponentModel.DataAnnotations;

namespace EF_DatabaseInheritance.DTOs
{
    public class UserCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        [RegularExpression("Customer|Seller|Admin", ErrorMessage = "Invalid UserType. Allowed values: Customer, Seller, Admin.")]
        public string UserType { get; set; } = null!;

        // ---------- Customer-specific ----------
        public string? PhoneNumber { get; set; }

        public string? ShippingAddress { get; set; }

        public string? BillingAddress { get; set; }

        // ---------- Seller-specific ----------
        public string? BusinessName { get; set; }

        public string? BusinessLicenseNo { get; set; }

        public string? BankAccountNumber { get; set; }

        public string? GSTNumber { get; set; }

        public string? PANNumber { get; set; }

        public string? WarehouseAddress { get; set; }

        public string? SupportPhone { get; set; }

        public bool? IsVerified { get; set; }

        // ---------- Admin-specific ----------
        public string? RoleName { get; set; }

        public string? Department { get; set; }

        public string? Permissions { get; set; }
    }

    public class UserReadDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string UserType { get; set; } = null!;
    }

    public class ValidateUserAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            var dto = (UserCreateDTO)validationContext.ObjectInstance;
            var type = dto.UserType.ToLower();

            switch (type)
            {
                case "customer":
                    if (string.IsNullOrWhiteSpace(dto.PhoneNumber) ||
                        string.IsNullOrWhiteSpace(dto.ShippingAddress) ||
                        string.IsNullOrWhiteSpace(dto.BillingAddress))
                        return new ValidationResult("Customer must have PhoneNumber, ShippingAddress, and BillingAddress.");
                    break;

                case "seller":
                    if (string.IsNullOrWhiteSpace(dto.BusinessName) ||
                        string.IsNullOrWhiteSpace(dto.BusinessLicenseNo) ||
                        string.IsNullOrWhiteSpace(dto.BankAccountNumber))
                        return new ValidationResult("Seller must have BusinessName, BusinessLicenseNo, and BankAccountNumber.");
                    break;

                case "admin":
                    if (string.IsNullOrWhiteSpace(dto.RoleName) ||
                        string.IsNullOrWhiteSpace(dto.Department))
                        return new ValidationResult("Admin must have RoleName and Department.");
                    break;

                default:
                    return new ValidationResult("Invalid UserType. Allowed values: Customer, Seller, Admin.");
            }

            return ValidationResult.Success;
        }
    }
}
