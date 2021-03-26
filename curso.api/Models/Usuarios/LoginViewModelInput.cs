using System.ComponentModel.DataAnnotations;

namespace curso.api.Models.Usuarios
{
	public class LoginViewModelInput
	{
		#region Propriedades
		[Required(ErrorMessage ="O Login é obrigatório.")]
		public string Login { get; set; }
		[Required(ErrorMessage ="A Senha é obrigatória.")]
		public string Senha { get; set; }
		#endregion

	}
}
