using EF_DatabaseInheritance.Data;
using EF_DatabaseInheritance.DTOs;
using EF_DatabaseInheritance.Entities;
using EF_DatabaseInheritance.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EF_DatabaseInheritance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        private readonly EF_DatabaseInheritanceDbContext _context;

        public PaymentsController(EF_DatabaseInheritanceDbContext context)
        {
            _context = context;
        }

        // ---------------- GET ALL PAYMENTS ----------------
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPayments()
        {

            var payments = await _context.Payments.ToListAsync();

            if(payments.Count == 0)
                return NotFound("No payments found.");

            var result = payments.Select(p => new PaymentReadDTO
            {
                Id = p.Id,
                Amount = p.Amount,
                Currency = p.Currency,
                PaymentGateway = p.PaymentGateway,
                PaymentStatus = p.PaymentStatus.ToString(),
                PaymentDate = p.PaymentDate,
                PaymentType = p.GetType().Name
            }).ToList();

            return Ok(result);
        }           

        // POST api/<PaymentsController>
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentCreateDTO paymentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Payment payment;

            switch(paymentDto.PaymentType.ToLower())
            {
                case "card":

                    if (string.IsNullOrWhiteSpace(paymentDto.CardNumber) ||
                       string.IsNullOrWhiteSpace(paymentDto.CardHolderName) ||
                       !paymentDto.ExpiryMonth.HasValue || !paymentDto.ExpiryYear.HasValue ||
                       string.IsNullOrWhiteSpace(paymentDto.CVV))
                        return BadRequest("Card payment requires CardNumber, CardHolderName, ExpiryMonth, ExpiryYear, and CVV.");

                    payment = new CardPayment
                    {
                        Amount = paymentDto.Amount,
                        Currency = paymentDto.Currency,
                        PaymentGateway = paymentDto.PaymentGateway,
                        OrderId = paymentDto.OrderId,
                        TransactionId = paymentDto.TransactionId,
                        PaymentDate = DateTime.UtcNow,
                        PaymentStatus = PaymentStatus.Pending,
                        CardNumber = paymentDto.CardNumber!,
                        CardHolderName = paymentDto.CardHolderName!,
                        ExpiryMonth = paymentDto.ExpiryMonth.Value,
                        ExpiryYear = paymentDto.ExpiryYear.Value,
                        CVV = paymentDto.CVV!
                    };
                    break;

                case "upi":
                    if (string.IsNullOrWhiteSpace(paymentDto.UpiId))
                        return BadRequest("UPI payment requires UpiId.");
                    payment = new UPIPayment
                    {
                        Amount = paymentDto.Amount,
                        Currency = paymentDto.Currency,
                        PaymentGateway = paymentDto.PaymentGateway,
                        OrderId = paymentDto.OrderId,
                        TransactionId = paymentDto.TransactionId,
                        PaymentDate = DateTime.UtcNow,
                        PaymentStatus = PaymentStatus.Pending,
                        UpiId = paymentDto.UpiId!,
                        AppName = paymentDto.AppName,
                        TransactionRefNo = paymentDto.TransactionRefNo
                    };
                    break;

                case "wallet":
                    if (string.IsNullOrWhiteSpace(paymentDto.WalletType))
                        return BadRequest("Wallet payment requires WalletType.");
                    payment = new WalletPayment
                    {
                        Amount = paymentDto.Amount,
                        Currency = paymentDto.Currency,
                        PaymentGateway = paymentDto.PaymentGateway,
                        OrderId = paymentDto.OrderId,
                        TransactionId = paymentDto.TransactionId,
                        PaymentDate = DateTime.UtcNow,
                        PaymentStatus = PaymentStatus.Pending,
                        WalletType = paymentDto.WalletType!,
                        WalletBalanceUsed = paymentDto.WalletBalanceUsed ?? 0,
                        CashbackReceived = paymentDto.CashbackReceived ?? 0
                    };
                    break;

                default:
                    return BadRequest("Invalid PaymentType. Allowed values: Card, UPI, Wallet.");

            }

            await _context.Payments.AddAsync(payment);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"{paymentDto.PaymentType} payment created successfully",
                PaymentId = payment.Id
            });  
        }

        // ---------------- GET PAYMENT BY ID ----------------
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {

            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
                return NotFound($"Payment with ID {id} not found.");

            object detailed;

            switch(payment)
            {
                case CardPayment card:
                    detailed = new
                    {
                        card.Id,
                        card.Amount,
                        card.Currency,
                        card.PaymentGateway,
                        card.PaymentStatus,
                        card.PaymentDate,
                        PaymentType = "Card",
                        card.CardHolderName,
                        card.ExpiryMonth,
                        card.ExpiryYear,
                        card.MaskedCard
                    };
                    break;

                case UPIPayment upi:
                    detailed = new
                    {
                        upi.Id,
                        upi.Amount,
                        upi.Currency,
                        upi.PaymentGateway,
                        upi.PaymentStatus,
                        upi.PaymentDate,
                        PaymentType = "UPI",
                        upi.UpiId,
                        upi.AppName,
                        upi.TransactionRefNo
                    };
                    break;

                case WalletPayment wallet:
                    detailed = new
                    {
                        wallet.Id,
                        wallet.Amount,
                        wallet.Currency,
                        wallet.PaymentGateway,
                        wallet.PaymentStatus,
                        wallet.PaymentDate,
                        PaymentType = "Wallet",
                        wallet.WalletType,
                        wallet.WalletBalanceUsed,
                        wallet.CashbackReceived
                    };
                    break;

                default:
                    detailed = payment;
                    break;
            }   

            return Ok(detailed);

        }
    }
}
