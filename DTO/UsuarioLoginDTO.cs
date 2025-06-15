using System.ComponentModel.DataAnnotations;

namespace EmprestimoLivros.DTO
{
    public class UsuarioLoginDTO
    {
        [Required(ErrorMessage = "Digite um email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Digite a senha!")]
        public string Senha { get; set; }
    }
}
