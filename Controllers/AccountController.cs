using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using bitify.Data;
using bitify.Dtos;
using bitify.Entities;
using bitify.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace bitify.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private DataContext _dbcontext;
        private readonly ITokenService tokenService;

        public AccountController(DataContext dbcontext, ITokenService TokenService)
        {

            _dbcontext = dbcontext;
            tokenService = TokenService;
        }

        [HttpGet("servererror")]
        public ActionResult<string> GetServerError()
        {
            var thing = _dbcontext.Users.Find(-1);

            var thingtoreturn = thing.ToString();

            return thingtoreturn;

        }

        //[HttpPost("register")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> RegisterUser(RegisterDto objregister)
        {
            if (await IsUserExist(objregister.UserName)) return BadRequest("UserName is Taken.");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = objregister.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(objregister.Password)),
                PasswordSalt = hmac.Key
            };

            _dbcontext.Add(user);

            await _dbcontext.SaveChangesAsync();

            return new UserDto
            {
                UserName = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto objlogindto)
        {

            var user = await _dbcontext.Users.SingleOrDefaultAsync(x=> x.UserName == objlogindto.UserName);

            if (user == null) return Unauthorized("Invalid User");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(objlogindto.Password));

            for (int i=0; i< PasswordHash.Length;i++)
            {
                if (PasswordHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto
            {
                UserName = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }

        private async Task<bool> IsUserExist(string Username)
        {
            return await _dbcontext.Users.AnyAsync(x => x.UserName == Username.ToLower());
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        { 
            
            return await _dbcontext.Users.ToListAsync();
        }



    }
}

