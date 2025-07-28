using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NIIEPayAPI.Data;
using NIIEPayAPI.Models;

namespace NIIEPayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly NiiepayContext _context;

        public AccountsController(NiiepayContext context)
        {
            _context = context;
        }

        // POST: api/accounts/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterRequest request)
        {
            // Check trùng số tài khoản
            if (await _context.Accounts.AnyAsync(a => a.AccountNumber == request.AccountNumber))
                return BadRequest(new { status = "FAIL", message = "Số tài khoản đã tồn tại." });

            // Check số dư ban đầu
            if (request.InitialBalance < 100000)
                return BadRequest(new { status = "FAIL", message = "Số dư ban đầu phải ≥ 100.000 VND." });

            // Check CCCD còn hạn
            if (request.IdExpiryDate <= DateTime.Now)
                return BadRequest(new { status = "FAIL", message = "Căn cước công dân đã hết hạn." });

            var account = new Account
            {
                AccountNumber = request.AccountNumber,
                AccountHolderName = request.AccountHolderName,
                PhoneNumber = request.PhoneNumber,
                CitizenId = request.CitizenId,
                IdExpiryDate = DateOnly.FromDateTime(request.IdExpiryDate), // Convert DateTime to DateOnly
                AvailableBalance = request.InitialBalance
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return Ok(new { status = "SUCCESS", accountId = account.AccountNumber, message = "Tạo tài khoản thành công" });
        }

        // GET: api/accounts/{accountNumber}
        [HttpGet("{accountNumber}")]
        public async Task<IActionResult> GetAccount(string accountNumber)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account == null)
                return NotFound(new { status = "FAIL", message = "Không tìm thấy tài khoản." });

            return Ok(account);
        }
    }
}
