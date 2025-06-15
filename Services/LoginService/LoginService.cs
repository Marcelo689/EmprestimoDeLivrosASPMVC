using EmprestimoLivros.Data;
using EmprestimoLivros.DTO;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.SenhaService;

namespace EmprestimoLivros.Services.LoginService
{
    public class LoginService : ILoginInterface
    {
        private ApplicationDbContext _context;
        private ISenhaInterface _senha;
        public LoginService(ApplicationDbContext context,ISenhaInterface senhaInterface)
        {
            _context = context;
            _senha = senhaInterface;
        }
        public async Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioRegisterDTO dto)
        {
            ResponseModel<UsuarioModel> response = new();

            try
            {
                if (VerificarSeEmailExiste(dto))
                {
                    response.Mensagem = "Email já cadastrado";
                    response.Status = false;
                    return response;
                }

                _senha.CriarSenhaHash(dto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                var modelo = new UsuarioModel();

                modelo.Nome = dto.Nome;
                modelo.Email = dto.Email;
                modelo.Sobrenome = dto.Sobrenome;
                modelo.SenhaHash = senhaHash;
                modelo.SenhaSalt = senhaSalt;

                _context.Add(modelo);
                await _context.SaveChangesAsync();

                response.Mensagem = "Usuario cadastrado com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        private bool VerificarSeEmailExiste(UsuarioRegisterDTO dto)
        {
            return _context.Usuarios.Any(e => e.Email == dto.Email);
        }
    }
}
