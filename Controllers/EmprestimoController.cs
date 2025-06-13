using EmprestimoLivros.Data;
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
            var listaEmprestimos = _db.Emprestimos.ToList();
            return View(listaEmprestimos);
        }
    }
}
