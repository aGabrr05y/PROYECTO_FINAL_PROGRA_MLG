using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public abstract class Abastecimiento
    {
        // Usar string para permitir valores como "CF" o el NIT del cliente
        public string Id { get; set; }
        public Cliente Cliente { get; set; }
        public int NumeroBomba { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.Now;
        public double LitrosServidos { get; set; }
        public double TotalCobrado { get; set; }
        // Propiedades para calibración automática
        // PulsosPorLitro leído desde el ACK del dispositivo (estado actual)
        public double PulsosPorLitro { get; set; }
        // Límite de litros solicitado para iniciar una calibración
        public double LimiteLitrosRequested { get; set; }
        // Indica si este abastecimiento ya fue calibrado
        public bool Calibrado { get; set; }
        public double PrecioPorLitro { get; set; } = 37.35;

        public abstract void Procesar();
    }
}
