using curso.api.Business.Entities;
using System.Collections.Generic;

namespace curso.api.Business.Repositories
{
	public interface ICursoRepository : IRepository<Curso>
	{
		IList<Curso> ObterPorUsuario(int codigoUsuario);
	}
}
