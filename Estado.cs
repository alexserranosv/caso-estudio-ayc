using System;

namespace CasoEstudioAYC
{
    /// <summary>
    /// Representa un estado individual del autómata finito no determinista
    /// </summary>
    public class Estado
    {
        public string Nombre { get; private set; }
        public bool EsEstadoFinal { get; private set; }

        public Estado(string nombre, bool esEstadoFinal = false)
        {
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            EsEstadoFinal = esEstadoFinal;
        }

        public void EstablecerComoFinal()
        {
            EsEstadoFinal = true;
        }

        public override string ToString()
        {
            return EsEstadoFinal ? $"{Nombre}*" : Nombre;
        }

        public override bool Equals(object obj)
        {
            return obj is Estado estado && Nombre == estado.Nombre;
        }

        public override int GetHashCode()
        {
            return Nombre.GetHashCode();
        }
    }
}