using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public class AbastecimientoPrepago : Abastecimiento
    {
        public double MontoPagado { get; set; }
        public double LimiteLitros { get; set; }

        public bool DetenidoManualmente { get; set; } = false;

        public override void Procesar()
        {
            if (DetenidoManualmente)
            {
                // Se detuvo antes: cobrar solo lo que se sirvió realmente
                LitrosServidos = Math.Round(LitrosServidos, 2);
                TotalCobrado = Math.Round(LitrosServidos * PrecioPorLitro, 2);
            }
            else
            {
                // Completó normalmente: cobrar el monto completo pagado
                LitrosServidos = Math.Round(LimiteLitrosRequested, 2);
                TotalCobrado = Math.Round(MontoPagado, 2);
            }
        }
    }
}
