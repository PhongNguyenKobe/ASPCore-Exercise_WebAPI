namespace NIIEPayAPI.Models
{
    public class TransferRequest
    {
        public string FromAccount { get; set; }
        public string ToAccountOrPhone { get; set; }
        public string ToBankCode { get; set; } // null nếu internal
        public decimal Amount { get; set; }
        public string Note { get; set; }
    }
}
