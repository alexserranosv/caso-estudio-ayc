using System;

namespace CasoEstudioAYC
{
    /// <summary>
    /// Representa una transición entre estados del autómata
    /// </summary>
    public class Transicion
    {
        public string EstadoOrigen { get; private set; }
        public string EstadoDestino { get; private set; }
        public char Simbolo { get; private set; }
        public bool EsTransicionVacia { get; private set; }

        public Transicion(string estadoOrigen, string estadoDestino, char simbolo)
        {
            EstadoOrigen = estadoOrigen ?? throw new ArgumentNullException(nameof(estadoOrigen));
            EstadoDestino = estadoDestino ?? throw new ArgumentNullException(nameof(estadoDestino));
            Simbolo = simbolo;
            EsTransicionVacia = (simbolo == 'ε'); // Símbolo epsilon para transiciones vacías
        }

        public bool PuedeTransicionar(char simboloEntrada)
        {
            return EsTransicionVacia || Simbolo == simboloEntrada;
        }

        public override string ToString()
        {
            string simboloMostrar = EsTransicionVacia ? "ε" : Simbolo.ToString();
            return $"({EstadoOrigen}, {simboloMostrar}) → {EstadoDestino}";
        }

        public override bool Equals(object obj)
        {
            return obj is Transicion transicion &&
                   EstadoOrigen == transicion.EstadoOrigen &&
                   EstadoDestino == transicion.EstadoDestino &&
                   Simbolo == transicion.Simbolo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EstadoOrigen, EstadoDestino, Simbolo);
        }
    }
}