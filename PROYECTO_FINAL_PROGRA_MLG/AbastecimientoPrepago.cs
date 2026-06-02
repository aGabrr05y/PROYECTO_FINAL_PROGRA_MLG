using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    internal class AbastecimientoPrepago : Abastecimiento
    {
        public double MontoPagado { get; set; }

        public override void Procesar()
        {
            LitrosServidos = MontoPagado / PrecioPorLitro;
            TotalCobrado = MontoPagado;
        }
    }
}
