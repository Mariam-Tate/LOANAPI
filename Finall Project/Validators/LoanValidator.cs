using FluentValidation;
using LoanAPI.Data;

namespace Finall_Project.Validators
{
    public class LoanValidator : AbstractValidator<Loan>
    {
        public LoanValidator()
        {
            RuleFor(x => x.LoanType)
                    .NotEmpty().WithMessage("Type is required")
                    .IsInEnum().WithMessage("Type should be Fast, Auto or Installment");
            RuleFor(x => x.Amount)
                    .NotNull().WithMessage("Amount cannot be empty")
                    .InclusiveBetween(0, 100000);
            RuleFor(x => x.Currency)
                    .NotEmpty().WithMessage("Currency is required")
                    .IsInEnum().WithMessage("Currency should be GEL,USD or EUR");
            RuleFor(x => x.LoanPeriod)
                    .NotNull().WithMessage("LoanPeriod cannot be empty")
                    .InclusiveBetween(1, 20);
            RuleFor(x => x.Status)
                   .NotEmpty().WithMessage("Status is required")
                   .IsInEnum().WithMessage("Status should be In_process, Approved or Rejected");
        }
        public class UpdateLoanValidator : AbstractValidator<UpdateLoanByID>
        {
            public UpdateLoanValidator()
            {

                RuleFor(x => x.LoanType)
                        .NotEmpty().WithMessage("Type is required")
                        .IsInEnum().WithMessage("Type should be Fast, Auto or Installment");
                RuleFor(x => x.Amount)
                        .NotNull().WithMessage("Amount cannot be empty")
                        .InclusiveBetween(0, 100000);
                RuleFor(x => x.Currency)
                        .NotEmpty().WithMessage("Currency is required")
                        .IsInEnum().WithMessage("Currency should be GEL,USD or EUR");
                RuleFor(x => x.LoanPeriod)
                        .NotNull().WithMessage("LoanPeriod cannot be empty")
                        .InclusiveBetween(1, 20);
            }
        }
    }
}
