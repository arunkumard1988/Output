using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ldapi.Model;
using levenshtein.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ldapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        public LoginController (IConfiguration config)
        {
            _config = config;
        }

        public async Task<IActionResult> Login(string username, string password)
        {
            UserModel user = new UserModel();
            user.UserName = username;
            user.Password = password;
            IActionResult response = Unauthorized();

            var userdata = AuthenticateUser(user);
            if (userdata != null)
            {
                var tokenstring =await GenerateJSONWebtoken(userdata);
                response = Ok(new { token = tokenstring });
            }
            return response;
        }

        private UserModel AuthenticateUser(UserModel user)
        {
            UserModel usermodel = null;
            if(user.UserName=="arun" && user.Password == "arun")
            {
                usermodel = new UserModel();
                usermodel.UserName = "arun";
                usermodel.Password = "arun";
                usermodel.Email = "arunkumard1988@gmail.com";
            }
            return usermodel;
        }
        private async Task<string> GenerateJSONWebtoken(UserModel userdata)
        {
            var secureKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credential = new SigningCredentials(secureKey,SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userdata.UserName),
                new Claim(JwtRegisteredClaimNames.Email,userdata.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer:_config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires:DateTime.Now.AddMinutes(20),
                 signingCredentials:credential);

            var encodedtoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedtoken;
        } 


        [Authorize]
        [HttpPost("Post")]
        public string post()
        {
            var identity = HttpContext.User.Identity as  ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var username = claim[0].Value;
            return username;
        }
 

        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>          
        [Authorize]
        [HttpPost("GetValue")]
        public int LevenshteinDistance(LevenshteinModel lmode)
        {
            string s = lmode.FirstParam;
            string t = lmode.SecondParam;
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
        return d[n, m];           
            
        }
    }
}