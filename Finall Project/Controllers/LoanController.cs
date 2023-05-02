using LoanAPI.Data;
using LoanAPI.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Finall_Project.Validators;
using Finall_Project.Services;

namespace Finall_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly UserContext _loancontext;
        public LoanController(ILoanService loanService,UserContext loanContext)
        {
            _loanService = loanService;
            _loancontext = loanContext;
        }

        [HttpPost("addloan")]
        public ActionResult<Loan> AddLoan(Loan loan)
        {
            var validator = new LoanValidator();
            var result = validator.Validate(loan);
            List<string> errorsList = new();
            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    errorsList.Add(item.ErrorMessage);
                }
                return BadRequest(errorsList);
            }
          /*  if (User.Isblocked == "False") //todo: if isblocked is true, ar unda hqondes sesxis motxovnis ufleba
            {
                return BadRequest("Blocked user can't add Loan");
            }*/
            try
            {
                _loancontext.Add(loan);
            }
            catch (DbUpdateException e)
            {
                return BadRequest($"{e.InnerException.Message}.\nLeave the ID fields empty");
            }
            return Created("", loan);
        }
        [HttpGet("get/{id}")] //TODO: mxolod tavisi ID it unda uchandes info
        public IActionResult GetLoanById(int id)
        {
            var currentuserId = int.Parse(User.Identity.Name);
            if (id != currentuserId)
                return Forbid();
            var loan = _loanService.GetLoanById(id);
            if (loan == null)
                return NotFound();

            return Ok(loan);
        }
        [Authorize(Roles = Role.Admin)]
        [HttpGet("get all")]
        public IActionResult GetAllLoans()
        {
            var users = _loanService.GetAll();
            return
                Ok(users);
        }
        [Authorize(Roles = Role.Admin)]
        [HttpPut("update/{LoanID}")]
        public IActionResult UpdateLoanById(Loan loan)
        {
            var validator = new LoanValidator();
            var result = validator.Validate(loan);
            List<string> errorsList = new();
            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    errorsList.Add(item.ErrorMessage);
                }
                return BadRequest(errorsList);
            }
            try
            {
                _loanService.Update(loan);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound($"There is no Loan to update with id {loan.Id}");
            }
            return Ok(loan);
        }
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteLoanById(int id)
        {
            try
            {
                _loanService.DeleteLoanById(id);
            }
            catch (ArgumentNullException)
            {
                return NotFound($"There is no loan with id {id}");
            }
            return Ok($"Loan with id {id} deleted"); ;
        }
       
    }


}
