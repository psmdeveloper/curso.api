using Microsoft.AspNetCore.Mvc;
using curso.api.Models.Usuarios;

namespace curso.api.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class UsuarioController : ControllerBase
	{
		[HttpPost]
		[Route("Login")]
		public IActionResult Logar(LoginViewModelInput loginViewModelInput)
		{
			return Ok(loginViewModelInput);
		}

		[HttpPost]
		[Route("Registrar")]
		public IActionResult Registrar(RegistroViewModelInput registroViewModelInput)
		{
			return Created(nameof(UsuarioController), registroViewModelInput);
		}
	}
}
