#region REFERÊNCIAS
#region PRTOJETO
using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Filters;
using curso.api.Models.Cursos;
#endregion
#region PACOTES
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
#endregion
#region FRAMEWORK
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
#endregion
#endregion

namespace curso.api.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class CursoController : ControllerBase
	{
		#region ATRIBUTOS
		private readonly ICursoRepository _repository;
		#endregion

		#region CONSTRUTOR
		public CursoController(ICursoRepository repository)
		{
			_repository = repository;
		}
		#endregion

		#region AÇÕES / ACTIONS
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
			var cursoEntitade = new Curso
			{
				Nome = curso.Nome,
				Descricao = curso.Descricao,
				CodigoUsuario = codigoUsuario
			};

			_repository.Adicionar(cursoEntitade);
			_repository.Commit();

			return Created(nameof(CursoController), cursoEntitade);
		}


		[HttpGet]
		[Route("cursos")]
		[ValidacaoModelStateCustomizado]
		[Authorize]
		[SwaggerResponse(statusCode: 201, description: "Sucesso ao obter um curso.")]
		[SwaggerResponse(statusCode: 401, description: "Não autorizado.")]
		public async Task<IActionResult> Get()
		{
			var codigoUsuario = ObterLogin();
			var cursos = _repository
							.ObterPorUsuario(codigoUsuario)
							.Select(s => new CursoViewModelOutput()
							{
								Nome = s.Nome,
								Descricao = s.Descricao,
								Login = s.Usuario.Login
							});

			return Ok(cursos);
		}
		#endregion

		#region MÉTODOS
		private int ObterLogin()
		{
			return int.Parse(User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier)?.Value);
		}
		#endregion
	}
}
