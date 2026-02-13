namespace VHBurguer.DTOs.ProdutoDto
{
    public class CriarProdutoDto
    {
        public string Nome { get; set; } = null!;

        public decimal Preco { get; set; }

        public string Descricao { get; set; } = null!;
        
        // A imagem vem via multipart/form-data, ideal para upload de arquivos
        public IFormFile Imagem { get; set; } = null!;

        public List<int> CategoriasIds { get; set; } = new();
    }
}
