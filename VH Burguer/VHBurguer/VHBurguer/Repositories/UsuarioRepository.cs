using VHBurguer.Contexts;
using VHBurguer.Domains;
using VHBurguer.Interfaces;

namespace VHBurguer.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly VH_BurguerContext _context;

        public UsuarioRepository(VH_BurguerContext context)
        {
            _context = context;
        }

        public List<Usuario> Listar()
        {
            return _context.Usuario.ToList();
        }

        public Usuario? ObterPorId(int id) {
            // find performa melhor com chave primaria
            return _context.Usuario.Find(id);
        }

        public Usuario? ObterPorEmail(string email)
        {
            // FirstOrDefault -> retorna nosso usuario do banco
            return _context.Usuario.FirstOrDefault(usuario => usuario.Email == email);
        }

        public bool EmailExiste(string email)
        {
            // Any retorna como true ou false, voce usa somente para validacoes
            return _context.Usuario.Any(usuario =>usuario.Email == email);
        }

        public void Adicionar(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            _context.SaveChanges();
        }

        public void Atualizar(Usuario usuario)
        {
            Usuario? usuarioBanco = _context.Usuario.FirstOrDefault(usuarioAux => usuario.UsuarioID == usuarioAux.UsuarioID);

            if(usuarioBanco == null)
            {
                return;
            }

            usuarioBanco.Nome = usuario.Nome;
            usuarioBanco.Email = usuario.Email;
            usuarioBanco.Senha = usuario.Senha;

            _context.SaveChanges();
        }

        public void Remover(int id)
        {
            Usuario? usuario = _context.Usuario.FirstOrDefault(usuarioAux => usuarioAux.UsuarioID == id);

            if (usuario == null)
            {
                return;
            }
            _context.Usuario.Remove(usuario);
            _context.SaveChanges();
        }
    }
}
