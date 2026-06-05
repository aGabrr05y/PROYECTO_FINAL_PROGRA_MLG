using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public class AbastecimientoTanqueLleno : Abastecimiento
    {
        // L: Calcula el total a cobrar multiplicando litros servidos por precio por litro
        public override void Procesar()
        {
            // L: Total = litros × precio, redondeado a 2 decimales
            TotalCobrado = Math.Round(LitrosServidos * PrecioPorLitro, 2);

            LitrosServidos = Math.Round(LitrosServidos, 2);
        }
    }
}
