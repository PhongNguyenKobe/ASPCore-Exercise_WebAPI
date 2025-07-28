using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NIIEPayAPI.Data;
using NIIEPayAPI.Models;
using System;

namespace NIIEPayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private readonly NiiepayContext _context;

        public TransfersController(NiiepayContext context)
        {
            _context = context;
        }

        // ========== 1. Chuyển khoản nội bộ ==========
        [HttpPost("internal")]
        public async Task<IActionResult> InternalTransfer([FromBody] TransferRequest request)
        {
            var fromAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == request.FromAccount);

            if (fromAccount == null)
                return BadRequest(new { status = "FAIL", message = "Tài khoản nguồn không tồn tại." });

            var toAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == request.ToAccountOrPhone
                    || a.PhoneNumber == request.ToAccountOrPhone);

            if (toAccount == null)
                return BadRequest(new { status = "FAIL", message = "Tài khoản hoặc số điện thoại người nhận không tồn tại." });

            if (request.Amount <= 0)
                return BadRequest(new { status = "FAIL", message = "Số tiền phải lớn hơn 0." });

            if (fromAccount.AvailableBalance - request.Amount < 50000)
                return BadRequest(new { status = "FAIL", message = "Số dư không đủ (phải còn ít nhất 50.000 VND sau chuyển)." });

            // Thực hiện chuyển khoản
            fromAccount.AvailableBalance -= request.Amount;
            toAccount.AvailableBalance += request.Amount;

            // Tạo giao dịch
            var txnId = "TXN" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var transaction = new Transaction
            {
                TransactionId = txnId,
                TransactionType = "internal",
                FromAccount = fromAccount.Id,
                ToAccount = toAccount.Id,
                Amount = request.Amount,
                Note = request.Note
            };
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            // Ghi lịch sử cho người gửi
            _context.TransactionHistories.Add(new TransactionHistory
            {
                TransactionId = transaction.Id,
                AccountId = fromAccount.Id,
                Amount = -request.Amount,
                BalanceAfter = fromAccount.AvailableBalance,
                Note = request.Note,
                IsSender = true
            });

            // Ghi lịch sử cho người nhận
            _context.TransactionHistories.Add(new TransactionHistory
            {
                TransactionId = transaction.Id,
                AccountId = toAccount.Id,
                Amount = request.Amount,
                BalanceAfter = toAccount.AvailableBalance,
                Note = request.Note,
                IsSender = false
            });

            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "SUCCESS",
                transactionId = txnId,
                timestamp = DateTime.Now,
                remainingBalance = fromAccount.AvailableBalance
            });
        }

        // ========== 2. Chuyển khoản liên ngân hàng ==========
        [HttpPost("external")]
        public async Task<IActionResult> ExternalTransfer([FromBody] TransferRequest request)
        {
            var fromAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == request.FromAccount);

            if (fromAccount == null)
                return BadRequest(new { status = "FAIL", message = "Tài khoản nguồn không tồn tại." });

            if (string.IsNullOrEmpty(request.ToBankCode))
                return BadRequest(new { status = "FAIL", message = "Phải nhập mã ngân hàng cho giao dịch liên ngân hàng." });

            if (request.Amount <= 0)
                return BadRequest(new { status = "FAIL", message = "Số tiền phải lớn hơn 0." });

            if (fromAccount.AvailableBalance - request.Amount < 50000)
                return BadRequest(new { status = "FAIL", message = "Số dư không đủ (phải còn ít nhất 50.000 VND sau chuyển)." });

            // Trừ tiền
            fromAccount.AvailableBalance -= request.Amount;

            // Tạo giao dịch
            var txnId = "TXN" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var transaction = new Transaction
            {
                TransactionId = txnId,
                TransactionType = "external",
                FromAccount = fromAccount.Id,
                ToBankCode = request.ToBankCode,
                ToPhone = request.ToAccountOrPhone,
                Amount = request.Amount,
                Note = request.Note
            };
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            // Ghi lịch sử
            _context.TransactionHistories.Add(new TransactionHistory
            {
                TransactionId = transaction.Id,
                AccountId = fromAccount.Id,
                Amount = -request.Amount,
                BalanceAfter = fromAccount.AvailableBalance,
                Note = request.Note,
                IsSender = true
            });

            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "SUCCESS",
                transactionId = txnId,
                timestamp = DateTime.Now,
                remainingBalance = fromAccount.AvailableBalance
            });
        }
    }
}
