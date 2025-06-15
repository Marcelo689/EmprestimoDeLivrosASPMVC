using System.ComponentModel.DataAnnotations;

namespace EmprestimoLivros.Models
{
    public class EmprestimosModel
    {
        public int Id { get; set; }

        [Required( ErrorMessage ="Preencha o Recebedor")]
        public string Recebedor { get; set; }

        [Required(ErrorMessage = "Preencha o Fornecedor")]
        public string Fornecedor { get; set; }

        [Required(ErrorMessage = "Preencha o Livro a ser emprestado")]
        public string LivroEmprestado { get; set; } 
        public DateTime DataUltimaAtualizacao { get; set; } = DateTime.Now;
    }
}
