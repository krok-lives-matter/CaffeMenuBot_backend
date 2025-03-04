using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CaffeMenuBot.Data.Models.Dashboard;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using CaffeMenuBot.AppHost.Options;
using CaffeMenuBot.Data;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CaffeMenuBot.AppHost.Helpers
{
    public class JwtHelper
    {

        private readonly UserManager<DashboardUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CaffeMenuBotContext _context;
        private readonly JwtOptions _jwtConfig;
        private readonly IOptionsMonitor<JwtOptions> _optionsMonitor;

        public JwtHelper
        (
            CaffeMenuBotContext context,
            UserManager<DashboardUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptionsMonitor<JwtOptions> optionsMonitor
        )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _optionsMonitor = optionsMonitor;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        public string GenerateJwtToken(DashboardUser user)
        {
            // Now its time to define the jwt token which will be responsible of creating our tokens
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // We get our secret from the appsettings
            var key = Encoding.ASCII.GetBytes(_jwtConfig.SecretKey);

            // we define our token descriptor
            // We need to utilise claims which are properties in our token which gives information about the token
            // which belong to the specific user who it belongs to
            // so it could contain their id, name, email the good part is that these information
            // are generated by our server and identity framework which is valid and trusted
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim("Username", user.UserName),
                    new Claim("Email", user.Email),
                    // the JTI is used for our refresh token which we will be converting in the next video
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())             
                }),
                // the life span of the token needs to be shorter and utilise refresh token to keep the user signedin
                // but since this is a demo app we can extend it to fit our current need
                Expires = DateTime.Now.AddHours(3),
                // here we are adding the encryption algorithm information which will be used to decrypt our token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            // add roles
            _context.UserRoles.Include(r => r.Role).FirstOrDefault(r => r.UserId == user.Id);

            string? roles = this.ConvertRolesToJwtFormat(user.Roles);

            if(roles != null)
            {
                var claim = new Claim("Roles", roles);
                tokenDescriptor.Subject.AddClaim(claim);
            }
            
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }  

        public async Task AssignRolesAsync(DashboardUser user, List<IdentityRole> roles)
        {
            await this.CreateNeededRoles(roles);

            foreach(var role in roles)
                await _userManager.AddToRoleAsync(user, role.Name);
        }

        private async Task CreateNeededRoles(List<IdentityRole> roles)
        {
            foreach(var role in roles)
            {
                if (!_context.Roles.Any(r => r.Name == role.Name))
                    await _roleManager.CreateAsync(role);
            }            
        }

        public List<IdentityRole> ConvertJwtRolesToIdentity(string roles)
        {
            string[] splited = roles.Trim().Split(',');
            List<IdentityRole> output = new List<IdentityRole>();
            foreach(string role in splited)
                output.Add(new IdentityRole(role));
            return output;
        }

        public string? ConvertRolesToJwtFormat(ICollection<DashboardUserRole> userRoles)
        {
            if(userRoles.Count == 0)
                return null;
            
            string roles = "";

                foreach (var role in userRoles)
                    roles += role.Role.Name + ",";

                // remove comma in the end
                roles = roles.Remove(roles.Length - 1, 1);
            
            return roles;
        }
    }
}