using EmprestimoLivros.Data;
using EmprestimoLivros.DTO;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.SenhaService;
using EmprestimoLivros.Services.SessaoService;
using Microsoft.Extensions.Localization;
using System.Net;
using System.Net.Mail;

namespace EmprestimoLivros.Services.LoginService
{
    public class LoginService : ILoginInterface
    {
        private ApplicationDbContext _context;
        private ISenhaInterface _senha;
        public ISessaoInterface _sessaoInterface { get; }
        private readonly IStringLocalizer<LoginService> _localizer;
        public LoginService(ApplicationDbContext context,ISenhaInterface senhaInterface, ISessaoInterface sessaoInterface, IStringLocalizer<LoginService> localizer)
        {
            _context = context;
            _senha = senhaInterface;
            _sessaoInterface = sessaoInterface;
            _localizer = localizer;
        }

        public async Task<ResponseModel<UsuarioModel>> RemoverUsuario(string email)
        {
            var response = new ResponseModel<UsuarioModel>();
            try
            {
                if(string.IsNullOrEmpty(email))
                {
                    response.Mensagem = _localizer["emailNotFound"];
                    response.Status = false;
                }

                var usuario = _context.Usuarios.FirstOrDefault(e => e.Email.ToLower() == email.ToLower());
                if(usuario is null)
                {
                    response.Mensagem = _localizer["emailNotFound"];
                    response.Status = false;
                }

                response.Mensagem = _localizer["loginDeleted"];
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
                response.Dados = usuario;
                return response;
            }catch (Exception ex)
            {
                response.Status = false;
                response.Mensagem = ex.Message;
                return response;
            }
        }
        public async Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDTO dto)
        {
            var response = new ResponseModel<UsuarioModel>();

            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(e => e.Email == dto.Email);

                if(usuario is null)
                {
                    response.Mensagem = _localizer["invalidCredentials"];
                    response.Status = false;
                    return response;
                }

                if(!_senha.VerificaSenha(dto.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    response.Mensagem = _localizer["invalidCredentials"];
                    response.Status = false;
                    return response;
                }

                //Criar sessão
                _sessaoInterface.CriarSessao(usuario);
                response.Mensagem = _localizer["sucessOnLogin"];

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
                    response.Mensagem = _localizer["emailAlreadyInUse"];
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

                response.Mensagem = _localizer["userRegistered"];
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
