using Microsoft.AspNetCore.Mvc;
using curso.api.Models.Usuarios;
using curso.api.Models;
using Swashbuckle.AspNetCore.Annotations;

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
		[SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", type: typeof(LoginViewModelInput))]
		[SwaggerResponse(statusCode: 400, description: "Campos Obrigatórios.", type: typeof(ValidaCampoViewModelOutput))]
		[SwaggerResponse(statusCode: 500, description: "Erro interno.", type: typeof(ErroGenericoViewModel))]
		public IActionResult Logar(LoginViewModelInput loginViewModelInput)
		{
			return Ok(loginViewModelInput);
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
		public IActionResult Registrar(RegistroViewModelInput registroViewModelInput)
		{
			return Created(nameof(UsuarioController), registroViewModelInput);
		}
	}
}
