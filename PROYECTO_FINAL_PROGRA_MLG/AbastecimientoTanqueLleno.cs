using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public class AbastecimientoTanqueLleno : Abastecimiento
    {
        public override void Procesar()
        {
            // Calcular total y redondear a 2 decimales
            TotalCobrado = Math.Round(LitrosServidos * PrecioPorLitro, 2);
            // Asegurar que litros servidos también se almacenen con 2 decimales
            LitrosServidos = Math.Round(LitrosServidos, 2);
        }
    }
}
