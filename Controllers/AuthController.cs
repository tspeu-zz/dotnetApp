using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers {
    //http://localhost:5000/api/elNombredelConrollerValuesControllere
    [Route ("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IAuthRepository _repo;
        public AuthController (IAuthRepository repo) {
            _repo = repo;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto registerDto){

            //validad la request

            //validar usaurio y lowercas
            registerDto.Username = registerDto.Username.ToLower();
            //verificar si existe
            if(await _repo.UserExits(registerDto.Username))
                return BadRequest("El usaurio ya existe");

            var UserToCreate = new User
            {
                Username = registerDto.Username
            };

            var createdUser = await _repo.Register(UserToCreate,registerDto.Password );

            // return CreatedAtRoute();
            return StatusCode(201);
        }

    }
}