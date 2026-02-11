using System.Security.Cryptography;
using System.Text;
using VHBurguer.Domains;
using VHBurguer.DTOs;
using VHBurguer.Exceptions;
using VHBurguer.Interfaces;


namespace VHBurguer.Aplication.Services
{
    // service concentra o "como fazer"
    public class UsuarioService
    {
        // _repository é o canal para acessar os dados.
        // São inumeras camadas para chegar no banco por conta de segurança
        private readonly IUsuarioRepository _repository;

        // Injecao de dependencias
        // Implementamos o repositório e o service só depende da interface
        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        //* Private pq o metodo não é regra de negocio e ele só vai ser usado nessa service
        private static LerUsuarioDto LerDto(Usuario usuario)
        {
            LerUsuarioDto lerUsuario = new LerUsuarioDto
            {
                UsuarioID = usuario.UsuarioID,
                Nome = usuario.Nome,
                Email = usuario.Email,
                StatusUsuario = usuario.StatusUsuario ?? true
            };
            return lerUsuario;

        }

        public List<LerUsuarioDto> Listar()
        {
            List<Usuario> usuarios = _repository.Listar();
            // Select 
            List<LerUsuarioDto> usuariosDto = usuarios
                .Select(usuariosBanco => LerDto(usuariosBanco)) // percorre cada usuario e LerDto
                .ToList(); // ToList devolve um lista de DTOs
            return usuariosDto;
        }

        private static void ValidarEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new DomainException("Email invalido");
            }
        }

        private static byte[] HashSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
            {
                throw new DomainException("Senha é obrigatoria");
            }

            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        }

        public LerUsuarioDto ObterPorId(int id)
        {
            Usuario usuario = _repository.ObterPorId(id);
            if(usuario == null)
            {
                throw new DomainException("Usuario não existe.");
            }
            return LerDto(usuario);
        }
    }
}
