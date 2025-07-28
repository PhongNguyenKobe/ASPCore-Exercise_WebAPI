namespace NIIEPayAPI.Models
{
    public class SavingsOpenRequest
    {
        public string AccountNumber { get; set; }    
        public decimal Amount { get; set; }          
        public int TermMonths { get; set; }          // (1,2,3,6,9,12,18,24,36)
        public bool AutoRenew { get; set; }          
    }
}
