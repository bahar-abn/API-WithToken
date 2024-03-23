using API.Data;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class APIController : ControllerBase
    {
            private readonly ApplicationDbContext _dbContext;

            public APIController()
            {
                _dbContext = new ApplicationDbContext();
            }

            [HttpPost]
            [Route("generateToken")]
            public IActionResult GenerateToken()
            {
                string randomToken = GenerateRandomToken();

                _dbContext.Users.Add(new User { Name = "User", Token = randomToken });
                _dbContext.SaveChanges();

                return Ok("Token generated and saved in the database.");
            }

            [HttpGet]
            [Route("authenticate")]
            public IActionResult AuthenticateUser()
            {
                string token = Request.Headers["Authorization"].FirstOrDefault();

                var user = _dbContext.Users.FirstOrDefault(u => u.Token == token);

                if (user != null)
                {
                    return Ok($"User authenticated with token: {user.Token}");
                }
                else
                {
                    return Unauthorized();
                }
            }

            private string GenerateRandomToken()
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                return new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }


    }

