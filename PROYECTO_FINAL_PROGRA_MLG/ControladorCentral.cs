using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    // L:a comunicación entre la interfaz gráfica y el Arduino
    public class ControladorCentral
    {
        public Bomba[] Bombas = { new Bomba { Numero = 1 }, new Bomba { Numero = 2 } };

        public RegistroAbastecimientos Registro = new();

        // G: Escala de conversión: 1 litro "app" = 0.05 litros reales 50 ml
        private const double ESCALA_LITROS = 0.05;

        public ControladorCentral()
        {
            Registro.CargarDesdeArchivo();
        }

        // G: Genera JSON para enviar orden al Arduino
        //    Convierte litros app → litros reales usando la escala
        public string EnviarOrdenJSON(Abastecimiento a, double limiteLitrosApp = 0.0)
        {
            var j = new JObject();
            j["bomba"] = a.NumeroBomba;

            if (a is AbastecimientoTanqueLleno)
            {
                j["tanqueLleno"] = true; 
            }
            else if (limiteLitrosApp > 0.0)
            {
                double litrosReales = limiteLitrosApp * ESCALA_LITROS;
                j["limiteLitros"] = litrosReales; 
            }

            return j.ToString(Newtonsoft.Json.Formatting.None);
        }

        // G: Genera comando JSON para calibrar el medidor de flujo del Arduino
        public string GenerarComandoCalibracion(double pulsosPorLitro)
        {
            var j = new JObject();
            j["cmd"] = "calibrar";
            j["pulsosPorLitro"] = pulsosPorLitro;
            return j.ToString(Newtonsoft.Json.Formatting.None);
        }

        // G: Procesa las respuestas JSON que llegan desde el Arduino
        //    Actualiza litros servidos, marca bombas como libres y guarda en archivo
        public void RecibirRespuestaJSON(string json)
        {
            try
            {
                var j = JObject.Parse(json);

                // G: Ignorar mensajes de calibración y ACKs de inicio
                if (j["cmd"] != null) return;
                if (j["ok"] != null) return;

                int bomba = j.Value<int>("bomba");
                var ultimo = Registro.Lista.LastOrDefault(x => x.NumeroBomba == bomba);
                if (ultimo == null) return;

                // G: Actualizar litros servidos convierte reales a app
                if (j.TryGetValue("litrosServidos", out var t) && t.Type != JTokenType.Null)
                {
                    double litrosReales = t.Value<double>();
                    double litrosApp = litrosReales / ESCALA_LITROS;
                    ultimo.LitrosServidos = litrosApp;
                    Registro.GuardarEnArchivo();
                }

                // G: Si el despacho finalizó, procesa el total, libera la bomba y guarda
                if (j.TryGetValue("finalizado", out var tf) && tf.Type == JTokenType.Boolean && tf.Value<bool>())
                {
                    try { ultimo.Procesar(); } catch { }

                    int idx = bomba - 1;
                    if (Bombas != null && idx >= 0 && idx < Bombas.Length)
                        Bombas[idx].Ocupada = false;

                    Registro.GuardarEnArchivo();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RecibirRespuestaJSON error: {ex.Message}");
            }
        }

        // G: Genera comando JSON para detener una bomba manualmente
        public string EnviarComandoParada(int bomba)
        {
            var j = new JObject();
            j["bomba"] = bomba;
            j["comando"] = "parar";
            return j.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}