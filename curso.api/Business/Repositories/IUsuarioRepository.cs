using curso.api.Business.Entities;
using curso.api.Models.Usuarios;

namespace curso.api.Business.Repositories
{
	public interface IUsuarioRepository : IRepository<Usuario>
	{
		Usuario ObterUsuario(LoginViewModelInput login);
	}
}
