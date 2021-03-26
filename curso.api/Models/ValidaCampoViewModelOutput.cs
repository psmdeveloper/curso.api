using System.Collections.Generic;

namespace curso.api.Models
{
	public class ValidaCampoViewModelOutput
	{
		#region Propriedades
		public IEnumerable<string> Erros { get; }
		#endregion

		#region Construtor
		public ValidaCampoViewModelOutput(IEnumerable<string> erros)
		{
			Erros = erros;
		}
		#endregion
	}
}
