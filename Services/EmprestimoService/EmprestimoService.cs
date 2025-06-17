using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EmprestimoLivros.Services.EmprestimoService
{
    public class EmprestimoService : IEmprestimoInterface
    {
        public EmprestimoService(ApplicationDbContext context)
        {
            Context = context;
        }

        public ApplicationDbContext Context { get; }

        public async Task<ResponseModel<List<EmprestimosModel>>> BuscarEmprestimos()
        {
            var responseModel = new ResponseModel<List<EmprestimosModel>>();
            try
            {
                var listaEmprestimos = await Context.Emprestimos.ToListAsync();
                responseModel.Dados = listaEmprestimos;
                responseModel.Mensagem = "Dados coletados com sucesso!";

                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Mensagem = ex.Message;
                responseModel.Status = false;

                return responseModel;
            }
        }

        public async Task<ResponseModel<EmprestimosModel>> BuscarEmprestimoPorId(int id)
        {
            var response = new ResponseModel<EmprestimosModel>();
            try
            {
                if (id == null)
                {
                    response.Mensagem = "Emprestimo não localizado!";
                    response.Status = false;
                    return response;
                }

                var emprestimo = Context.Emprestimos.FirstOrDefault(x => x.Id == id);

                if(emprestimo == null)
                {
                    response.Mensagem = "Emprestimo não localizado!";
                    response.Status = false;
                    return response;
                }

                response.Dados = emprestimo;
                response.Mensagem = "dados coletados com sucesso!";

                return response;

            }catch(Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }

        public async Task<DataTable> BuscarDadosEmprestimoExcel()
        {
            DataTable dataTable = new DataTable();

            dataTable.TableName = "Dados emprestimo";

            dataTable.Columns.Add("Recebedor", typeof(string));
            dataTable.Columns.Add("Fornecedor", typeof(string));
            dataTable.Columns.Add("Livro", typeof(string));
            dataTable.Columns.Add("DataEmprestimo", typeof(string));

            var dados = BuscarEmprestimos().Result.Dados;

            if (dados.Count > 0)
            {
                dados.ForEach(emprestimo =>
                {
                    dataTable.Rows.Add(emprestimo.Recebedor, emprestimo.Fornecedor, emprestimo.LivroEmprestado, emprestimo.DataUltimaAtualizacao);
                });
            }

            return dataTable;
        }

        public async Task<ResponseModel<EmprestimosModel>> CadastrarEmprestimo(EmprestimosModel emprestimosModel)
        {
            var response = new ResponseModel<EmprestimosModel>();

            try
            {
                Context.Add(emprestimosModel);
                Context.SaveChangesAsync();
                response.Mensagem = "Cadastro realizado com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }

        public async Task<ResponseModel<EmprestimosModel>> EditarEmprestimo(EmprestimosModel emprestimosModel)
        {
            var response = new ResponseModel<EmprestimosModel>();
            try
            {
                var emprestimo = await BuscarEmprestimoPorId(emprestimosModel.Id);
                if (emprestimo.Status == false)
                    return emprestimo;

                emprestimo.Dados.LivroEmprestado = emprestimosModel.LivroEmprestado;
                emprestimo.Dados.Fornecedor = emprestimosModel.Fornecedor;
                emprestimo.Dados.Recebedor = emprestimosModel.Recebedor;

                Context.Update(emprestimo.Dados);
                await Context.SaveChangesAsync();
                response.Mensagem = "Edição realizada com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }

        public async Task<ResponseModel<EmprestimosModel>> RemoveEmprestimo(EmprestimosModel modelo)
        {
            var response = new ResponseModel<EmprestimosModel>();
            try
            {
                Context.Remove(modelo);
                await Context.SaveChangesAsync();
                response.Mensagem = "Excluido com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }
    }
}
