using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NIIEPayAPI.Data;
using NIIEPayAPI.Models;

namespace NIIEPayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingsController : ControllerBase
    {
        private readonly NiiepayContext _context;

        public SavingsController(NiiepayContext context)
        {
            _context = context;
        }

        // POST: api/savings/open
        [HttpPost("open")]
        public async Task<IActionResult> OpenSavings([FromBody] SavingsOpenRequest request)
        {
            // 1. Kiểm tra tài khoản
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == request.AccountNumber);

            if (account == null)
                return BadRequest(new { status = "FAIL", message = "Không tìm thấy tài khoản." });

            // 2. Kiểm tra số tiền và số dư
            if (request.Amount <= 0)
                return BadRequest(new { status = "FAIL", message = "Số tiền gửi phải lớn hơn 0." });

            if (account.AvailableBalance - request.Amount < 50000)
                return BadRequest(new { status = "FAIL", message = "Số dư không đủ (phải còn ít nhất 50.000 VND sau gửi)." });

            // 3. Kiểm tra kỳ hạn
            var interestRate = await _context.InterestRates
                .FirstOrDefaultAsync(r => r.TermMonths == request.TermMonths);

            if (interestRate == null)
                return BadRequest(new { status = "FAIL", message = "Kỳ hạn không hợp lệ." });

            // 4. Tính ngày đáo hạn
            var startDate = DateTime.Now;
            var maturityDate = startDate.AddMonths(request.TermMonths);

            // 5. Tạo sổ tiết kiệm
            var saving = new SavingsAccount
            {
                AccountId = account.Id,
                Amount = request.Amount,
                TermMonths = request.TermMonths,
                InterestRate = interestRate.InterestRate1,
                AutoRenew = request.AutoRenew,
                StartDate = DateOnly.FromDateTime(startDate),       
                MaturityDate = DateOnly.FromDateTime(maturityDate),
                Status = "open"
            };

            _context.SavingsAccounts.Add(saving);

            // 6. Cập nhật số dư tài khoản
            account.AvailableBalance -= request.Amount;
            _context.Accounts.Update(account);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "SUCCESS",
                message = "Mở sổ tiết kiệm thành công",
                savingId = saving.Id,
                accountNumber = request.AccountNumber,
                amount = request.Amount,
                interestRate = interestRate.InterestRate1,
                maturityDate = maturityDate
            });
        }

        // GET: api/savings/rates
        [HttpGet("rates")]
        public async Task<IActionResult> GetRates()
        {
            var rates = await _context.InterestRates
                .OrderBy(r => r.TermMonths)
                .ToListAsync();

            return Ok(rates);
        }
    }
}
