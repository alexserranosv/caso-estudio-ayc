using System;

namespace CasoEstudioAYC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== EVALUADOR DE EXPRESIONES REGULARES USANDO ANF-ε ===");
            Console.WriteLine("Caso de Estudio AYC104 - Autómatas y Compiladores\n");

            // Crear instancia del evaluador
            EvaluadorExpresionRegular evaluador = new EvaluadorExpresionRegular();

            // === CONFIGURAR AUTÓMATA PARA LA EXPRESIÓN REGULAR (a|b)*abb ===
            // Este autómata acepta cadenas que terminan en "abb" sobre el alfabeto {a,b}
            Console.WriteLine("Configurando autómata para la expresión regular: (a|b)*abb");
            Console.WriteLine("Descripción: Acepta todas las palabras que terminan en 'abb'\n");

            // Definir estados: Q = {q0, q1, q2, q3}
            string[] estados = { "q0", "q1", "q2", "q3" };
            bool estadosDefinidos = evaluador.DefinirConjuntoEstados(estados);
            Console.WriteLine($"Estados definidos: {(estadosDefinidos ? "✓" : "✗")} {string.Join(", ", estados)}");

            // Definir alfabeto: Σ = {a, b}
            char[] alfabeto = { 'a', 'b' };
            bool alfabetoDefinido = evaluador.DefinirAlfabeto(alfabeto);
            Console.WriteLine($"Alfabeto definido: {(alfabetoDefinido ? "✓" : "✗")} {{{string.Join(", ", alfabeto)}}}");

            // Definir estado inicial: q0
            bool estadoInicialDefinido = evaluador.DefinirEstadoInicial("q0");
            Console.WriteLine($"Estado inicial definido: {(estadoInicialDefinido ? "✓" : "✗")} q0");

            // Definir estados finales: F = {q3}
            string[] estadosFinales = { "q3" };
            bool estadosFinalesDefinidos = evaluador.DefinirEstadosFinales(estadosFinales);
            Console.WriteLine($"Estados finales definidos: {(estadosFinalesDefinidos ? "✓" : "✗")} {{{string.Join(", ", estadosFinales)}}}");

            // Definir función de transición δ
            Console.WriteLine("\nDefiniendo transiciones:");
            
            // Desde q0: bucle para cualquier símbolo, transición a q1 con 'a'
            bool t1 = evaluador.AgregarTransicion("q0", "q0", 'a');
            bool t2 = evaluador.AgregarTransicion("q0", "q0", 'b');
            bool t3 = evaluador.AgregarTransicion("q0", "q1", 'a');
            Console.WriteLine($"  δ(q0, a) = {{q0, q1}} - {(t1 && t3 ? "✓" : "✗")}");
            Console.WriteLine($"  δ(q0, b) = {{q0}} - {(t2 ? "✓" : "✗")}");

            // Desde q1: transición a q2 con 'b'
            bool t4 = evaluador.AgregarTransicion("q1", "q2", 'b');
            Console.WriteLine($"  δ(q1, b) = {{q2}} - {(t4 ? "✓" : "✗")}");

            // Desde q2: transición a q3 con 'b' (estado final)
            bool t5 = evaluador.AgregarTransicion("q2", "q3", 'b');
            Console.WriteLine($"  δ(q2, b) = {{q3}} - {(t5 ? "✓" : "✗")}");

            Console.WriteLine($"\nQuíntuplo completo: {(evaluador.QuintuploCompleto ? "✓" : "✗")}");

            if (evaluador.QuintuploCompleto)
            {
                // Mostrar tabla de transiciones
                Console.WriteLine();
                evaluador.MostrarTablaDeTransiciones();

                // === PRUEBAS CON PALABRAS ===
                Console.WriteLine("=== PRUEBAS DE EVALUACIÓN ===");
                
                // Palabras que DEBEN ser aceptadas (terminan en 'abb')
                string[] palabrasAceptadas = { "abb", "aabb", "babb", "ababb", "baabb", "aaabb", "bbabb" };
                Console.WriteLine("\nPalabras que DEBEN ser ACEPTADAS:");
                foreach (string palabra in palabrasAceptadas)
                {
                    char resultado = evaluador.EvaluarPalabra(palabra);
                    string estado = ObtenerDescripcionResultado(resultado);
                    string simbolo = resultado == 's' ? "✓" : "✗";
                    Console.WriteLine($"  '{palabra}' → {resultado} ({estado}) {simbolo}");
                }

                // Palabras que DEBEN ser rechazadas (no terminan en 'abb')
                string[] palabrasRechazadas = { "a", "b", "ab", "ba", "aa", "bb", "aba", "bba", "abab", "abba" };
                Console.WriteLine("\nPalabras que DEBEN ser RECHAZADAS:");
                foreach (string palabra in palabrasRechazadas)
                {
                    char resultado = evaluador.EvaluarPalabra(palabra);
                    string estado = ObtenerDescripcionResultado(resultado);
                    string simbolo = resultado == 'n' ? "✓" : "✗";
                    Console.WriteLine($"  '{palabra}' → {resultado} ({estado}) {simbolo}");
                }

                // Prueba con palabra vacía
                Console.WriteLine("\nPrueba especial:");
                char resultadoVacio = evaluador.EvaluarPalabra("");
                string estadoVacio = ObtenerDescripcionResultado(resultadoVacio);
                string simboloVacio = resultadoVacio == 'n' ? "✓" : "✗";
                Console.WriteLine($"  '' (palabra vacía) → {resultadoVacio} ({estadoVacio}) {simboloVacio}");

                // Prueba con símbolos inválidos
                Console.WriteLine("\nPrueba con símbolos fuera del alfabeto:");
                char resultadoInvalido = evaluador.EvaluarPalabra("abc");
                string estadoInvalido = ObtenerDescripcionResultado(resultadoInvalido);
                string simboloInvalido = resultadoInvalido == 'p' ? "✓" : "✗";
                Console.WriteLine($"  'abc' → {resultadoInvalido} ({estadoInvalido}) {simboloInvalido}");

                // === RESUMEN DE LA IMPLEMENTACIÓN ===
                Console.WriteLine("\n=== RESUMEN DE LA IMPLEMENTACIÓN ===");
                Console.WriteLine("✓ Autómata Finito No Determinista con transiciones ε (ANF-ε)");
                Console.WriteLine("✓ Expresión regular equivalente: (a|b)*abb");
                Console.WriteLine("✓ Alfabeto: {a, b} (solo letras según requisitos)");
                Console.WriteLine("✓ Símbolo para transiciones vacías: ε (epsilon)");
                Console.WriteLine("✓ Clase principal con múltiples clases complementarias");
                Console.WriteLine("✓ Métodos públicos que solo reciben tipos simples");
                Console.WriteLine("✓ Evaluación usando algoritmo ANF-ε con clausura epsilon");
                Console.WriteLine("✓ Tabla de transiciones con estados inicial y finales marcados");
                
                Console.WriteLine("\n=== CÓDIGOS DE RETORNO ===");
                Console.WriteLine("  's': Palabra satisface la expresión regular");
                Console.WriteLine("  'n': Palabra no cumple la expresión regular");
                Console.WriteLine("  'p': Palabra contiene símbolos fuera del alfabeto");
                Console.WriteLine("  'e': Error - quíntuplo incompleto");
            }
            else
            {
                Console.WriteLine("Error: No se pudo configurar el quíntuplo completo.");
            }

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        private static string ObtenerDescripcionResultado(char codigo)
        {
            return codigo switch
            {
                's' => "Palabra ACEPTA la expresión regular",
                'n' => "Palabra NO acepta la expresión regular",
                'p' => "Palabra contiene símbolos inválidos",
                'e' => "Error: quíntuplo incompleto",
                _ => "Código desconocido"
            };
        }
    }
}
