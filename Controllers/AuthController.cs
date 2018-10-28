using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers {
    //http://localhost:5000/api/elNombredelConrollerValuesControllere
    [Route ("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController (IAuthRepository repo, IConfiguration config) {
            _config = config;
            _repo = repo;

        }

        [HttpPost ("register")]
        public async Task<IActionResult> Register (UserForRegisterDto registerDto) {

            //validad la request

            //validar usaurio y lowercas
            registerDto.Username = registerDto.Username.ToLower ();
            //verificar si existe
            if (await _repo.UserExits (registerDto.Username))
                return BadRequest ("El usaurio ya existe");

            var UserToCreate = new User {
                Username = registerDto.Username
            };

            var createdUser = await _repo.Register (UserToCreate, registerDto.Password);

            // return CreatedAtRoute();
            return StatusCode (201);
        }
        //auth con token JWT
        [HttpPost ("login")]
        public async Task<IActionResult> Login (UserForLogin reg) {
            var userFromRepo = await _repo.Login (reg.Username.ToLower(), reg.Password);

            if (userFromRepo == null)
                return Unauthorized ();

            //crear el data con  el user ID y el username
            var reclamo = new [] {
                new Claim (ClaimTypes.NameIdentifier, userFromRepo.id.ToString ()),
                new Claim (ClaimTypes.Name, userFromRepo.Username)
            };
//obtener string del token desde appsetting.json
            var key = new SymmetricSecurityKey (Encoding.UTF8.
            GetBytes(_config.GetSection("AppSetting:Token").Value));

// encriptarlas credencias
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        
            //el descrptodr metadata del token
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(reclamo),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        
        }

    }
}