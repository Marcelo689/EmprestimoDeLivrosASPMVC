using EmprestimoLivros.Data;
using EmprestimoLivros.DTO;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.SenhaService;
using EmprestimoLivros.Services.SessaoService;

namespace EmprestimoLivros.Services.LoginService
{
    public class LoginService : ILoginInterface
    {
        private ApplicationDbContext _context;
        private ISenhaInterface _senha;
        public ISessaoInterface _sessaoInterface { get; }
        public LoginService(ApplicationDbContext context,ISenhaInterface senhaInterface, ISessaoInterface sessaoInterface)
        {
            _context = context;
            _senha = senhaInterface;
            _sessaoInterface = sessaoInterface;
        }


        public async Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDTO dto)
        {
            var response = new ResponseModel<UsuarioModel>();

            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(e => e.Email == dto.Email);

                if(usuario is null)
                {
                    response.Mensagem = "Credenciais Inválidas";
                    response.Status = false;
                    return response;
                }

                if(!_senha.VerificaSenha(dto.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    response.Mensagem = "Credenciais Inválidas";
                    response.Status = false;
                    return response;
                }

                //Criar sessão
                _sessaoInterface.CriarSessao(usuario);
                response.Mensagem = "Usuario logado com sucesso!";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
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
