using System;
using System.Collections.Generic;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public class ControladorCentral
    {
        public Bomba[] Bombas = { new Bomba { Numero = 1 }, new Bomba { Numero = 2 } };
        public RegistroAbastecimientos Registro = new();

        public ControladorCentral()
        {
            // Cargar abastecimientos guardados al iniciar la aplicación
            Registro.CargarDesdeArchivo();
        }

        public string EnviarOrdenJSON(Abastecimiento ab)
        {
            if (ab is AbastecimientoPrepago prepago)
            {
                var obj = new
                {
                    bomba = ab.NumeroBomba,
                    monto = prepago.MontoPagado
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            else
            {
                var obj = new
                {
                    bomba = ab.NumeroBomba,
                    tanqueLleno = true
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }



        }

        public void RecibirRespuestaJSON(string json)
        {
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            int bomba = data.bomba;
            double litros = data.litrosServidos;

            var ultimo = Registro.Lista.Last(x => x.NumeroBomba == bomba);

            ultimo.LitrosServidos = litros;
            ultimo.Procesar();

            Registro.GuardarEnArchivo();
        }
    }
}
