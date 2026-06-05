using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public class Cliente
    {
        public string Nombre { get; set; }

        public string NIT { get; set; }

        // L: True = consumidor final, False = cliente facturado
        public bool ConsumidorFinal { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Nombre)) return Nombre;
            if (!string.IsNullOrWhiteSpace(NIT)) return NIT;
            return string.Empty;
        }
    }
}