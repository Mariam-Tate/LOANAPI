using Finall_Project.Enums;
using Finall_Project.Helpers;
using Finall_Project.Services;
using Finall_Project.Validators;
using LoanAPI.Data;
using LoanAPI.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Finall_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly AppSettings _appSettings;
        private readonly IUserService _userService;

        public UserController(IOptions<AppSettings> appSettings, IUserService userService)
        {

            _appSettings = appSettings.Value;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUser userModel)
        {
            var user = _userService.Login(userModel);
            if (user == null)
                return BadRequest("Username or Password is incorrect");

            string tokenString = GenerateToken(user);
            return Ok(new
            {
                user.UserName,
                user.Role,
                tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("adduser")]
        public ActionResult<User> AddUser([FromBody] User user)
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

            if (user.Role == Role.Admin)
                return BadRequest("Cannot create an Admin user");

            if (user.IsBlocked)
            {
                return BadRequest("Cannot set isBlocked to true");
            }

            user.Password = PasswordHasher.HashPass(user.Password);

            var existingUser = _userService.GetUserByUsername(user.UserName);
            if (existingUser != null)
            {
                return BadRequest($"User with username {user.UserName} already exists");
            }
            try
            {
                _userService.AddUser(user);
            }

            catch (DbUpdateException e)
            {
                return BadRequest($"{e.InnerException.Message}.\nLeave the ID fields empty");
            }

            return Ok(user);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            //if (id != GetCurrentId(HttpContext))
            //{
            //    return BadRequest("can't get information about another user");
            //}
            return Ok(user);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet("getallusers")]
        public IActionResult GetAllUsers()
        {
            if (!User.IsInRole(Role.Admin))
                return Forbid("Only Administrator can use this feature");

            var users = _userService.GetAll();
            return
          
                Ok(users);
        }

        [HttpPut("updateuser/{id}")]
        public IActionResult UpdatUserById(int id, [FromBody] User user)
        {
            //if (id != GetCurrentId(HttpContext)) //tu mivutite zevit swori da qvevit araswori id mainc cvlis
            //{
            //    return BadRequest("can't update another user");
            //}
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
            if (user.Role == Role.Admin)
            {
                return BadRequest("User Cannot change the role");
            }
            if (user.IsBlocked)
            {
                return BadRequest("Cannot set isBlocked to true");
            }

            user.Password = PasswordHasher.HashPass(user.Password);
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
        [HttpPut("blockuser/{id}")]
        public IActionResult BlockUser(int id, [FromBody] bool isBlocked)
        {
            if (!User.IsInRole(Role.Admin))
                return Forbid("Only Administrator can update status"); //ar agdebs am mesijs

            try
            {
                var user = _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound($"There is no User with id {id}");
                }
                _userService.BlockUser(id, isBlocked);

                return Ok($"User with id {id} has changed isBlocked status to {isBlocked}");
            }
            catch (ArgumentNullException)
            {
                return NotFound($"There is no User with id {id}");
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("deleteuser/{id}")]
        public IActionResult DeleteUserById(int id)
        {
            if (!User.IsInRole(Role.Admin))
                return Forbid("Only Administrator can delete users");
            try
            {
                _userService.Delete(id);
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
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
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

       // //if you uncomment this, code goes to fetch error, it was working fine in the morning :)

        //public int GetCurrentId(HttpContext context)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        //    var token = context.Request.Headers["Authorization"].ToString().Split(" ")[1];
        //    var tokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(key),
        //        ValidateIssuer = false,
        //        ValidateAudience = false
        //    };
        //    var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
        //    var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
        //    if (userIdClaim != null)
        //    {
        //        var userId = int.Parse(userIdClaim.Value);
        //        return userId;
        //    }
        //    return -1;
        //}
    }
}
