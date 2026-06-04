using Newtonsoft.Json.Linq;
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
            try
            {
                var j = Newtonsoft.Json.Linq.JObject.Parse(json);

                int bomba = j.Value<int>("bomba");

                var ultimo = Registro.Lista.Last(x => x.NumeroBomba == bomba);

                if (j.TryGetValue("litrosServidos", out var t) && t.Type != Newtonsoft.Json.Linq.JTokenType.Null)
                {
                    double litros = t.Value<double>();
                    ultimo.LitrosServidos = litros;
                    ultimo.Procesar();
                    Registro.GuardarEnArchivo();
                }
                else
                {
                    // Mensaje sin litrosServidos (por ejemplo ACK): no actualizar.
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RecibirRespuestaJSON error: {ex.Message}");
                // Evitar que una excepción en parsing detenga el hilo de recepción.
            }
        }
    }
}
