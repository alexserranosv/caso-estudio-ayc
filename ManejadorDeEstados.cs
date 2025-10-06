using System;
using System.Collections.Generic;
using System.Linq;

namespace CasoEstudioAYC
{
    /// <summary>
    /// Maneja conjuntos de estados activos durante la ejecución del autómata
    /// </summary>
    public class ManejadorDeEstados
    {
        private HashSet<string> estadosActivos;

        public ManejadorDeEstados()
        {
            estadosActivos = new HashSet<string>();
        }

        public void AgregarEstado(string nombreEstado)
        {
            if (!string.IsNullOrEmpty(nombreEstado))
            {
                estadosActivos.Add(nombreEstado);
            }
        }

        public void LimpiarEstados()
        {
            estadosActivos.Clear();
        }

        public bool ContieneEstado(string nombreEstado)
        {
            return estadosActivos.Contains(nombreEstado);
        }

        public bool TieneEstados()
        {
            return estadosActivos.Count > 0;
        }

        public string[] ObtenerEstadosComoArray()
        {
            return estadosActivos.ToArray();
        }

        public bool ContieneAlgunEstadoDe(string[] estadosFinales)
        {
            return estadosFinales.Any(estadoFinal => estadosActivos.Contains(estadoFinal));
        }

        public void EstablecerEstados(string[] nuevosEstados)
        {
            estadosActivos.Clear();
            foreach (string estado in nuevosEstados)
            {
                if (!string.IsNullOrEmpty(estado))
                {
                    estadosActivos.Add(estado);
                }
            }
        }

        public int ObtenerCantidadEstados()
        {
            return estadosActivos.Count;
        }
    }
}