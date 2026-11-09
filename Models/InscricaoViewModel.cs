using System;
using System.Collections.Generic;

namespace SistemaEventos.Models
{
    public class EventosDisponiveisViewModel
    {
        public IEnumerable<EventoCardViewModel> Eventos { get; set; } = new List<EventoCardViewModel>();
        public string? Busca { get; set; }
    }

    public class EventoCardViewModel
    {
        public int EventoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string Local { get; set; } = string.Empty;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public string NomeCoordenador { get; set; } = string.Empty;
        public bool JaInscrito { get; set; }
    }

    public class MinhasInscricoesViewModel
    {
        public IEnumerable<InscricaoDetalheViewModel> Inscricoes { get; set; } = new List<InscricaoDetalheViewModel>();
    }

    public class InscricaoDetalheViewModel
    {
        public int InscricaoId { get; set; }
        public int EventoId { get; set; }
        public string NomeEvento { get; set; } = string.Empty;
        public DateTime DataEvento { get; set; }
        public string Local { get; set; } = string.Empty;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public DateTime DataInscricao { get; set; }
        public bool PresencaConfirmada { get; set; }
        public bool CertificadoDisponivel { get; set; }
    }
}