using LoanAPI.Data;
using LoanAPI.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Finall_Project.Validators;
using Finall_Project.Services;
using Finall_Project.Enums;
using System.Security.Claims;
using LoggerService;
using FluentValidation;
using System.Linq;
using static Finall_Project.Validators.LoanValidator;
using Finall_Project.Helpers;
using Microsoft.Extensions.Options;

namespace Finall_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly ILoggerManager _logger;
        private readonly JwtTokenHelper _jwtHelper;

        public LoanController(ILoanService loanService, 
                              ILoggerManager logger, 
                              JwtTokenHelper jwtHelper)
        {
            _loanService = loanService;
            _logger = logger;
            _jwtHelper = jwtHelper; 
        }

        [HttpPost("addloan")]
        public ActionResult<UserModifyLoan> AddLoan([FromBody] UserModifyLoan newloan)
        {
            _logger.LogInfo("get info about add loan ");
            var validator = new UpdateLoanValidator();
            var result = validator.Validate(newloan);
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
                    var loanToAdd = new Loan
                    {
                        LoanType = newloan.LoanType,
                        Amount = newloan.Amount,
                        Currency = newloan.Currency,
                        LoanPeriod = newloan.LoanPeriod,
                        Status = Status.InProcess,
                        UserId = _jwtHelper.GetCurrentId()
                    };
                    _loanService.AddLoan(loanToAdd);
                }
                catch (DbUpdateException e)
                {
                    return BadRequest($"{e.InnerException.Message}.\nLeave the ID fields empty");
                }
                return Created("", newloan);
            
            }

        [HttpGet("get/{id}")]
        public IActionResult GetLoanByUserId(int id)
        {
            _logger.LogInfo("get info by id ");
            if (id != _jwtHelper.GetCurrentId())
            {
                return BadRequest("can't get information about another user");
            }
            
            var loan = _loanService.GetLoanByUserId(id);
            if (loan == null)
                return NotFound();

            return Ok(loan);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet("getallloans")]
        public IActionResult GetAllLoans()
        {
            if (!User.IsInRole(Role.Admin))
                return Forbid("Only Administrator can use this feature");
            var loans = _loanService.GetAll();
            return
                Ok(loans);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPut("changeloanstatus/{id}")]
        public IActionResult ChangeLoanstatus(int id, Status loanstatus)
        {
            if (!User.IsInRole(Role.Admin))
                return Forbid("Only Administrator can update loanstatus");
            try
            {
                var loan = _loanService.GetLoanById(id);
                if (loan == null)
                {
                    return NotFound($"There is no loan with id {id}");
                }
                _loanService.ChangeLoanstatus(id, loanstatus);

                return Ok($"Loan with id {id} changed status to {loanstatus}");
            }
            catch (ArgumentNullException)
            {
                return NotFound($"There is no Loan with id {id}");
            }
        }

        [HttpPut("updateloan/{id}")]
        public IActionResult UpdateLoanById(int id, [FromBody] UserModifyLoan updateLoan)
        {
            _logger.LogInfo("get info about loan update");
            var existingLoan = _loanService.GetLoanById(id);
            if (existingLoan == null)
            {
                return NotFound("Loan not found");
            }

            
            if (existingLoan.UserId != _jwtHelper.GetCurrentId())
            {
                return Forbid();
            }

            if (existingLoan.Status != Status.InProcess)
            {
                return BadRequest("Loan can only be updated if it is in InProcess status");
            }

            var validator = new UpdateLoanValidator();
            var result = validator.Validate(updateLoan);
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
                var loanToUpdate = new Loan
                {
                    Id = id,
                    LoanType = updateLoan.LoanType,
                    Amount = updateLoan.Amount,
                    Currency = updateLoan.Currency,
                    LoanPeriod = updateLoan.LoanPeriod,
                    Status = existingLoan.Status,
                    User = existingLoan.User,
                    UserId = existingLoan.UserId
                };
                _loanService.UpdateLoanById(loanToUpdate);
            }
            catch (DbUpdateException e)
            {
                return BadRequest($"{e.InnerException.Message}.\nLeave the ID fields empty");
            }

            return Ok("Loan updated successfully");
        }

        [HttpDelete("deleteloan/{id}")]
        public IActionResult DeleteLoanById(int id)
        {
            _logger.LogInfo("get info about loan delete ");
            var existingLoan = _loanService.GetLoanById(id);
            var loan = _loanService.GetLoanById(id);
            if (User.IsInRole(Role.Admin) || loan.Status == Status.InProcess
                && existingLoan.UserId == _jwtHelper.GetCurrentId())
            {
                _loanService.DeleteLoanById(id);
                return Ok($"Loan with id {id} deleted");
            }
            if(existingLoan.UserId != _jwtHelper.GetCurrentId())
            {
                return Forbid();
            }
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
