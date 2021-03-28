#region REFERÊNCIAS
#region PROJETO
using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Models.Usuarios;
#endregion
#region FRAMEWORK
using System.Linq;
#endregion
#endregion

namespace curso.api.Infraestrutura.Data.Repositories
{
	public class UsuarioRepository : IUsuarioRepository
	{

		#region ATRIBUTOS
		private readonly CursoContext _context;
		#endregion

		#region CONSTRUTOR
		public UsuarioRepository(CursoContext context)
		{
			_context = context;
		}

		#endregion

		#region MÉTODOS
		public void Adicionar(Usuario usuario)
		{
			_context.Add(usuario);
		}

		public void Commit()
		{
			_context.SaveChanges();
		}

		public Usuario ObterUsuario(LoginViewModelInput login)
		{
			return _context.Usuarios.FirstOrDefault(u => u.Login == login.Login);
		}

		#endregion
	}
}
