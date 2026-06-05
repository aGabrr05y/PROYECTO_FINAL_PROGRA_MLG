using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public static class Config
    {
        // Precio por litro en su moneda. Ajuste según tarifa real.
        public static double PrecioPorLitro { get; set; } = 37.35;
        // Pulsos por litro estimado - puede actualizarse automáticamente
        public static double PulsosPorLitro { get; set; } = 100.0;
    }
}
