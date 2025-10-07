using System;

namespace CasoEstudioAYC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== EVALUADOR DE EXPRESIONES REGULARES USANDO ANF-ε ===\n");
            Console.WriteLine("Caso de Estudio AYC104 - Autómatas y Compiladores\n");

            // Crear instancia del evaluador
            EvaluadorExpresionRegular evaluador = new EvaluadorExpresionRegular();

            String er = "(j*k+m?h)e";

            Console.WriteLine($"Configurando autómata para la expresión regular: {er}\n");

            Console.WriteLine("Quintuplo del automata definido:\n");

            // Definir estados
            string[] estados = {"q0","q1", "q2", "q3", "q4", "q5", "q6", "q7"};
            bool estadosDefinidos = evaluador.DefinirConjuntoEstados(estados, out estados);
            Console.WriteLine($"Q - Estados definidos: {{{string.Join(", ", estados)}}} -> {(estadosDefinidos ? true : false)}");

            // Definir alfabeto
            char[] alfabeto = {'m', 'h', 'j', 'k', 'e'};
            bool alfabetoDefinido = evaluador.DefinirAlfabeto(alfabeto, out alfabeto);
            Console.WriteLine($"A - Alfabeto definido: {{{string.Join(", ", alfabeto)}}} -> {(alfabetoDefinido ? true : false)}");

            // Definir estado inicial
            String Estadoinicial = "q5";
            bool estadoInicialDefinido = evaluador.DefinirEstadoInicial(Estadoinicial);
            Console.WriteLine($"s - Estado inicial definido: {(string.IsNullOrWhiteSpace(Estadoinicial) ? "No definido":Estadoinicial)} -> {(estadoInicialDefinido ? true : false)}" );

            // Definir estados finales
            string[] estadosFinales = {"q6"};
            bool estadosFinalesDefinidos = evaluador.DefinirEstadosFinales(estadosFinales, out estadosFinales);
            Console.WriteLine($"F - Estados finales definidos: {{{string.Join(", ", estadosFinales)}}} -> {(estadosFinalesDefinidos ? true : false)}");

            // Definir función de transición δ
            Console.WriteLine("\nDefiniendo transiciones (O):\n");

           //Crear transiciones
            bool t1 = evaluador.AgregarTransicion("q5", "q7", 'e');
            bool t2 = evaluador.AgregarTransicion("q7", "q0", 'ε');
            bool t3 = evaluador.AgregarTransicion("q7", "q2", 'ε');
            bool t4 = evaluador.AgregarTransicion("q0", "q1", 'k');
            bool t5 = evaluador.AgregarTransicion("q1", "q6", 'ε');
            bool t6 = evaluador.AgregarTransicion("q2", "q3", 'm');
            bool t7 = evaluador.AgregarTransicion("q2", "q3", 'ε');
            bool t8 = evaluador.AgregarTransicion("q3", "q4", 'h');
            bool t9 = evaluador.AgregarTransicion("q4", "q6", 'ε');
            bool t10 = evaluador.AgregarTransicion("q0", "q0", 'j');

            Console.WriteLine($"  δ(q0, a) = {{q1}} - {(t1 ? true : false)}");
            Console.WriteLine($"  δ(q0, b) = {{q1}} - {(t2 ? true : false)}");
            Console.WriteLine($"  δ(q0, b) = {{q1}} - {(t3 ? true : false)}");
            Console.WriteLine($"  δ(q0, b) = {{q1}} - {(t4 ? true : false)}");
            Console.WriteLine($"  δ(q0, b) = {{q1}} - {(t5 ? true : false)}");
            Console.WriteLine($"  δ(q0, b) = {{q1}} - {(t6 ? true : false)}");
            Console.WriteLine($"  δ(q0, b) = {{q1}} - {(t7 ? true : false)}");
            Console.WriteLine($"  δ(q0, b) = {{q1}} - {(t8 ? true : false)}");
            Console.WriteLine($"  δ(q0, b) = {{q1}} - {(t9 ? true : false)}");
            Console.WriteLine($"  δ(q0, b) = {{q1}} - {(t10 ? true : false)}");

            //verificar quintuplo completo
            Console.WriteLine($"\n---------- Quíntuplo completo: {(evaluador.QuintuploCompleto ? true : false)} ----------");

         
                // Mostrar tabla de transiciones
                Console.WriteLine();
                MostrarTablaDeTransiciones(evaluador);

                // === PRUEBAS CON PALABRAS ===
                Console.WriteLine("=== PRUEBAS DE EVALUACIÓN ===");
                
                // palabras de prueba
                string[] palabrasAceptadas = { "ejjk", " ", "ejjjk", "bhfegfeh" };
                Console.WriteLine("\nResultados de las pruebas:\n");
                foreach (string palabra in palabrasAceptadas)
                {
                    char resultado = evaluador.EvaluarPalabra(palabra);
                    string estado = ObtenerDescripcionResultado(resultado);
                    Console.WriteLine($"  '{palabra}' -> ({estado}) -> {resultado}");
                }
                
                Console.WriteLine("\n=== CÓDIGOS DE RETORNO ===\n");
                Console.WriteLine("  's': Palabra satisface la expresión regular");
                Console.WriteLine("  'n': Palabra no cumple la expresión regular");
                Console.WriteLine("  'p': Palabra contiene símbolos fuera del alfabeto");
                Console.WriteLine("  'e': Error - quíntuplo incompleto");
            

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        private static string ObtenerDescripcionResultado(char codigo)
        {
            return codigo switch
            {
                's' => "Palabra satisface la expresión regular",
                'n' => "Palabra no cumple la expresión regular",
                'p' => "Palabra contiene símbolos fuera del alfabeto",
                'e' => "Error - quíntuplo incompleto",
                _ => "Código desconocido"
            };
        }

        /// <summary>
        /// Muestra la tabla de transiciones del autómata
        /// </summary>
        public static void MostrarTablaDeTransiciones(EvaluadorExpresionRegular evaluador)
        {
            if (!evaluador.QuintuploCompleto)
            {
                return;
            }

            Console.WriteLine("=== TABLA DE TRANSICIONES DEL AUTÓMATA ===\n");
            Console.WriteLine($"-> Estado inicial: {evaluador.estadoInicial}");
            Console.WriteLine($"-> Estados finales: {{{string.Join(", ", evaluador.estadosFinales)}}}");
            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------");
            // Crear encabezado con el alfabeto + epsilon
            var simbolosOrdenados = evaluador.alfabeto.OrderBy(c => c).ToList();
            simbolosOrdenados.Add('ε');
         
            Console.Write("Estado\t");
            foreach (char simbolo in simbolosOrdenados)
            {
                Console.Write($"{simbolo}\t");
            }
            Console.WriteLine();

            // Mostrar transiciones para cada estado
            foreach (Estado estado in evaluador.conjuntoEstados.OrderBy(e => e.Nombre))
            {
                string marcaEstado = estado.Nombre;
                if (estado.Nombre == evaluador.estadoInicial)
                    marcaEstado = ">" + marcaEstado;
                if (estado.EsEstadoFinal)
                    marcaEstado = "*" + marcaEstado;

                Console.Write($"{marcaEstado}\t");

                foreach (char simbolo in simbolosOrdenados)
                {
                    var transicionesDesdeEstado = evaluador.funcionTransicion
                        .Where(t => t.EstadoOrigen == estado.Nombre && t.Simbolo == simbolo)
                        .Select(t => t.EstadoDestino)
                        .ToList();

                    if (transicionesDesdeEstado.Count > 0)
                    {
                        Console.Write($"{{{string.Join(",", transicionesDesdeEstado)}}}\t");
                    }
                    else
                    {
                        Console.Write("∅\t");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine();
        }
    }
}
