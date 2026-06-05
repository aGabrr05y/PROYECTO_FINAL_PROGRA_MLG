using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace PROYECTO_FINAL_PROGRA_MLG
{
    public class RegistroAbastecimientos
    {
        public List<Abastecimiento> Lista { get; set; } = new();

        public void Agregar(Abastecimiento a)
        {
            Lista.Add(a);
            GuardarEnArchivo();
        }

        public List<Abastecimiento> FiltrarPorDia(DateTime fecha)
        {
            return Lista.Where(x => x.FechaHora.Date == fecha.Date).ToList();
        }
        
        public List<Abastecimiento> ReportePrepago()
        {
            return Lista.Where(x => x is AbastecimientoPrepago).ToList();
        }

        public List<Abastecimiento> ReporteTanqueLleno()
        {
            return Lista.Where(x => x is AbastecimientoTanqueLleno).ToList();
        }

        public void GuardarEnArchivo()
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
            };

            File.WriteAllText("abastecimientos.json",
                Newtonsoft.Json.JsonConvert.SerializeObject(Lista, Newtonsoft.Json.Formatting.Indented, settings));
        }

        public void CargarDesdeArchivo()
        {
            if (File.Exists("abastecimientos.json"))
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
                };

                string json = File.ReadAllText("abastecimientos.json");
                Lista = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Abastecimiento>>(json, settings)
                        ?? new List<Abastecimiento>();
            }
        }

        public Bomba BombaMasUsada(Bomba[] bombas)
        {
            return bombas.OrderByDescending(b => b.VecesUsada).First();
        }

        public Bomba BombaMenosUsada(Bomba[] bombas)
        {
            return bombas.OrderBy(b => b.VecesUsada).First();
        }

    }
}
