#region Referências

#region PROJETO
using curso.api.Business;
using curso.api.Business.Repositories;
using curso.api.Infraestrutura.Data;
using curso.api.Infraestrutura.Data.Repositories;
#endregion

#region PACOTES
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
#endregion

#region FRAMEWORK
using System;
using System.IO;
using System.Reflection;
using System.Text;
#endregion

#endregion

namespace curso.api
{
	public class Startup
	{
		#region PROPRIEDADES
		public IConfiguration _configuration { get; }
		#endregion

		#region CONSTRUTOR
		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		#endregion

		#region MÉTODOS
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			#region AUTORIZAÇÃO
			var secret = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfigurations:Secret").Value);

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

			#region PIPELINE
			services.AddControllers()
				.ConfigureApiBehaviorOptions(options =>
				{
					options.SuppressModelStateInvalidFilter = true;
				});
			#endregion

			#region INJEÇÃO DE DEPEND&ENCIA / DEPENDENCY INJECTION
			services.AddScoped<IUsuarioRepository, UsuarioRepository>();
			services.AddScoped<ICursoRepository, CursoRepository>();
			services.AddScoped<IAuthenticationService, JwtService>();
			#endregion

			#region PERSISTÊNCIA DADOS
			#region ENTITY FRAMEWORK CORE
			services.AddDbContext<CursoContext>(options =>
			{
				options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
			});
			#endregion
			#endregion

			#region DOCUMENTAÇÃO API : SWAGGER
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

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			#region PIPELINE DE REQUISIÇÃO / REQUEST

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

			#endregion

			#region SWAGGER

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
		#endregion
	}
}
