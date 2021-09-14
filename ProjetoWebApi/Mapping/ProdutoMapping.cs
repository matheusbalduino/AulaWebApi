using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoWebApi.Mapping
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(p => p.ProdutoId);

            builder.Property(d => d.Descricao)
                .HasColumnType("varchar(400)")
                .IsRequired();

            builder.Property(n => n.Nome)
                .HasColumnType("varchar(80)")
                .IsRequired();

            builder.Property(p => p.Preco)
                .HasColumnType("decimal")
                .IsRequired();

            builder.Property(i => i.ImagemUrl)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.ToTable("Produtos");

        }
    }
}
