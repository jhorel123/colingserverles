using Coling.Repositorio.Contratos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Repositorio.Implementacion
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly IConfiguration configuration;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<TokenData> ConstruirToken(string usuarioname, string password, string rol)
        {
            var claims = new List<Claim>()
            {
                new Claim("usuario", usuarioname),
                new Claim("rol", rol),
                new Claim("estado", "Activo")
            };

            var SecretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["LlaveSecreta"] ?? ""));
            var creds = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(15);

            var tokenSeguridad = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expires, signingCredentials: creds);

            TokenData respuestaToken = new TokenData();
            respuestaToken.Token = new JwtSecurityTokenHandler().WriteToken(tokenSeguridad);
            respuestaToken.Expira = expires;

            return respuestaToken;
        }

        public async Task<string> EncriptarPassword(string password)
        {
            string Encriptado = "";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                Encriptado = Convert.ToBase64String(bytes);
            }
            return Encriptado;
        }

        public Task<bool> ValidarToken(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<TokenData> VerficarCredenciales(string usuariox, string passwordx)
        {
            TokenData tokenDevolver = new TokenData();
            string passEncriptado = await EncriptarPassword(passwordx);
            string consulta = "select * from usuario where nombreuser='" + usuariox + "' and password='" + passEncriptado + "'";
            DataTable Existe = conexion.EjecutarDataTabla(consulta, "tabla");
            if (Existe.Rows.Count > 0)
            {
                string rol = Existe.Rows[0]["rol"].ToString();
                tokenDevolver = await ConstruirToken(usuariox, passwordx, rol);
            }
            return tokenDevolver;
        }


    }
}