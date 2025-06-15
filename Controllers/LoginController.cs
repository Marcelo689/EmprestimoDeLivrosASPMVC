using EmprestimoLivros.DTO;
using EmprestimoLivros.Services.LoginService;
using EmprestimoLivros.Services.SessaoService;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLivros.Controllers
{
    public class LoginController : Controller
    {
        public LoginController(ILoginInterface loginInterface, ISessaoInterface sessaoInterface)
        {
            LoginInterface = loginInterface;
            SessaoInterface = sessaoInterface;
        }

        public ILoginInterface LoginInterface { get; }
        public ISessaoInterface SessaoInterface { get; }

        public IActionResult Login()
        {
            var usuario = SessaoInterface.BuscarSessao();
            if (usuario != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Logout()
        {
            SessaoInterface.RemoveSessao();
            return RedirectToAction("Login");
        }
        public IActionResult Registrar()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLoginDTO dto)
        {
            if (ModelState.IsValid)
            {
                var usuario = await LoginInterface.Login(dto);
                if (usuario.Status)
                {
                    TempData["MensagemSucesso"] = usuario.Mensagem;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["MensagemErro"] = usuario.Mensagem;
                    return View(dto);
                }
            }
            return View(dto);
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
