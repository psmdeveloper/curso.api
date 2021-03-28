using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace curso.api.Infraestrutura.Data.Repositories
{
	public class CursoRepository : ICursoRepository
	{
		#region ATRIBUTOS
		private readonly CursoContext _context;
		#endregion

		#region CONSTRUTOR
		public CursoRepository(CursoContext context)
		{
			_context = context;
		}
		#endregion

		#region MÉTODOS

		public void Adicionar(Curso entidade)
		{
			_context.Add(entidade);
		}

		public void Commit()
		{
			_context.SaveChanges();
		}

		public IList<Curso> ObterPorUsuario(int codigoUsuario)
		{
			return _context.Cursos
					.Include(p => p.Usuario) // Aplica o comportamento de um INNER JOIN (Conceito de conjuntos/relacionamento de dados SQL)
					.Where(c => c.CodigoUsuario == codigoUsuario)
					.ToList();
		}
		#endregion
	}
}
