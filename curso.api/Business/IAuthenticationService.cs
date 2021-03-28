using curso.api.Models.Usuarios;

namespace curso.api.Business
{
	public interface IAuthenticationService
	{
		string GerarToken(UsuarioViewModelOutput usuario);
	}
}
