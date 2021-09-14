using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoWebApi.Model;
using ProjetoWebApi.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProjetoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IMainService _mainService;
        public ProdutoController(IProdutoService produtoService, IMainService mainService)
        {
            _produtoService = produtoService;
            _mainService = mainService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProdutos()
        {
            try
            {
                var produtos = await _produtoService.GetAllProducts();

                if(produtos == null)
                    return BadRequest("Não tem nada");

                if (produtos.Count == 0)
                    return BadRequest("Tabela Vazia");

                return Ok(produtos);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Produto produto)
        {
            try
            {
                _mainService.Add<Produto>(produto);
                if(await _mainService.saveChangesAsync())
                {
                    return Ok(await _produtoService.GetById(produto.ProdutoId));
                }
                return BadRequest("Não foi possível Inserir o Produto");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadDeImagem()
        {

            // Upload de imagem para o servidor
            try
            {
                // Cria uma pasta caso não exita
                if (!Directory.Exists("Images"))
                {
                    // comando para criação de pasta
                    Directory.CreateDirectory("Images");
                }

                //captura em uma variável o file enviado por request.
                var file = Request.Form.Files[0];

                // Criação do caminho para salvar a imagem no servidor. 
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Images");

                // verifica se o arquivo chegou com sucesso. 
                if (file.Length > 0)
                {
                    // Coloca na variável o nome do file.
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;

                    // Cria o caminho completo com o nome do file. Remove caracteres indesejados.
                    var fullPath = Path.Combine(pathToSave, filename.Replace("\"", " ").Trim());


                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    // retorna em caso de sucesso
                    return Ok("Imagem salva com sucesso");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return BadRequest("Erro ao fazer upload da imagem");

        }
        [HttpPut("update")]
        public async Task<IActionResult> AtualizarProduto(Produto model)
        {
            try
            {
                // Salva na variável o produto
                var produto = await _produtoService.GetById(model.ProdutoId);

                if (produto == null) return BadRequest();

                // verifica se há mudança de imagem no produto
                if (String.IsNullOrEmpty(model.ImagemUrl))
                    model.ImagemUrl = produto.ImagemUrl;

                // atribui id do produto ao model do request
                model.ProdutoId = produto.ProdutoId;

                // cria o update do produto.
                _mainService.Update<Produto>(model);

                // salva o produto no banco de dados.
                if (await _mainService.saveChangesAsync())
                {
                    //retorna o produto criado com sucesso
                    return Ok(await _produtoService.GetById(model.ProdutoId));
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> RemoverProduto(int id)
        {
            try
            {
                var produto = await _produtoService.GetById(id);

                if (produto == null) BadRequest("Não há nada");

                _mainService.Delete<Produto>(produto);

                if(await _mainService.saveChangesAsync())
                {
                    return Ok($"Produto {produto.Nome} Removido");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}


