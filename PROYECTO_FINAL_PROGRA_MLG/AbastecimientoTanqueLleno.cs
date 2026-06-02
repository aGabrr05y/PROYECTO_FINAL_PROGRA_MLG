using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    internal class AbastecimientoTanqueLleno : Abastecimiento
    {
        public override void Procesar()
        {
            TotalCobrado = LitrosServidos * PrecioPorLitro;
        }
    }
}
