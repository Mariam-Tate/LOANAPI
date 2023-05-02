

namespace LoanAPI.Data
{
    public class Loan
    {
        public int Id { get; set; }
        public loanType LoanType { get; set; }
        public enum loanType
        {
            Fast = 1,          
            Auto = 2,          
            Installment = 3 }; //ganvadeba
        public int Amount { get; set; }
        public currency Currency { get; set; }
        public enum currency
        {
            GEL,
            USD,
            EUR
        };
        public int LoanPeriod { get; set; }
        public status Status { get; set; }
        public enum status
        {
            In_process,
            Approved,
            Rejected
        };
        public User User { get; set; }
        public int UserID { get; set; }
    }
}
