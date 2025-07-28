namespace NIIEPayAPI.Models
{
    public class AccountRegisterRequest
    {
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string PhoneNumber { get; set; }
        public string CitizenId { get; set; }
        public DateTime IdExpiryDate { get; set; }
        public decimal InitialBalance { get; set; }
    }
}
