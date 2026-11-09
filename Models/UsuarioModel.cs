using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEventos.Models
{
    [Table("tbl_Usuario")]
    public class UsuarioModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(8, ErrorMessage = "Senha deve ter pelo menos 8 caracteres")]
        [MaxLength(256)]
        public string Senha { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? Telefone { get; set; }

        [MaxLength(20)]
        public string? Ra { get; set; }

        [Required]
        [MaxLength(20)]
        public string Tipo { get; set; } = string.Empty;
    }
}