using EmprestimoLivros.DTO;
using EmprestimoLivros.Models;

namespace EmprestimoLivros.Services.LoginService
{
    public interface ILoginInterface
    {
        Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioRegisterDTO dto);
        Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDTO dto);
        Task<ResponseModel<UsuarioModel>> RemoverUsuario(string email);
    }
}
