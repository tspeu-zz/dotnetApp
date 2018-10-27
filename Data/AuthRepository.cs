using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data

{
    public class AuthRepository : IAuthRepository {

        private readonly DataContext _ctx;

        public AuthRepository (DataContext ctx) {
            
            _ctx = ctx;
        }


        /*recoge el user y el pass->para compararlo con el de la dB
        se debe generar el HAsh y compararlo con el de la db
        */
        public async Task<User> Login (string username, string password) {
            var usuario = await _ctx.Users.FirstOrDefaultAsync( x => x.Username == username);
//si se devuelve n ull el controle de tipo get 
//devuelve el http code 
            if (usuario == null){
                return null;
            }

            if(!VeriryPasswordHash(password, usuario.PasswordHash, usuario.PasswordSalt)){
                return null;
            }
            return usuario;
        }

        private bool VeriryPasswordHash(string pass, byte[] passwordHash, byte[] passwordSalt)
        {
            //se obtiene el hash<<slta ramdom
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                //se debe codificar a un array de byte UTF8
                
                //byte[] data = System.Text.Encoding.UTF8.GetBytes(pass);
                //se encripta el password
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));

                //hace falta verificar por cada byt 
                for(int i = 0 ; i < computedHash.Length; i++) {
                    if(computedHash[i] != passwordHash[i]) 
                        return false;        
                }
            }
            return true;
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

        public async Task<bool> UserExits (string username) {
            if (await _ctx.Users.AnyAsync( x=> x.Username == username )){
                return true;
            }

            return false;
        }
    }
}