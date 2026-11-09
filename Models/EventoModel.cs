using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEventos.Models
{
    [Table("tbl_Evento")]
    public class EventoModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do evento é obrigatório")]
        [MaxLength(100, ErrorMessage = "Máximo de 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [MaxLength(500, ErrorMessage = "Máximo de 500 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data é obrigatória")]
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "O local é obrigatório")]
        [MaxLength(50, ErrorMessage = "Máximo de 50 caracteres")]
        public string Local { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hora de início é obrigatória")]
        [Column("hora_inicio")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "Hora de fim é obrigatória")]
        [Column("hora_fim")]
        public TimeSpan HoraFim { get; set; }

        [Required]
        [ForeignKey("Coordenador")]
        public int CoordenadorId { get; set; }

        public UsuarioModel Coordenador { get; set; } = null!;
    }
}