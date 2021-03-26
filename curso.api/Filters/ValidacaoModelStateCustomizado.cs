using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using curso.api.Models;


namespace curso.api.Filters
{
	public class ValidacaoModelStateCustomizado : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				var validacaoViewModel = new ValidaCampoViewModelOutput(context.ModelState.SelectMany(sm => sm.Value.Errors).Select(s => s.ErrorMessage));
				context.Result = new BadRequestObjectResult(validacaoViewModel);
			}
		}
	}
}
