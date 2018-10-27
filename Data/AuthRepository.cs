using System;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data

{
    public class AuthRepository : IAuthRepository {

        private readonly DataContext _ctx;

        public AuthRepository (DataContext ctx) {
            
            _ctx = ctx;
        }

        public Task<User> Login (User user, string password) {
            throw new System.NotImplementedException ();
        }

//se insertan en la DB usando el async->
        public async Task<User> Register (User user, string password) {
            
            //se almacenan el passEncrytpato y la clave
            //generada por el metodo CratePassHas
            byte[] passwordHash, passwordSalt;
            //para envias la referencia a las vaariables de arriba se usa
            //out ASI CUANDO SE actuaLIZA cone l metodo tambien hace
            //push al array bytv[]
            creaPassEncripClave(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
        //de la clase DataCOntex de EF-
            await _ctx.Users.AddAsync(user);
        //se hace commit en la db para guardar los cambios
            await _ctx.SaveChangesAsync();
            return user;
        }

        /* 
        SE encrpta el pass yse  genera la clave ramdon 
        y se guardan en el array Byte
        */
        private void creaPassEncripClave(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            //system sevurity criptografy usandos HASH SHA512-256
            //para generar un clave ramdon
            //al usar using--> se emplea el metodo DISPOSE()
            //que desacarta todo una vex implementado->lo desecha
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                //la key se genera aqui
                passwordSalt = hmac.Key;

                //se debe codificar a un array de byte UTF8
                
                byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
                //se encripta el password
                passwordHash = hmac.ComputeHash(data);
            }
        }

        public Task<bool> UserExits (string password) {
            throw new System.NotImplementedException ();
        }

    }
}