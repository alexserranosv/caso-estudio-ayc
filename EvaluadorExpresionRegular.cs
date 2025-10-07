using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasoEstudioAYC
{
    /// <summary>
    /// Clase principal que evalúa palabras usando un Autómata Finito No Determinista con transiciones epsilon
    /// </summary>
    public class EvaluadorExpresionRegular
    {
        // Elementos del quintuplo (Q, Σ, δ, q0, F)
        public List<Estado> conjuntoEstados;
        public List<char> alfabeto;
        public List<Transicion> funcionTransicion;
        public string estadoInicial;
        public List<string> estadosFinales;

        private bool tieneTransicionesInvalidas;

        // Clases complementarias para el procesamiento
        private ManejadorDeEstados manejadorEstados;

        public EvaluadorExpresionRegular()
        {
            LimpiarQuintuplo();
        }

        /// <summary>
        /// Propiedad que indica si el quintuplo está completo y válido
        /// </summary>
        public bool QuintuploCompleto
        {
            get
            {
                return conjuntoEstados.Count > 0 &&
                       alfabeto.Count > 0 &&
                       funcionTransicion.Count > 0 &&
                       !string.IsNullOrEmpty(estadoInicial) &&
                       estadosFinales.Count > 0 &&
                       !tieneTransicionesInvalidas && // ← Nueva validación
                       ValidarConsistenciaDelQuintuplo();
            }
        }

        /// <summary>
        /// Define el conjunto de estados del autómata
        /// </summary>
        public bool DefinirConjuntoEstados(string[] nombresEstados, out string[] NombresEstado)
        {
            NombresEstado = Array.Empty<string>();

            if (nombresEstados == null || nombresEstados.Length == 0)
            {
                return false;
            }

            conjuntoEstados.Clear();

            List<string> lista = new List<string>(nombresEstados);
            lista.RemoveAll(item => string.IsNullOrWhiteSpace(item));

            NombresEstado = lista.ToArray();

            if (NombresEstado.Length == 0)
            {
                return false;
            }

            NombresEstado = lista.Distinct().ToArray();

            foreach (string nombre in NombresEstado)
                {
                    conjuntoEstados.Add(new Estado(nombre));
                }

                return true;
          
        }


        /// <summary>
        /// Define el alfabeto del autómata
        /// </summary>
        public bool DefinirAlfabeto(char[] simbolos, out char[] Simbolos)
        {
            Simbolos = Array.Empty<char>();

            if (simbolos == null || simbolos.Length == 0) {
                return false;
            }
               
            alfabeto.Clear();

            List<char> lista = new List<char>(simbolos);
            lista.RemoveAll(item => char.IsWhiteSpace(item));

            Simbolos = lista.ToArray();

            if (Simbolos.Length == 0) {
                return false;
            }

            Simbolos = lista.Distinct().ToArray();

            foreach (char simbolo in Simbolos)
            {
                // Solo letras y dígitos según requisitos
                if (char.IsLetterOrDigit(simbolo))
                {
                    alfabeto.Add(simbolo);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Agrega una transición al autómata
        /// </summary>
        public bool AgregarTransicion(string estadoOrigen, string estadoDestino, char simbolo)
        {
            if (string.IsNullOrEmpty(estadoOrigen) || string.IsNullOrEmpty(estadoDestino))
            {
                tieneTransicionesInvalidas = true;
                return false;
            }

            // Verificar que los estados existan
            if (!ExisteEstado(estadoOrigen) || !ExisteEstado(estadoDestino))
            {
                tieneTransicionesInvalidas = true;
                return false;
            }

            // Verificar que el símbolo esté en el alfabeto o sea epsilon
            if (simbolo != 'ε' && !alfabeto.Contains(simbolo))
            {
                tieneTransicionesInvalidas = true;
                return false;
            }

            Transicion nuevaTransicion = new Transicion(estadoOrigen, estadoDestino, simbolo);

            if (funcionTransicion.Contains(nuevaTransicion))
            {
                tieneTransicionesInvalidas = true;
               return false;
            }

            funcionTransicion.Add(nuevaTransicion);
            return true;
        }

        /// <summary>
        /// Define el estado inicial del autómata
        /// </summary>
        public bool DefinirEstadoInicial(string nombreEstado)
        {
            if (string.IsNullOrEmpty(nombreEstado) || !ExisteEstado(nombreEstado))
                return false;

            estadoInicial = nombreEstado;
            return true;
        }

        /// <summary>
        /// Define los estados finales del autómata
        /// </summary>
        public bool DefinirEstadosFinales(string[] nombresEstadosFinales, out string[] NombresEstadosFinales)
        {
            NombresEstadosFinales = Array.Empty<string>();

            if (nombresEstadosFinales == null || nombresEstadosFinales.Length == 0)
                return false;

            estadosFinales.Clear();

            List<string> lista = new List<string>(nombresEstadosFinales);
            lista.RemoveAll(item => string.IsNullOrWhiteSpace(item));

            NombresEstadosFinales = lista.ToArray();

            if (NombresEstadosFinales.Length == 0)
            {
                return false;
            }

            NombresEstadosFinales = lista.Distinct().ToArray();

            foreach (string nombre in NombresEstadosFinales)
            {
                if (string.IsNullOrEmpty(nombre) || !ExisteEstado(nombre))
                    return false;
                
                estadosFinales.Add(nombre);
                // Marcar el estado como final
                Estado estado = conjuntoEstados.First(e => e.Nombre == nombre);
                estado.EstablecerComoFinal();
            }
            return true;
        }

        /// <summary>
        /// Evalúa si una palabra satisface la expresión regular definida por el autómata
        /// Retorna: 'e' = error, 'p' = palabra inválida, 's' = satisface, 'n' = no satisface
        /// </summary>
        public char EvaluarPalabra(string palabra)
        {
            if (!QuintuploCompleto)
                return 'e';

            if (string.IsNullOrEmpty(palabra))
                palabra = ""; // Palabra vacía

            // Verificar que todos los símbolos estén en el alfabeto
            foreach (char simbolo in palabra)
            {
                if (!alfabeto.Contains(simbolo))
                    return 'p';
            }

            // Procesar la palabra usando el algoritmo ANF-ε
            return ProcesarPalabraConANF(palabra) ? 's' : 'n';
        }

        /// <summary>
        /// Limpia el quintuplo para recibir una nueva definición de autómata
        /// </summary>
        public void LimpiarQuintuplo()
        {
            conjuntoEstados = new List<Estado>();
            alfabeto = new List<char>();
            funcionTransicion = new List<Transicion>();
            estadoInicial = null;
            estadosFinales = new List<string>();
            manejadorEstados = new ManejadorDeEstados();
            tieneTransicionesInvalidas = false; // ← Reiniciar el flag
        }

        private bool ExisteEstado(string nombreEstado)
        {
            return conjuntoEstados.Any(e => e.Nombre == nombreEstado);
        }

        private bool ValidarConsistenciaDelQuintuplo()
        {
            // Verificar que el estado inicial existe
            if (!ExisteEstado(estadoInicial))
                return false;

            // Verificar que todos los estados finales existen
            foreach (string estadoFinal in estadosFinales)
            {
                if (!ExisteEstado(estadoFinal))
                    return false;
            }

            // Verificar que todas las transiciones tienen estados válidos

            foreach (Transicion transicion in funcionTransicion)
            {
                // Origen y destino deben existir
                if (!ExisteEstado(transicion.EstadoOrigen) || !ExisteEstado(transicion.EstadoDestino))
                    return false;

                // Símbolo debe pertenecer al alfabeto (excepto epsilon)
                if (transicion.Simbolo != 'ε' && !alfabeto.Contains(transicion.Simbolo))
                {
                    return false;
                }

            }
       
            return true;
        }

        private bool ProcesarPalabraConANF(string palabra)
        {
            // Inicializar con el estado inicial y su clausura epsilon
            manejadorEstados.LimpiarEstados();
            manejadorEstados.AgregarEstado(estadoInicial);
            CalcularClausuraEpsilon();

            // Procesar cada símbolo de la palabra
            foreach (char simbolo in palabra)
            {
                ProcesarSimboloEntrada(simbolo);
                
                if (!manejadorEstados.TieneEstados())
                    return false; // No hay estados activos, palabra rechazada
            }

            // Verificar si algún estado activo es final
            return manejadorEstados.ContieneAlgunEstadoDe(estadosFinales.ToArray());
        }

        private void ProcesarSimboloEntrada(char simbolo)
        {
            string[] estadosAnteriores = manejadorEstados.ObtenerEstadosComoArray();
            manejadorEstados.LimpiarEstados();

            // Para cada estado anterior, aplicar transiciones con el símbolo
            foreach (string estadoActual in estadosAnteriores)
            {
                var transicionesAplicables = funcionTransicion
                    .Where(t => t.EstadoOrigen == estadoActual && t.Simbolo == simbolo)
                    .ToList();

                foreach (Transicion transicion in transicionesAplicables)
                {
                    manejadorEstados.AgregarEstado(transicion.EstadoDestino);
                }
            }

            // Calcular clausura epsilon de los nuevos estados
            CalcularClausuraEpsilon();
        }

        private void CalcularClausuraEpsilon()
        {
            bool cambiosRealizados = true;
            
            while (cambiosRealizados)
            {
                cambiosRealizados = false;
                string[] estadosActuales = manejadorEstados.ObtenerEstadosComoArray();

                foreach (string estado in estadosActuales)
                {
                    var transicionesEpsilon = funcionTransicion
                        .Where(t => t.EstadoOrigen == estado && t.EsTransicionVacia)
                        .ToList();

                    foreach (Transicion transicion in transicionesEpsilon)
                    {
                        if (!manejadorEstados.ContieneEstado(transicion.EstadoDestino))
                        {
                            manejadorEstados.AgregarEstado(transicion.EstadoDestino);
                            cambiosRealizados = true;
                        }
                    }
                }
            }
        }
    }
}