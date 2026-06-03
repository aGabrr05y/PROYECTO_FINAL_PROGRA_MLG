using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public class ControladorCentral
    {
        public Bomba[] Bombas = { new Bomba { Numero = 1 }, new Bomba { Numero = 2 } };
        public RegistroAbastecimientos Registro = new();

        public string EnviarOrdenJSON(Abastecimiento ab)
        {
            var json = new
            {
                bomba = ab.NumeroBomba,
                tipo = ab is AbastecimientoPrepago ? "prepago" : "tanqueLleno",
                monto = (ab as AbastecimientoPrepago)?.MontoPagado,
                precioPorLitro = ab.PrecioPorLitro
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(json);



        }

        public void RecibirRespuestaJSON(string json)
        {
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            int bomba = data.bomba;
            double litros = data.litrosServidos;

            // Use LastOrDefault and handle missing entry to avoid InvalidOperationException
            var ultimo = Registro.Lista.LastOrDefault(x => x.NumeroBomba == bomba);

            if (ultimo == null)
            {
                // No matching abastecimiento found for the reported pump.
                // Gracefully ignore or log the event. For now, do nothing.
                return;
            }

            ultimo.LitrosServidos = litros;
            ultimo.Procesar();

            Registro.GuardarEnArchivo();
        }
    }
}
