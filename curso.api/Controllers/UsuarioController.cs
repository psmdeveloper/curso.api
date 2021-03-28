#region REFERÊNCIAS
#region PROJETO
using curso.api.Business;
using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Filters;
using curso.api.Models;
using curso.api.Models.Usuarios;
#endregion
#region PACOTES
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
#endregion
#region FRAMEWORK
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
#endregion
#endregion

namespace curso.api.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class UsuarioController : ControllerBase
	{

		#region ATRIBUTOS
		private readonly IUsuarioRepository _repository;
		private readonly IAuthenticationService _authentication;
		#endregion

		#region CONSTRUTOR
		public UsuarioController(IUsuarioRepository repository, IAuthenticationService authentication)
		{
			_repository = repository;
			_authentication = authentication;
		}
		#endregion

		#region AÇÕES / ACTIONS
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
		//[Authorize]
		[SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", type: typeof(LoginViewModelInput))]
		[SwaggerResponse(statusCode: 400, description: "Campos Obrigatórios.", type: typeof(ValidaCampoViewModelOutput))]
		[SwaggerResponse(statusCode: 500, description: "Erro interno.", type: typeof(ErroGenericoViewModel))]
		public IActionResult Logar(LoginViewModelInput loginViewModelInput)
		{
			var usuario = _repository.ObterUsuario(loginViewModelInput);
			if (usuario == null)
			{
				return BadRequest("Login não localizado.");
			}

			var usuarioOutput = new UsuarioViewModelOutput
			{
				Codigo = usuario.Codigo,
				Email = usuario.Email,
				Login = usuario.Login
			};

			var token = _authentication.GerarToken(usuarioOutput);

			return Ok(new { Token = token, Usuario = usuarioOutput });

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
		[Authorize]
		[SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", type: typeof(LoginViewModelInput))]
		[SwaggerResponse(statusCode: 400, description: "Campos Obrigatórios.", type: typeof(ValidaCampoViewModelOutput))]
		[SwaggerResponse(statusCode: 500, description: "Erro interno.", type: typeof(ErroGenericoViewModel))]
		public IActionResult Registrar(RegistroViewModelInput registroViewModelInput)
		{
			var usuario = new Usuario
			{
				Email = registroViewModelInput.Email,
				Login = registroViewModelInput.Login,
				Senha = registroViewModelInput.Senha
			};

			_repository.Adicionar(usuario);
			_repository.Commit();

			return Created(nameof(UsuarioController), usuario);
		}
		#endregion
	}
}
