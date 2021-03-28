using System.ComponentModel.DataAnnotations;

namespace curso.api.Models.Cursos
{
	public class CursoViewModelOutput
	{
		#region PROPRIEDADES
		public string Nome{ get; set; }
		
		public string Descricao { get; set; }
		public string Login { get; set; }

		#endregion
	}
}
