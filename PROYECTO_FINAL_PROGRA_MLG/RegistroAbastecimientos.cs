using System;
using System.Collections.Generic;
using System.Text;

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
            File.WriteAllText("abastecimientos.json",
                Newtonsoft.Json.JsonConvert.SerializeObject(Lista, Newtonsoft.Json.Formatting.Indented));
        }

        public void CargarDesdeArchivo()
        {
            if (File.Exists("abastecimientos.json"))
            {
                string json = File.ReadAllText("abastecimientos.json");
                Lista = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Abastecimiento>>(json);
            }
        }
    }
}
