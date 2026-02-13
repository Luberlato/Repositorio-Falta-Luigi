using Microsoft.EntityFrameworkCore;
using VHBurguer.Contexts;
using VHBurguer.Domains;
using VHBurguer.Interfaces;

namespace VHBurguer.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly VH_BurguerContext _context;

        public ProdutoRepository(VH_BurguerContext context)
        {
            _context = context;
        }

        public List<Produto> Listar()
        {
            List<Produto> produtos = _context.Produto
                .Include(produtoDb => produtoDb.Categoria) // Busca produtos e suas devidas categorias
                .Include(produtoDb => produtoDb.Usuario) // Busca os produtos e seus usuarios
                .ToList();
            return produtos;
        }

        public Produto ObterPorId(int id)
        {
            Produto? produto = _context.Produto
                .Include(produtoDb => produtoDb.Categoria)
                .Include(produtoDb => produtoDb.Usuario)
                .FirstOrDefault(produtoDb => produtoDb.ProdutoID == id);

            return produto;
        }

        public bool NomeExiste(string nome, int? produtoIdAtual = null)
        {
            // AsQueryable() monta a consulta para executar passo a passo
            // monta a consulta na tabela produto e não executa até finaliza-la
            var produtoConsultado = _context.Produto.AsQueryable();

            if (produtoIdAtual.HasValue)
            {
                produtoConsultado = produtoConsultado.Where(produtoDb => produtoDb.ProdutoID != produtoIdAtual.Value);
            }
            return produtoConsultado.Any(produtoDb => produtoDb.Nome == nome);
        }

        public byte[] ObterImagem(int id)
        {
            var produto = _context.Produto
                .Where(produtoDb => produtoDb.ProdutoID == id)
                .Select(produtoDb => produtoDb.Imagem)
                .FirstOrDefault();
            return produto;
        }

        public void Adicionar(Produto produto, List<int> categoriasIds)
        {
            List<Categoria> categorias = _context.Categoria
                // Contains verifica se o valor existe e retornar true ou false
                .Where(categoriaDb => categoriasIds.Contains(categoriaDb.CategoriaID))
                .ToList();
            
            // Adiciona as categorias incluidas ao produto
            produto.Categoria = categorias;

            _context.Produto.Add(produto);
            _context.SaveChanges();
        }

        public void Atualizar(Produto produto, List<int> categoriasIds)
        {
            Produto? produtoBanco = _context.Produto
                .Include(produtoDb => produtoDb.Categoria)
                .FirstOrDefault(produtoDb => produtoDb.ProdutoID == produto.ProdutoID);

            if (produtoBanco == null)
            {
                return;
            }

            produtoBanco.Nome = produto.Nome;
            produtoBanco.Preco = produto.Preco;
            produtoBanco.Descricao = produto.Descricao;

            if (produto.Imagem != null && produto.Imagem.Length > 0) 
            {
                produtoBanco.Imagem = produto.Imagem;
            }

            if (produto.StatusProduto.HasValue)
            {
                produtoBanco.StatusProduto = produto.StatusProduto;
            }

            //* Muda todas as categorias no banco com o id igual das cateforias que vieram da requisicao(front)
            var categorias = _context.Categoria
                .Where(categoriaDb => categoriasIds.Contains(categoriaDb.CategoriaID)).ToList();

            // Clear() remove as ligações atuais entre o produto e as categorias
            // Não apaga, só remove o vinculo com a tabela produto categorias
            produtoBanco.Categoria.Clear();

            foreach(var categoria in categorias)
            {
                produtoBanco.Categoria.Add(categoria);
            }

            _context.SaveChanges();
        }

        public void Remover(int id)
        {
            Produto? produto = _context.Produto.FirstOrDefault(produtoDb => produtoDb.ProdutoID==id);

            if(produto == null)
            {
                return;
            }

            _context.Produto.Remove(produto);
            _context.SaveChanges();
        }
    }
}
