using LoanAPI.Data;
using LoanAPI.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Finall_Project.Services
{
     public class LoanService : ILoanService
        {
            private readonly UserContext _loans;

            public LoanService(UserContext loans)
            {
                _loans = loans;
            }

            public Loan AddLoan(Loan loan)
            {
                 _loans.Loans.Add(loan);
                _loans.SaveChanges();
                return loan;
            }
            public Loan GetLoanById(int id)
            {
                return _loans.Loans.Include(a => _loans.Loans).FirstOrDefault(x => x.Id == id);
            }
            public List<Loan> GetAll()
            {
                return _loans.Loans.Include(a => _loans.Loans).ToList();
            }
            public Loan Update(Loan loan)
            {
                _loans.Loans.Update(loan);
                _loans.SaveChanges();
                return loan;
            }
            public void DeleteLoanById(int id)
            {
                Loan loan = _loans.Loans.FirstOrDefault(x => x.Id == id);

                _loans.Loans.Remove(loan);
                _loans.SaveChanges();
                return;
            }
        }
}
