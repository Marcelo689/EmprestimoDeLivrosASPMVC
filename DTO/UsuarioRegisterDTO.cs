using System.ComponentModel.DataAnnotations;

namespace EmprestimoLivros.DTO
{
    public class UsuarioRegisterDTO
    {
        [Required(ErrorMessage = "Digite um nome!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite um sobrenome!")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "Digite um email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Digite uma senha!")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Digite a confirmação de senha!"), Compare("Senha", ErrorMessage = "As senhas não são iguais")]

        public string ConfirmaSenha { get; set; }
    }
}
