using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public class Bomba
    {
        public int Numero { get; set; }

        // G: Indica si la bomba está actualmente en uso true = ocupada, false = disponible
        public bool Ocupada { get; set; }
        public int VecesUsada { get; set; }

        // G: Marca la bomba como ocupada y aumenta el contador de uso
        public void Iniciar()
        {
            Ocupada = true;
            VecesUsada++;
        }
        public void Finalizar()
        {
            Ocupada = false;
        }
    }
}