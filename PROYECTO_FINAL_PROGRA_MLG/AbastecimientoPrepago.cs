using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public class AbastecimientoPrepago : Abastecimiento
    {
        public double MontoPagado { get; set; }

        public override void Procesar()
        {
            // Calcular y redondear a 2 decimales
            LitrosServidos = Math.Round(MontoPagado / PrecioPorLitro, 2);
            TotalCobrado = Math.Round(MontoPagado, 2);
        }
    }
}
