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

            var ultimo = Registro.Lista.Last(x => x.NumeroBomba == bomba);

            ultimo.LitrosServidos = litros;
            ultimo.Procesar();

            Registro.GuardarEnArchivo();
        }
    }
}
