using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    internal class Bomba
    {
        public int Numero { get; set; }
        public bool Ocupada { get; set; }
        public int VecesUsada { get; set; }

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
