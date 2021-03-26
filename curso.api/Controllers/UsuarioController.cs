using Microsoft.AspNetCore.Mvc;
using System.Text;

using curso.api.Models.Usuarios;
using curso.api.Models;
using curso.api.Filters;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace curso.api.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class UsuarioController : ControllerBase
	{
		/// <summary>
		/// Proporciona a acao de logar.
		/// </summary>
		/// <param name="Login">
		/// O formato do objeto esperado é: {Login, Senha}
		/// </param>
		/// <returns>
		/// O formato do objeto esperado é: {Login, Senha}
		/// </returns>
		[HttpPost]
		[Route("Login")]
		/*
		 * Adicionado na minha action, metadado para que o pipeline de requisicacao do MVC da minha API
		 * saiba em qual action o filter deve ser chamado.
		 */
		[ValidacaoModelStateCustomizado]
		// Personalizacoes do swagger para classes de result dos verbos HTTP
		[SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", type: typeof(LoginViewModelInput))]
		[SwaggerResponse(statusCode: 400, description: "Campos Obrigatórios.", type: typeof(ValidaCampoViewModelOutput))]
		[SwaggerResponse(statusCode: 500, description: "Erro interno.", type: typeof(ErroGenericoViewModel))]
		public IActionResult Logar(LoginViewModelInput loginViewModelInput)
		{

			#region Autorizacao
			var usuarioViewModelOutput = new UsuarioViewModelOutput()
			{
				Codigo = 1,
				Login = "pmoreira",
				Email = "pmoreira@server.kr"
			};
			var secret = Encoding.ASCII.GetBytes("CursoDio2021ModuloConfiguracaoBackendDotNetCore");
			var symmetricSecurityKey = new SymmetricSecurityKey(secret);
			var securityTokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
				new Claim(type: ClaimTypes.NameIdentifier, value: usuarioViewModelOutput.Codigo.ToString()),
				new Claim(type: ClaimTypes.Name, value: usuarioViewModelOutput.Login.ToString()),
				new Claim(type: ClaimTypes.Email, value: usuarioViewModelOutput.Email.ToString()),
				}),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
			};
			var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
			var tokenGenerated = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
			var token = jwtSecurityTokenHandler.WriteToken(tokenGenerated);

			#endregion

			return Ok(new
			{
				Token = token,
				Usuario = usuarioViewModelOutput,
			});
		}

		/// <summary>
		/// Proporciona a acao de registrar um usuario.
		/// </summary>
		/// <param name="Registrar">
		/// O formato do objeto esperado é: {Login, E-mail, Senha}
		/// </param>
		/// <returns>
		/// O formato do objeto esperado é: {Login, E-mail, Senha}
		/// </returns>
		[HttpPost]
		[Route("Registrar")]
		[ValidacaoModelStateCustomizado]
		[SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", type: typeof(LoginViewModelInput))]
		[SwaggerResponse(statusCode: 400, description: "Campos Obrigatórios.", type: typeof(ValidaCampoViewModelOutput))]
		[SwaggerResponse(statusCode: 500, description: "Erro interno.", type: typeof(ErroGenericoViewModel))]
		public IActionResult Registrar(UsuarioViewModelOutput registroViewModelInput)
		{
			return Created(nameof(UsuarioController), registroViewModelInput);
		}
	}
}
