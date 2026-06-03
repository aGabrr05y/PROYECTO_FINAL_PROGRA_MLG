using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public class Cliente
    {
        public string Nombre { get; set; }
        public string NIT { get; set; }
        public bool ConsumidorFinal { get; set; }

        public override string ToString()
        {
            // Mostrar el nombre si existe, si no mostrar el NIT (o "CF" para consumidor final)
            if (!string.IsNullOrWhiteSpace(Nombre)) return Nombre;
            if (!string.IsNullOrWhiteSpace(NIT)) return NIT;
            return string.Empty;
        }
    }
}
