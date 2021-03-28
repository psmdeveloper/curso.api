#region REFERÊNCIAS
#region PROJETO
using curso.api.Models.Usuarios;
#endregion
#region PACOTES
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
#endregion
#region FRAMEWORK
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
#endregion
#endregion

namespace curso.api.Business
{
	public class JwtService : IAuthenticationService
	{
		#region ATRIBUTOS
		private readonly IConfiguration _configuration;
		#endregion

		#region CONSTRUTOR
		public JwtService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		#endregion

		#region MÉTODOS
		public string GerarToken(UsuarioViewModelOutput usuario)
		{
			var secret = Encoding.ASCII.GetBytes(_configuration.GetSection("Secret").Value);
			var symmetricSecurityKey = new SymmetricSecurityKey(secret);
			var securityTokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
				new Claim(type: ClaimTypes.NameIdentifier, value: usuario.Codigo.ToString()),
				new Claim(type: ClaimTypes.Name, value: usuario.Login.ToString()),
				new Claim(type: ClaimTypes.Email, value: usuario.Email.ToString()),
				}),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
			};
			var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
			var tokenGenerated = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
			var token = jwtSecurityTokenHandler.WriteToken(tokenGenerated);

			return token;
		}
		#endregion
	}
}
