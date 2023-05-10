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
        //private readonly UserService _userService;
        //private readonly AppSettings _appSettings;

        public LoanController(ILoanService loanService, ILoggerManager logger
             /* , UserService userService, AppSettings appSettings*/)
        {
            _loanService = loanService;
            _logger = logger;
            //_userService = userService;
            //_appSettings = appSettings;
        }

        [HttpPost("addloan")]
        public ActionResult<Loan> AddLoan([FromBody] Loan loan)
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
            if (loan.Status != Status.InProcess)
            {
                return BadRequest("Status should be In Process");
            }
            //if (user.IsBlocked)
            //{
            //    return Forbid();
            //}
            try
            {
                _loanService.AddLoan(loan);
            }
            catch (DbUpdateException e)
            {
                return BadRequest($"{e.InnerException.Message}.\nLeave the ID fields empty");
            }
            return Created("", loan);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetLoanById(int id)
        {

            _logger.LogInfo("get info by id ");
            //var currentuserId = int.Parse(User.Identity.Name);
            //if (id != currentuserId)
            //    return Forbid();
            var loan = _loanService.GetLoanById(id);
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
        [HttpPut("updateloans/{id}")]
        public IActionResult UpdateLoan([FromBody] Loan loan)
        {
            if (!User.IsInRole(Role.Admin))
                return Forbid("Only Administrator can use this feature");
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
                _loanService.UpdateLoan(loan);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound($"There is no Loan to update with id {loan.Id}");
            }
            return Ok(loan);
        }

        [HttpPut("updateloan/{id}")]
        public IActionResult UpdateLoanById(int id, [FromBody] UpdateLoanByID updateLoan)
        {
            var existingLoan = _loanService.GetLoanById(id);
            if (existingLoan == null)
            {
                return NotFound("Loan not found");
            }

            /*   var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
               if (existingLoan.UserID != currentUserId)
               {
                   return Forbid();
               }
            */
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
            var loan = _loanService.GetLoanById(id);
            if (User.IsInRole(Role.Admin) || loan.Status == Status.InProcess 
                /*&& loan.UserId == GetCurrentId(HttpContext)*/)
            {
                _loanService.DeleteLoanById(id);
                return Ok($"Loan with id {id} deleted");
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

        /* [HttpDelete("deleteloan/{id}")]
         public IActionResult DeleteLoanById(int id)
         {

             var appSettings = Options.Create(new AppSettings());
             var userService = new UserService();
             var userController = new UserController(appSettings, userService);
             var userId = userController.GetCurrentId(HttpContext);

             var loan = _loanService.GetLoanById(id);

             if (loan == null)
             {
                 return NotFound($"There is no loan with id {id}");
             }
             if (loan.UserId != userId)
             {
                 return BadRequest("can't get information about another user");
             }

             if (User.IsInRole(Role.Admin) || (loan.UserId == userId && loan.Status == Status.InProcess))
             {
                 _loanService.DeleteLoanById(id);
                 return Ok($"Loan with id {id} deleted");
             }

             return Forbid("You do not have permission to delete this loan");
         }
         /* if (!User.IsInRole(Role.Admin))
              return Forbid("Only Administrator can use this feature");
          try
          {
              _loanService.DeleteLoanById(id);
          }
          catch (ArgumentNullException)
          {
              return NotFound($"There is no loan with id {id}");
          }
          return Ok($"Loan with id {id} deleted");
      }*/

    }   
}
