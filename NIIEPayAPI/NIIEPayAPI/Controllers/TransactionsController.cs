using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NIIEPayAPI.Data;

namespace NIIEPayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly NiiepayContext _context;

        public TransactionsController(NiiepayContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Xem lịch sử giao dịch theo accountNumber và khoảng thời gian
        /// </summary>
        /// <param name="accountNumber">Số tài khoản</param>
        /// <param name="fromDate">Ngày bắt đầu (yyyy-MM-dd)</param>
        /// <param name="toDate">Ngày kết thúc (yyyy-MM-dd)</param>
        /// <returns>Danh sách giao dịch</returns>
        [HttpGet]
        public async Task<IActionResult> GetTransactionHistory(
            [FromQuery] string accountNumber,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            // Tìm account theo accountNumber
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account == null)
                return NotFound(new { status = "FAIL", message = "Không tìm thấy tài khoản." });

            // Query lịch sử giao dịch theo accountId
            var query = _context.TransactionHistories
                                .Include(th => th.Transaction)
                                .Where(th => th.AccountId == account.Id)
                                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(th => th.TransactionTime >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(th => th.TransactionTime <= toDate.Value);

            var result = await query
                .OrderByDescending(th => th.TransactionTime)
                .Select(th => new
                {
                    th.TransactionId,
                    AccountHolder = account.AccountHolderName,
                    accountNumber = account.AccountNumber,
                    th.Amount,
                    th.TransactionTime,
                    th.BalanceAfter,
                    th.Note,
                    IsSender = th.IsSender
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
