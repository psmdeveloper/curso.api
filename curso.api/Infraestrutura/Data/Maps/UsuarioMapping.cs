using curso.api.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace curso.api.Infraestrutura.Data.Maps
{
	public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
	{
		public void Configure(EntityTypeBuilder<Usuario> builder)
		{
			builder
				.ToTable("TB_USUARIO")
				.HasKey(pk => pk.Codigo);

			builder.Property(p => p.Codigo).ValueGeneratedOnAdd();
			builder.Property(p => p.Login);
			builder.Property(p => p.Senha);
			builder.Property(p => p.Email);


		}
	}
}
