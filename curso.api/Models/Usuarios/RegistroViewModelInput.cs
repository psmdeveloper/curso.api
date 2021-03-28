using System.ComponentModel.DataAnnotations;

namespace curso.api.Models.Usuarios
{
	public class RegistroViewModelInput
	{
		#region PROPRIEDADES
		[Required(ErrorMessage = "O login é obrigatorio.")]
		public string Login { get; set; }

		[Required(ErrorMessage = "O E-mail é obrigatório.")]
		public string Email { get; set; }
		[Required(ErrorMessage = "A senha é obrigatória.")]
		public string Senha { get; set; }
		#endregion
	}
}

