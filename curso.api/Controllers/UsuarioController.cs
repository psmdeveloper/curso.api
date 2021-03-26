using Microsoft.AspNetCore.Mvc;
using curso.api.Models.Usuarios;

namespace curso.api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuarioController : ControllerBase
	{
		[HttpPost]
		public IActionResult Logar(LoginViewModelInput loginViewModelInput)
		{
			return Ok(loginViewModelInput);
		}

		[HttpPost]
		public IActionResult Registrar(RegistroViewModelInput registroViewModelInput)
		{
			return Created(nameof(UsuarioController), registroViewModelInput);
		}
	}
}
