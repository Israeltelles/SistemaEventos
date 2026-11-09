using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEventos.Models
{
    [Table("tbl_Presenca")]
    public class PresencaModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("codigo_presenca")]
        public string CodigoPresenca { get; set; } = string.Empty;

        [Required]
        public int UsuarioId { get; set; }

        public UsuarioModel Usuario { get; set; } = null!;

        [Required]
        public int EventoId { get; set; }

        public EventoModel Evento { get; set; } = null!;

        [Required]
        public bool Confirmacao { get; set; }

        public DateTime DataConfirmacao { get; set; }
    }
}
