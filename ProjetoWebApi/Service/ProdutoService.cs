using Microsoft.EntityFrameworkCore;
using ProjetoWebApi.Data;
using ProjetoWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoWebApi.Service
{
    public interface IProdutoService
    {
        public Task<List<Produto>> GetAllProducts();
        public Task<Produto> GetById(int IdProduto);
    }

    public class ProdutoService : IProdutoService
    {
        private readonly NossoDbContext _nossoDbContext;

        public ProdutoService(NossoDbContext nossoDbContext)
        {
            _nossoDbContext = nossoDbContext;
        }
        public async Task<List<Produto>> GetAllProducts()
        {
            try
            {
                return await _nossoDbContext.Produtos.ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Produto> GetById(int IdProduto)
        {
            try
            {
                return await _nossoDbContext.Produtos
                                            .Where(p => p.ProdutoId == IdProduto)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
