using System.ComponentModel.DataAnnotations;

namespace curso.api.Models.Cursos
{
	public class CursoViewModelInput
	{
		#region Propriedades
		[Required(ErrorMessage = "Nome é obrigatório.")]
		public string Nome{ get; set; }
		
		[Required(ErrorMessage = "Descrição é obrigatório.")]
		public string Descricao { get; set; }
		
		#endregion
	}
}
