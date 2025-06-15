
using ClosedXML.Excel;
using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.SessaoService;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EmprestimoLivros.Controllers
{
    public class EmprestimoController : Controller
    {
        private ApplicationDbContext _db { get; }
        public ISessaoInterface SessaoInterface { get; }

        public EmprestimoController(ApplicationDbContext db, ISessaoInterface sessaoInterface)
        {
            _db = db;
            SessaoInterface = sessaoInterface;
        }

        public IActionResult Index()
        {
            var usuario = SessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }


            var listaEmprestimos = _db.Emprestimos.Select( e => (EmprestimoDTO) e).ToList();
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
        public IActionResult Cadastrar(EmprestimosModel modelo)
        {
            if (ModelState.IsValid)
            {
                modelo.DataUltimaAtualizacao = DateTime.Now;
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
            var usuario = SessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }
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
                var emprestimoDb = _db.Emprestimos.Find(modelo.Id);

                emprestimoDb.Fornecedor = modelo.Fornecedor;
                emprestimoDb.Recebedor  = modelo.Recebedor;
                emprestimoDb.LivroEmprestado = modelo.LivroEmprestado;

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
            var usuario = SessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }
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

        public IActionResult Exportar()
        {
            var dados = GetDados();

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

        private DataTable GetDados()
        {
            DataTable dataTable = new DataTable();

            dataTable.TableName = "Dados emprestimo";

            dataTable.Columns.Add("Recebedor", typeof(string));
            dataTable.Columns.Add("Fornecedor", typeof(string));
            dataTable.Columns.Add("Livro", typeof(string));
            dataTable.Columns.Add("DataEmprestimo", typeof(string));

            var dados = _db.Emprestimos.ToList();

            if(dados.Count > 0)
            {
                dados.ForEach( emprestimo =>
                {
                    dataTable.Rows.Add(emprestimo.Recebedor, emprestimo.Fornecedor, emprestimo.LivroEmprestado, emprestimo.DataUltimaAtualizacao);
                });
            }

            return dataTable;
        }
    }
}
