using LoanAPI.Data;
using System.Collections.Generic;

namespace Finall_Project.Services
{
    public interface ILoanService
    {
        Loan AddLoan(Loan loan);
        Loan GetLoanById(int id);
        List<Loan> GetAll();
        Loan Update(Loan loan);
        void DeleteLoanById(int id);

    }
}
