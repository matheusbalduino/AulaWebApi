using Microsoft.EntityFrameworkCore;
using ProjetoWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoWebApi.Data
{
    public class NossoDbContext: DbContext
    {
        public NossoDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NossoDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
