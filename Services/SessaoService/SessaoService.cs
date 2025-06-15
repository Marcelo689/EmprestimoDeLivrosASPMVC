using EmprestimoLivros.Models;
using Newtonsoft.Json;

namespace EmprestimoLivros.Services.SessaoService
{
    public class SessaoService : ISessaoInterface
    {
        private IHttpContextAccessor _contextAccessor;
        public SessaoService(IHttpContextAccessor acessor)
        {
            _contextAccessor = acessor;
        }
        public UsuarioModel BuscarSessao()
        {
            var sessaoUsuario = _contextAccessor.HttpContext.Session.GetString("sessaoUsuario");
            if (string.IsNullOrEmpty(sessaoUsuario))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);
        }

        public void CriarSessao(UsuarioModel usuario)
        {
            var usuarioJson = JsonConvert.SerializeObject(usuario);
            _contextAccessor.HttpContext.Session.SetString("sessaoUsuario", usuarioJson);
        }

        public void RemoveSessao()
        {
            _contextAccessor.HttpContext.Session.Remove("sessaoUsuario");
        }
    }
}
