using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaEventos.Models
{
    [Table("tbl_Certificado")]
    public class CertificadoModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        public UsuarioModel Usuario { get; set; } = null!;

        [Required]
        public int EventoId { get; set; }

        public EventoModel Evento { get; set; } = null!;

        [Column("data_emissao", TypeName = "date")]
        public DateTime DataEmissao { get; set; }
    }
}