
using curso.api.Business.Entities;
using curso.api.Infraestrutura.Data.Maps;
using Microsoft.EntityFrameworkCore;

namespace curso.api.Infraestrutura.Data
{
	public class CursoContext : DbContext
	{
		#region Propriedades
		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Curso> Cursos { get; set; }
		#endregion

		#region Construtor
		public CursoContext(DbContextOptions<CursoContext> options)
			: base(options)
		{
		}
		#endregion

		#region Metodos
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.ApplyConfiguration(new UsuarioMapping())
				.ApplyConfiguration(new CursoMapping());

			base.OnModelCreating(modelBuilder);
		}
		#endregion
	}
}
