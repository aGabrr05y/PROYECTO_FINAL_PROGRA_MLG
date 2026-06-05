using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public class AbastecimientoPrepago : Abastecimiento
    {
        public double MontoPagado { get; set; }
        // opcional: almacenar límite en litros calculado
        public double LimiteLitros { get; set; }

        public override void Procesar()
        {
            // Al procesar un prepago, LitrosServidos debe venir del Arduino
            // (establecido en ControladorCentral.RecibirRespuestaJSON). Aquí
            // calculamos el total a cobrar en base a los litros realmente servidos.
            LitrosServidos = Math.Round(LitrosServidos, 2);
            // En prepago el cobro ES el monto pagado exacto, no se recalcula
            TotalCobrado = Math.Round(MontoPagado, 2);
            // Nota: si desea manejar reembolsos (cuando el monto pagado > total cobrado),
            // se puede calcular: var reembolso = Math.Round(MontoPagado - TotalCobrado, 2);
        }
    }
}
