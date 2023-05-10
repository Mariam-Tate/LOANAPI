using LoanAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Finall_Project.Services
{
    public interface ILoanService
    {
        Loan AddLoan(Loan loan);
        Loan GetLoanById(int id);
        List<Loan> GetAll();
        Loan UpdateLoan(Loan loan);
        Loan UpdateLoanById(Loan loan);
        void DeleteLoanById(int id);

    }
}
