using Finall_Project.Helpers;
using Finall_Project.Repository;
using Finall_Project.Validators;
using LoanAPI.Data;
using LoanAPI.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Finall_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserContext _Usercontext;
        private readonly AppSettings _appSettings;
        private readonly UserService _userService;
        public UserController(UserContext context, IOptions<AppSettings> appSettings, UserService userService)
        {
            _Usercontext = context;
            _appSettings = appSettings.Value;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
      /*  public IActionResult Login([FromBody] LoginUser userModel)
        {
            var user = _userService.Login(userModel);
            if (user == null)
                return BadRequest("Username or Password is incorrect");

            string tokenString = GenerateToken(user);
            return Ok(new
            {
                user.Id,
                user.Username,
                user.Role,
                Token = tokenString
            });
        }*/
        public IActionResult Login([FromBody] LoginUser userModel)
        {

            var user = _userService.Login(userModel);
            if (userModel == null)
                return BadRequest(new { message = "Username or Password is incorrect" });
            string tokenString = GenerateToken(userModel);
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });

        }

        //[Authorize(Roles = Role.Admin)]
        [AllowAnonymous]

        [HttpPost("adduser")]
         public ActionResult<User> AddUser(User user)
         {
             var validator = new UserValidator();
             var result = validator.Validate(user);
             List<string> errorsList = new();
             if (!result.IsValid)
             {
                 foreach (var item in result.Errors)
                 {
                     errorsList.Add(item.ErrorMessage);
                 }
                 return BadRequest(errorsList);
             }

             //if (!User.IsInRole(Role.Admin))
             //    return Forbid();

             user.Password = PasswordHasher.hashPass(user.Password);

             if (_userService.AddUser(user) == null)
             {
                 return BadRequest($"User with username {user.Username} is already registered");
             }
             try
             {
                 _userService.AddUser(user);
             }
             catch (DbUpdateException e)
             {
                 return BadRequest($"{e.InnerException.Message}.\nLeave the ID fields empty");
             }

             return Created("", user);
         }
        [HttpGet("get/{id}")] //TODO: mxolod tavisi ID it unda uchandes info
        public IActionResult GetUserById(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);
                if (id != currentUserId && !User.IsInRole(Role.Admin))
            return Forbid();
                var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
      
        [Authorize(Roles = Role.Admin)]
        [HttpGet("get all")]
        public IActionResult GetAllUsers()
           {
               var users = _userService.GetAll();
               return
                   Ok(users);
           }

        [HttpPut("update/{UserId}")]
        public IActionResult UpdatUseretById(User user)
        {
            var validator = new UserValidator();
            var result = validator.Validate(user);
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
                _userService.Update(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound($"There is no user to update with id {user.Id}");
            }

            return Ok(user);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPut("Blockuser/{ChangeStatus}")]
        public IActionResult blockuser(int id, User user)
        {
            if (!User.IsInRole(Role.Admin))
                return Forbid("Only Administrator can update status");
            if (!User.IsInRole(Role.Admin))
                return Forbid("Only Administrator can update status");
            try
            {
                _Usercontext.Update(id);
            }
            catch (ArgumentNullException)
            {
                return NotFound($"There is no User with id {id}");
            }

            return Ok($"User with id {id} is blocked");
        
    }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteUserById(int id)
        {
            if (!User.IsInRole(Role.Admin))
                return Forbid("Only Administrator can delete users");
            try
            {
                _Usercontext.Remove(id);
            }
            catch (ArgumentNullException)
            {
                return NotFound($"There is no User with id {id}");
            }

            return Ok($"User with id {id} deleted");
        }


        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
