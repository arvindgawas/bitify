using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bitify.Data;
using bitify.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bitify.Controllers
{
   
    public class UserController : BaseApiController
    {


        private DataContext _dbcontext;
        public UserController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>>  GetUsers()
        {
            return await _dbcontext.Users.ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _dbcontext.Users.FindAsync(id);
        }

        
    }
}
