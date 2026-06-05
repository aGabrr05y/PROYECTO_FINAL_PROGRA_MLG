using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public class AbastecimientoPrepago : Abastecimiento
    {
        public double MontoPagado { get; set; }

        // M: Límite de litros que debería despacharse según el monto pagado (no se usa directamente aquí)
        public double LimiteLitros { get; set; }

        // M: Indica si el operador detuvo manualmente el despacho antes de completar el monto prepagado
        public bool DetenidoManualmente { get; set; } = false;

        // M: Método principal que calcula lo que realmente se cobra y sirve:
        //    - Si se detuvo manualmente → cobra solo por los litros realmente servidos
        //    - Si terminó normalmente → cobra el monto prepagado completo
        public override void Procesar()
        {
            if (DetenidoManualmente)
            {
                // M: Caso detenido antes de completar: solo se pagan los litros realmente surtidos
                LitrosServidos = Math.Round(LitrosServidos, 2);
                TotalCobrado = Math.Round(LitrosServidos * PrecioPorLitro, 2);
            }
            else
            {
                // M: Caso normal: se sirvieron los litros equivalentes al monto prepagado
                LitrosServidos = Math.Round(LimiteLitrosRequested, 2);
                TotalCobrado = Math.Round(MontoPagado, 2);
            }
        }
    }
}
