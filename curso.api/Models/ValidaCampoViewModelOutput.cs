using System.Collections.Generic;

namespace curso.api.Models
{
	public class ValidaCampoViewModelOutput
	{
		#region PROPRIEDADES
		public IEnumerable<string> Erros { get; }
		#endregion

		#region CONSTRUTOR
		public ValidaCampoViewModelOutput(IEnumerable<string> erros)
		{
			Erros = erros;
		}
		#endregion
	}
}
