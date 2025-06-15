using EmprestimoLivros.DTO;
using EmprestimoLivros.Services.LoginService;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLivros.Controllers
{
    public class LoginController : Controller
    {
        public LoginController(ILoginInterface loginInterface)
        {
            LoginInterface = loginInterface;
        }

        public ILoginInterface LoginInterface { get; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(UsuarioRegisterDTO dto)
        {
            if (ModelState.IsValid)
            {
                var usuario = await LoginInterface.RegistrarUsuario(dto);

                if (usuario.Status)
                {
                    TempData["MensagemSucesso"] = usuario.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = usuario.Mensagem;
                    return View(dto);
                }

                return RedirectToAction("Index");
            }
            return View(dto);
        }
    }
}
