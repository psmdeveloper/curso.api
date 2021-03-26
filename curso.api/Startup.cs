using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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

					d.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
					{
						Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 123abdcef')",
						Name = "Authorization",
						In = ParameterLocation.Header,
						Type = SecuritySchemeType.ApiKey,
						Scheme = "Bearer"
					});

					d.AddSecurityRequirement(new OpenApiSecurityRequirement
					{
						{
							new OpenApiSecurityScheme
							{
								Reference=new OpenApiReference
								{
									Type= ReferenceType.SecurityScheme,
									Id="Bearer"
								}
							},
							Array.Empty<string>()
						}
					});
				});
			#endregion

			#region Pipeline
			services.AddControllers()
				.ConfigureApiBehaviorOptions(options =>
				{
					options.SuppressModelStateInvalidFilter = true;
				});
			#endregion

			#region Autorizacao
			var secret = Encoding.ASCII.GetBytes(Configuration.GetSection("JwtConfigurations:Secret").Value);

			services.AddAuthentication(a =>
			{
				a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(secret),
					ValidateIssuer = false,
					ValidateAudience = false
				};
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

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});


			#region Swagger

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
			#endregion

		}
	}
}
