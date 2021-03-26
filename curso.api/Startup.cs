using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

namespace curso.api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			#region Swagger
			services.AddSwaggerGen(d =>
				{
					/* 
					 * Montando o nome do arquivo de origem com a informacao de documentacao; Que neste caso, 
					 * é o mesmo arquivo, gerado em tempo de build, após a configuracao das propriedades do projeto (gerar arquivo XML)
					*/
					var xmlArquivo = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

					/*
					 * Obtendo o caminho fisico do assembly deste projeto, para compor a localizacao completa do arquivo contendo as descricoes
					 * desejadas de documentacao do projeto/API.
					 */
					var xmlCaminho = Path.Combine(AppContext.BaseDirectory, xmlArquivo);

					/* 
					 * Informando"/configurando ao " servico do swagger a origem da documentacao a ser exposta/externalizada
					 * para consumidores da API.
					 */
					d.IncludeXmlComments(xmlCaminho);
				});
			#endregion

			#region Pipeline
			services.AddControllers()
				.ConfigureApiBehaviorOptions(options =>
				{
					options.SuppressModelStateInvalidFilter = true;
				});
			#endregion

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			// Adicionando o middleware do swagger ao Pipeline de requisicoes da API
			app.UseSwagger();

			/*
			 * Sobrescrevendo o comportamento de rotas, adicionando, que para se obtrer acesso
			 * ao Swagger, nenhum prefixo de rota sera necessário
			 */
			app.UseSwaggerUI(r =>
			{
				r.SwaggerEndpoint("/swagger/v1/swagger.json", "Api curso DIO");
				r.RoutePrefix = string.Empty; // swagger
			});
		}
	}
}
