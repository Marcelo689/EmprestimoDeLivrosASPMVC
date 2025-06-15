
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.EmprestimoService;
using EmprestimoLivros.Services.SessaoService;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace EmprestimoLivros.Controllers
{
    public class EmprestimoController : Controller
    {
        private ApplicationDbContext _db { get; }
        public IEmprestimoInterface EmprestimoInterface { get; }
        public ISessaoInterface SessaoInterface { get; }

        public EmprestimoController(ApplicationDbContext db, IEmprestimoInterface emprestimoInterface, ISessaoInterface sessaoInterface)
        {
            _db = db;
            EmprestimoInterface = emprestimoInterface;
            SessaoInterface = sessaoInterface;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = SessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var listaResponse = await EmprestimoInterface.BuscarEmprestimos();
            var listaEmprestimos = listaResponse.Dados.Select( e => (EmprestimoDTO) e).ToList();
            return View(listaEmprestimos);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            var usuario = SessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(EmprestimosModel modelo)
        {
            if (ModelState.IsValid)
            {
                var emprestimoResult = await EmprestimoInterface.CadastrarEmprestimo(modelo);

                if (emprestimoResult.Status)
                {
                    TempData["MensagemSucesso"] = emprestimoResult.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = emprestimoResult.Mensagem;
                    return View(modelo);
                }

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            var usuario = SessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var modelo = await EmprestimoInterface.BuscarEmprestimoPorId(id.Value);

            return View(modelo.Dados);
        }
        
        [HttpPost]
        public async Task<IActionResult> Editar(EmprestimosModel modelo)
        {
            if (ModelState.IsValid)
            {
                var emprestimoResponse = await EmprestimoInterface.EditarEmprestimo(modelo);

                if (emprestimoResponse.Status)
                {
                    TempData["MensagemSucesso"] = emprestimoResponse.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = emprestimoResponse.Mensagem;
                    return View(modelo);
                }
                return RedirectToAction("Index");
            }
            TempData["MensagemErro"] = "Ocorreu algum erro!";
            return View(modelo);
        }
        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            var usuario = SessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var modelo = await EmprestimoInterface.BuscarEmprestimoPorId(id.Value);

            return View(modelo.Dados);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(EmprestimosModel modelo)
        {
            if (modelo == null)
            {
                TempData["MensagemErro"] = "Emprestimo nao localizado";
            }

            var emprestimoResult = await EmprestimoInterface.RemoveEmprestimo(modelo);

            if (emprestimoResult.Status)
            {
                TempData["MensagemSucesso"] = emprestimoResult.Mensagem;
            }
            else
            {
                TempData["MensagemErro"] = emprestimoResult.Mensagem;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Exportar()
        {
            var dados = await EmprestimoInterface.BuscarDadosEmprestimoExcel();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.AddWorksheet(dados, "Dados Emprestimo");

                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spredsheetml.sheet", "Emprestimo.xls");
                }
            }
        }

    }
}
