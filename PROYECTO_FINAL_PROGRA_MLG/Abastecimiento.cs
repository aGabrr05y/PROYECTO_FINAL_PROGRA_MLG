using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public abstract class Abastecimiento
    {
        public string Id { get; set; }

        public Cliente Cliente { get; set; }

        public int NumeroBomba { get; set; }

        public DateTime FechaHora { get; set; } = DateTime.Now;

        public double LitrosServidos { get; set; }

        public double TotalCobrado { get; set; }

        // M: Relación de pulsos por litro leída desde el ACK del dispositivo
        public double PulsosPorLitro { get; set; }

        // M: Límite de litros solicitado para iniciar una calibración automática del equipo
        public double LimiteLitrosRequested { get; set; }

        public bool Calibrado { get; set; }
        public double PrecioPorLitro { get; set; } = 37.35;

        // M: Método abstracto que define la lógica de procesamiento específica para cada tipo de abastecimiento (contado físico o facturado)
        public abstract void Procesar();
    }
}
