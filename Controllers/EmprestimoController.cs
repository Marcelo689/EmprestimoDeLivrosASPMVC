using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLivros.Controllers
{
    public class EmprestimoController : Controller
    {
        private ApplicationDbContext _db { get; }
        public EmprestimoController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var listaEmprestimos = _db.Emprestimos.Select( e => (EmprestimoDTO) e).ToList();
            return View(listaEmprestimos);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(EmprestimosModel modelo)
        {
            if (ModelState.IsValid)
            {
                _db.Emprestimos.Add(modelo);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var modelo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);
            if (modelo == null)
                return NotFound();
            return View(modelo);
        }
        
        [HttpPost]
        public IActionResult Editar(EmprestimosModel modelo)
        {
            if (ModelState.IsValid)
            { 
                _db.Update(modelo);
                _db.SaveChanges();
                TempData["MensagemSucesso"] = "Editado com sucesso!";
                return RedirectToAction("Index");
            }
            TempData["MensagemErro"] = "Ocorreu algum erro!";
            return View(modelo);
        }
        [HttpGet]
        public IActionResult Excluir(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var modelo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);
            if (modelo == null)
                return NotFound();

            return View(modelo);
        }

        [HttpPost]
        public IActionResult Excluir(EmprestimosModel modelo)
        {
            if (modelo == null)
                return NotFound();

            _db.Remove(modelo);
            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Deletado com sucesso!";
            return RedirectToAction("Index");
        }
    }
}
