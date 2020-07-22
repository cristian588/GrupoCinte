using AutoMapper;
using GrupoCinte.Api.Data;
using GrupoCinte.Common.Dtos;
using GrupoCinte.Common.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GrupoCinte.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository Repository;
        private readonly IConfiguration Configuration;
        private readonly IMapper Mapper;
        public AuthController(IAuthRepository authRepository, IConfiguration configuration, IMapper mapper)
        {
            Repository = authRepository;
            Configuration = configuration;
            Mapper = mapper;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            try
            {
                var userFromRepo = await Repository.Login(userForLoginDto.IdNumber, userForLoginDto.Password);
                if (userFromRepo == null) return StatusCode(401, "User or password incorrect");
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Name)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var user = Mapper.Map<UserForListDto>(userFromRepo);
                return StatusCode(200, new UserForLoginResultDto()
                {
                    Token = tokenHandler.WriteToken(token),
                    Name = user.Name,
                    LastName = user.LastName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Helpers.Helpers.GetErrorMessage("Login error", ex));
            }
        }
    }
}