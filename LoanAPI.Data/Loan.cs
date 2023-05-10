

using Finall_Project.Enums;
using System.ComponentModel.DataAnnotations;

namespace LoanAPI.Data
{
    public class Loan
    {
        public int Id { get; set; }
        public LoanType LoanType { get; set; }
        public int Amount { get; set; }
        public Currency Currency { get; set; }
        public int LoanPeriod { get; set; }
        public Status Status { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }

    public class UpdateLoanByID
    {
        [Required(ErrorMessage = "LoanType is required")]
        public LoanType LoanType { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Currency is required")]
        public Currency Currency { get; set; }
        [Required(ErrorMessage = "LoanPeriod is required")]
        public int LoanPeriod { get; set; }

    }

}

