using curso.api.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace curso.api.Infraestrutura.Data.Maps
{
	public class CursoMapping : IEntityTypeConfiguration<Curso>
	{
		public void Configure(EntityTypeBuilder<Curso> builder)
		{
			builder
				.ToTable("TB_CURSO")
				.HasKey(pk => pk.Codigo);

			builder.Property(p => p.Codigo).ValueGeneratedOnAdd();
			builder.Property(p => p.Nome);
			builder.Property(p => p.Descricao);
			builder.Property(p => p.CodigoUsuario);

			// Definição da Foreign Key ( Chave estrangeira )
			builder
				.HasOne(p => p.Usuario)
				.WithMany().HasForeignKey(fk => fk.CodigoUsuario);


		}
	}
}
