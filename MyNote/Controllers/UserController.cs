using AutoMapper;
using Contracts;
using Entities.DTO.User;
using Entities.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNote.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IManagerRepo _managerRepo;
        private readonly IOptions<AppSettings> _options;
        private readonly IMapper _mapper;
        private readonly JwtSecurityTokenHandler SecurityTokenHandler;

        public UserController(IManagerRepo managerRepo, IOptions<AppSettings> options, IMapper mapper )
        {
            this._managerRepo = managerRepo;
            this._options = options;
            this._mapper = mapper;
            SecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        [HttpPost("loginAsync")]
        public async Task<ActionResult<UserInfoDTO>> LoginAsync([FromBody] UserCredentials userCredentials, CancellationToken cancellationToken)
        {
            UserInfoDTO userInfoDTO = new UserInfoDTO();
            if(userCredentials is null)
            {
                return BadRequest("No Data");
            }
            
            var user = await _managerRepo.User.LoginAsync(userCredentials.Login, userCredentials.Password, false, cancellationToken);
            if( user != null)
            {
                var key = Encoding.ASCII.GetBytes(_options.Value.SecretKey);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.GivenName, user.FirstName),
                            new Claim(ClaimTypes.Name, user.LastName),
                            new Claim(ClaimTypes.Role, user.Role.ToString())
                        }
                   ),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var securityToken = SecurityTokenHandler.CreateToken(tokenDescriptor);
                userInfoDTO.Token = SecurityTokenHandler.WriteToken(securityToken);
                userInfoDTO.UserDetails = _mapper.Map<UserDTO>(user);
            }

            if (string.IsNullOrEmpty(userInfoDTO?.Token))
            {
                return Unauthorized("Error login or password");
            }
            return Ok(userInfoDTO);
        }

    }
}
