using EmprestimoLivros.Models;

namespace EmprestimoLivros.Services.SessaoService
{
    public interface ISessaoInterface
    {
        void CriarSessao(UsuarioModel usuario);
        void RemoveSessao();
        UsuarioModel BuscarSessao();
    }
}
