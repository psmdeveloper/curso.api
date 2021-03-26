using curso.api.Filters;
using curso.api.Models.Cursos;

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace curso.api.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class CursoController : ControllerBase
	{

		#region Ações		
		/// <summary>
		/// Este recurso permite cadastrar curso para o usuário autenticado..
		/// </summary>
		/// <param name="curso"></param>
		/// <returns>Retorna status 201 e dados do cuyrso do usuário.</returns>
		[HttpPost]
		[Route("cadastrar")]
		[ValidacaoModelStateCustomizado]
		[Authorize]
		[SwaggerResponse(statusCode: 201, description: "Sucesso ao cadastrar um curso.")]
		[SwaggerResponse(statusCode: 401, description: "Não autorizado.")]
		public async Task<IActionResult> Post(CursoViewModelInput curso)
		{
			var codigoUsuario = ObterLogin();
			return Created("[controller]", curso);
		}

		
		[HttpGet]
		[Route("cursos")]
		[ValidacaoModelStateCustomizado]
		[Authorize]
		[SwaggerResponse(statusCode: 201, description: "Sucesso ao cadastrar um curso.")]
		[SwaggerResponse(statusCode: 401, description: "Não autorizado.")]
		public async Task<IActionResult> Get()
		{
			var codigoUsuario = ObterLogin();
			var cursos = new List<CursoViewModelOutput>
			{
				new CursoViewModelOutput{ Nome="Teste", Descricao="Descricao teste", Login = codigoUsuario.ToString()},
				new CursoViewModelOutput{ Nome="Teste II", Descricao="Descricao teste II", Login = (codigoUsuario+1).ToString()}
			};

			return Ok(cursos);
		}
		#endregion

		private int ObterLogin()
		{
			return int.Parse(User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier)?.Value);
		}
	}
}
