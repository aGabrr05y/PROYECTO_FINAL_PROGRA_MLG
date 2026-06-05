using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public class ControladorCentral
    {
        public Bomba[] Bombas = { new Bomba { Numero = 1 }, new Bomba { Numero = 2 } };
        public RegistroAbastecimientos Registro = new();

        // Escala: 1 litro "app" = 0.05 litros reales (50 ml)
        // Esto hace que Q37.35 → 1 litro app → 0.05 L reales enviados al Arduino
        private const double ESCALA_LITROS = 0.05;

        public ControladorCentral()
        {
            Registro.CargarDesdeArchivo();
        }

        /// <summary>
        /// Genera el JSON de orden para el Arduino.
        /// limiteLitros es en litros "app"; este método lo escala a litros reales antes de enviar.
        /// </summary>
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
                // Convertir litros app → litros reales para el Arduino
                double litrosReales = limiteLitrosApp * ESCALA_LITROS;
                j["limiteLitros"] = litrosReales;
            }

            return j.ToString(Newtonsoft.Json.Formatting.None);
        }

        public string GenerarComandoCalibracion(double pulsosPorLitro)
        {
            var j = new JObject();
            j["cmd"] = "calibrar";
            j["pulsosPorLitro"] = pulsosPorLitro;
            return j.ToString(Newtonsoft.Json.Formatting.None);
        }

        public void RecibirRespuestaJSON(string json)
        {
            try
            {
                var j = JObject.Parse(json);

                // Ignorar ACKs de calibración
                if (j["cmd"] != null) return;

                // Ignorar ACKs de inicio (tienen "ok":true)
                if (j["ok"] != null) return;

                int bomba = j.Value<int>("bomba");

                var ultimo = Registro.Lista.LastOrDefault(x => x.NumeroBomba == bomba);
                if (ultimo == null) return;

                if (j.TryGetValue("litrosServidos", out var t) && t.Type != JTokenType.Null)
                {
                    double litrosReales = t.Value<double>();
                    // Convertir litros reales → litros app para mostrar en UI
                    // 0.05 reales = 1 litro app
                    double litrosApp = litrosReales / ESCALA_LITROS;
                    ultimo.LitrosServidos = litrosApp;
                    Registro.GuardarEnArchivo();
                }

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

        public string EnviarComandoParada(int bomba)
        {
            var j = new JObject();
            j["bomba"] = bomba;
            j["comando"] = "parar";
            return j.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}