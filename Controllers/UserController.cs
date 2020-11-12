using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bitify.Data;
using bitify.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bitify.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {


        private DataContext _dbcontext;
        public UserController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>>  GetUsers()
        {
            return await _dbcontext.Users.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _dbcontext.Users.FindAsync(id);
        }

        
    }
}
