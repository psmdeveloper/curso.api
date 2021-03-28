using System.ComponentModel.DataAnnotations;

namespace curso.api.Models.Usuarios
{
	public class UsuarioViewModelOutput
	{
		#region PROPRIEDADES
		public int Codigo { get; set; }

		[Required(ErrorMessage = "O login é obrigatorio.")]
		public string Login { get; set; }

		[Required(ErrorMessage = "O E-mail é obrigatório.")]
		public string Email { get; set; }

		#endregion
	}
}

