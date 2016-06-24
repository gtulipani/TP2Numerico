using System;
using System.IO;

namespace TP2
{
    class Program
    {
        // Constantes del problema.
        static long maximoN = 2097152;
        static float np = 96570;
        static float lambdaOriginal = (np / 90000);
        static float G = 6.673f * Pow(10, -11);
        static float c = (3 * Pow(10, 8));
        static float c2 = Pow(c, 2);

        // Constantes dependientes de lambda
        static float lambda;
        static float m1;
        static float m2;
        static float GM;
        static float e;
        static float a;
        static float h2;
        static float h;
        static float q;

        public struct Ejes {
            private float semiejeMayor;
            private float semiejeMenor;

            public Ejes(int semiejeMayor, int semiejeMenor)
            {
                this.semiejeMayor = semiejeMayor;
                this.semiejeMenor = semiejeMenor;
            }

            public void setSemiejeMayor(float semiejeMayor)
            {
                this.semiejeMayor = semiejeMayor;
            }

            public void setSemiejeMenor(float semiejeMenor)
            {
                this.semiejeMenor = semiejeMenor;
            }

            public float getSemiejeMayor()
            {
                return this.semiejeMayor;
            }

            public float getSemiejeMenor()
            {
                return this.semiejeMenor;
            }

            public void copyFrom(Ejes semiejes)
            {
                this.setSemiejeMayor(semiejes.getSemiejeMayor());
                this.setSemiejeMenor(semiejes.getSemiejeMenor());
            }
        };

        static void Main(string[] args)
        {
            // Inicializo la aplicación y obtengo el nombre del archivo donde va 
            // a quedar todo guardado.
            actualizarConstantesLambda(lambdaOriginal); // Le doy valor a las constantes dependientes de lambda
            var runName = "Files/";
            /*
            var folderName = runName + "/Algoritmo1 Clasico/";
            actualizarConstantesLambda(lambdaOriginal); // Le doy valor a las constantes dependientes de lambda
            var fileNameArea = InitializeAreaFile(folderName);
            var fileNamePrimeraLey = InitializePrimeraLeyFile(folderName);
            var fileNameSegundaLey = InitializeSegundaLeyFile(folderName);
            var fileNameTerceraLey = InitializeTerceraLeyFile(folderName);
            var fileNameEnergia = InitializeEnergyFile(folderName);
            Console.Write("Comenzando con Algoritmo1 Clasico...\n");
            var start = DateTime.Now;
            CorrerAlgoritmoNVeces(fileNameArea, fileNamePrimeraLey, fileNameSegundaLey, fileNameTerceraLey, fileNameEnergia);
            var end = DateTime.Now;
            var span = end - start;
            Console.Write("Finalizado Algoritmo 1 Clasico en " + ((int)span.TotalMilliseconds).ToString() + " milisegundos\n");
            // Cambio la carpeta para la segunda corrida.
            */
            var folderName3 = runName + "/Algoritmo1 Relativista/";
            actualizarConstantesLambda(1); // Modifico las constantes dependientes de lambda para convertir el sistema a Mercurio y el Sol
            var fileNameEnergiaRelativista = InitializeEnergyFile(folderName3);
            Console.Write("Comenzando con Algoritmo1 Relativista...\n");
            var start3 = DateTime.Now;
            CorrerAlgoritmoNVeces_Relativista(fileNameEnergiaRelativista);
            var end3 = DateTime.Now;
            var span3 = end3 - start3;
            Console.Write("Finalizado Algoritmo 1 Relativista en " + ((int)span3.TotalMilliseconds).ToString() + " milisegundos\n");  
            Console.Read();
        }

        private static void actualizarConstantesLambda(float valorLambda)
        {
            lambda = valorLambda;
            m1 = lambda * 1.9891f * Pow(10, 30);
            m2 = lambda * 3.301f * Pow(10, 23);
            GM = G * (m1 + m2);
            e = 0.2056f / lambda;
            a = (Pow(lambda, 2)) * 5.791f * Pow(10, 10);
            h2 = a * GM * (1f - Pow(e, 2));
            h = Sqrt(h2);
            q = a * (1 - e);
        }

        #region Inicializadores

        private static string InitializeAreaFile(string folderName)
        {
            // Me aseguro de que exista una carpeta dónde dejar los archivos.
            var folder = folderName + "/Area/";
            var directoryInfo = new DirectoryInfo(folder);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            // Blanqueo el archivo de resultados, y le pongo las cabeceras a las columnas.
            var fileName = folder + "Area.csv";
            File.AppendAllText(fileName, string.Format("{0};{1};{2}{3}",
                    "h",
                    "A(O2) [m2]",
                    "A(O4) [m2]",
                    Environment.NewLine));
            return fileName;
        }

        private static string InitializePrimeraLeyFile(string folderName)
        {
            // Me aseguro de que exista una carpeta dónde dejar los archivos.
            var folder = folderName + "/Primera Ley/";
            var directoryInfo = new DirectoryInfo(folder);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            // Blanqueo el archivo de resultados, y le pongo las cabeceras a las columnas.
            var fileName = folder + "Primera Ley.csv";
            File.AppendAllText(fileName, string.Format("{0};{1};{2};{3};{4}{5}",
                    "N",
                    "An [m]",
                    "E(An) [m]",
                    "Bn [m]",
                    "E(Bn) [m]",
                    Environment.NewLine));
            return fileName;
        }

        private static string InitializeSegundaLeyFile(string folderName)
        {
            // Me aseguro de que exista una carpeta dónde dejar los archivos.
            var folder = folderName + "/Segunda Ley/";
            var directoryInfo = new DirectoryInfo(folder);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            // Blanqueo el archivo de resultados, y le pongo las cabeceras a las columnas.
            var fileName = folder + "Segunda Ley.csv";
            File.AppendAllText(fileName, string.Format("{0};{1};{2}{3}",
                    "N",
                    "T [s]",
                    "E(T) [s]",
                    Environment.NewLine));
            return fileName;
        }

        private static string InitializeTerceraLeyFile(string folderName)
        {
            // Me aseguro de que exista una carpeta dónde dejar los archivos.
            var folder = folderName + "/Tercera Ley/";
            var directoryInfo = new DirectoryInfo(folder);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            // Blanqueo el archivo de resultados, y le pongo las cabeceras a las columnas.
            var fileName = folder + "Tercera Ley.csv";
            File.AppendAllText(fileName, string.Format("{0};{1};{2};{3};{4}{5}",
                    "N",
                    "Tn2 [s2]",
                    "Rn3 [m3]",
                    "Tn2 / Rn3 [s2/m3]",
                    "E(Tn2 / Rn3) [s2/m3]",
                    Environment.NewLine));
            return fileName;
        }

        private static string InitializeEnergyFile(string folderName)
        {
            // Me aseguro de que exista una carpeta dónde dejar los archivos.
            var folder = folderName + "/Conservacion de la Energia/";
            var directoryInfo = new DirectoryInfo(folder);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            // Blanqueo el archivo de resultados, y le pongo las cabeceras a las columnas.
            var fileName = folder + "Conservacion de la Energia.csv";
            File.AppendAllText(fileName, string.Format("{0};{1};{2}{3}",
                    "n",
                    "un [1/m]",
                    "En [J]",
                    Environment.NewLine));
            return fileName;
        }

        #endregion

        #region EjecucionAlgoritmos

        private static void CorrerAlgoritmoNVeces(string fileNameArea, string fileNamePrimeraLey, string fileNameSegundaLey, string fileNameTerceraLey, string fileNameEnergia)
        {
            #region VariablesDelCiclo
            float areaAnterior = 0;
            float periodoAnterior = 0;
            float resultadoTerceraLeyAnterior = 0;
            Ejes semiejesAnterior = new Ejes(0, 0);
            float kAnterior = 0;
            #endregion
            bool calcularEnergia = false;

            for (long N = 8; N <= maximoN && N > 0; N *= 2)
            {
                Console.Write("Procesando para N = " + N.ToString() + "\n");
                float area;
                float periodo;
                float resultadoTerceraLey;
                Ejes semiejes = new Ejes(0, 0);
                float k = (2f * Pi()) / N; // Calculo el intervalo del ángulo (el paso de discretización)
                Algoritmo_Clasico(k, N, out area, ref semiejes, calcularEnergia, fileNameEnergia); // Corro el algoritmo y obtengo un resultado con (O2)
                realizarOperacionesPrimeraLey(fileNamePrimeraLey, N, semiejesAnterior, semiejes); // A.1
                area = realizarOperacionesArea(fileNameArea, N, areaAnterior, area, kAnterior, k); // A.2
                periodo = realizarOperacionesSegundaLey(fileNameSegundaLey, N, periodoAnterior, area); // A.2
                resultadoTerceraLey = realizarOperacionesTerceraLey(fileNameTerceraLey, N, periodo, semiejes, resultadoTerceraLeyAnterior); // A.3

                areaAnterior = area;
                periodoAnterior = periodo;
                resultadoTerceraLeyAnterior = resultadoTerceraLey;
                semiejesAnterior.copyFrom(semiejes);
                kAnterior = k;

                Console.Write("PROCESADO\n");
            }
        }

        private static void CorrerAlgoritmoNVeces_Relativista(string fileNameEnergia)
        {
            long N = 16777216;
            Console.Write("Analizando la precesión de la órbita para N = " + N.ToString() + "\n");
            double k = (2f * Pi()) / N; // Calculo el intervalo del ángulo (el paso de discretización)
            Algoritmo_Relativista(k, N, fileNameEnergia);
            Console.Write("PROCESADO\n");
        }

        static void Algoritmo_Clasico(float k, long N, out float area, ref Ejes ejes, bool calculoEnergia, string fileNameEnergia)
        {
            #region VariablesMetodoDiferencial
            float uActual = 1 / (a * (1 - e));
            float uProximo = 0;
            float v0 = 0;
            float vN = 0;
            #endregion

            float uAnterior = uActual;
            float areaParcial = calcularArea((1 / uActual));
            for (long n = 0; n <= N - 1; n++)
            {
                metodoEuler(uActual, v0, ref uProximo, ref vN, k);
                #region CalculosSemiejes
                if (n == (N / 4 - 1)) // Me encuentro aproximadamente a un cuarto de la orbita, puedo calcular Bn
                {
                    float hipotenusa = (1 / uProximo);
                    float catetoMayor = (a * e); // Utilizo la distancia del centro al cuerpo estatico
                    ejes.setSemiejeMenor(catetoPitagoras(hipotenusa, catetoMayor));
                }
                if (n == (N / 2 - 1)) // Me encuentro a la mitad de la orbita, puedo calcular An
                {
                    ejes.setSemiejeMayor(Abs((1 / uProximo) - (a * e)));
                }
                #endregion
                #region CalculosArea
                float fN = calcularArea((1f / uProximo)); //Calculo el area para cada rN
                int factorTrapecio = 2;
                if (n == (N - 1)) //Para n inicial y final se suma una sola vez
                {
                    factorTrapecio = 1;
                }
                areaParcial += (fN * factorTrapecio);
                #endregion
                #region CalculosEnergia
                if (calculoEnergia)
                {
                    float energiaParcial = realizarOperacionesEnergia(N, n, uAnterior, uActual, uProximo, k, fileNameEnergia); // A.4
                    uAnterior = uActual; // Me guardo el valor anterior para usarlo para el calculo de la derivada
                }
                #endregion 
                uActual = uProximo;
                v0 = vN;
            }
            areaParcial *= (0.5f * k);
            area = areaParcial;
        }

        static void Algoritmo_Relativista(double k, long N, string fileNameEnergia)
        {
            #region VariablesMetodoDiferencial
            double uActual = 1 / (a * (1 - e));
            double uProximo = 0;
            double v0 = 0;
            double vN = 0;
            #endregion

            double uAnterior = uActual;
            double uAnteultimo = 0, uFinal; // Variables que se utilizaran para la Interpolacion
            for (long n = 0; n <= N - 1; n++)
            {
                metodoEulerRelativista(uActual, v0, ref uProximo, ref vN, k);
                /*
                #region CalculoEnergia
                double energiaParcial = realizarOperacionesEnergia(N, n, uAnterior, uActual, uProximo, k, fileNameEnergia);
                uAnterior = uActual; // Me guardo el valor anterior para usarlo para el calculo de la derivada
                #endregion
                */
                if (n == N - 1)
                {
                    uAnteultimo = uActual;
                }
                uActual = uProximo;
                v0 = vN;
            }
          uFinal = uActual;
            #region CalculoPrecesion
            double uExtra = uFinal;
            int cantidadRepeticiones = 0;
            metodoEulerRelativista(uFinal, v0, ref uExtra, ref vN, k); // Obtengo un valor más de la órbita
            v0 = vN;
            while (uExtra >= uFinal) 
            {
                // Sigo obteniendo valores de la órbita hasta conseguir uno uN vuelva a disminuir (rN vuelve a aumentar)
                // Voy corriendo los dos que me voy guardando así me queda una terna consecutiva
                uAnteultimo = uFinal;
                uFinal = uExtra;
                metodoEulerRelativista(uFinal, v0, ref uExtra, ref vN, k); // Obtengo un valor más de la órbita
                v0 = vN;
                cantidadRepeticiones++;
            }


            double rAnteultimo = 1 / uAnteultimo;
            double rFinal = 1 / uFinal;
            double rExtra = 1 / uExtra;
            Console.WriteLine("Los puntos utilizados para la interpolacion son: rAnteultimo = " + rAnteultimo.ToString() + ", rFinal = " + rFinal.ToString() + " y rExtra = " + rExtra.ToString());
            // Para realizar la interpolacion, posiciono los puntos con el ángulo de la órbita en las abscisas y su distancia al Foco en las ordenadas
            // El angulo alpha correspondiente a rAnteultimo es (N-1+cantidadRepeticiones)
            // El angulo alpha correspondiente a rFinal es (N+cantidadRepeticiones)
            // El angulo alpha correspondiente a rFinal es (N+1+cantidadRepeticiones)
            double posicionAnteultimo = k*(-1 + cantidadRepeticiones);
            double posicionFinal = k*(cantidadRepeticiones);
            double posicionExtra = k*(1 + cantidadRepeticiones);
            Console.WriteLine("Para realizar la interpolacion se ubican los puntos de la siguiente forma:");
            Console.WriteLine("- rAnteultimo en " + posicionAnteultimo.ToString());
            Console.WriteLine("- rFinal en " + posicionFinal.ToString());
            Console.WriteLine("- rExtra en " + posicionExtra.ToString());
            double C0, C1, C2;
            interpolacionNewton(posicionAnteultimo, rAnteultimo, posicionFinal, rFinal, posicionExtra, rExtra, out C0, out C1, out C2); // Interpolo y obtengo los coeficientes del polinomio
            Console.WriteLine("C0 = " + C0.ToString());
            Console.WriteLine("C1 = " + C1.ToString());
            Console.WriteLine("C2 = " + C2.ToString());
            // El polinomio interpolador es de la forma f(x) = C0 + C1(x - x0) + C2(x - x0)(x - x1)
            // Si ubicamos rAnteultimo en posicionAnteultimo y sucesivamente rFinal y rExtra, x0 = posicionAnteultimo, x1 = posicionFinal y x2 = posicionExtra
            // f(x) = C0 + C1 (x - posicionAnteultimo) + C2(x - posicionAnteultimo)(x - posicionFinal)
            // f(x) = C0 + C1 (x - posicionAnteultimo) + C2(x^2 - x*posicionFinal - x*posicionAnteultimo + posicionAnteultimo*posicionFinal)
            // f(x) = x^2 (C2) + x (C1 - C2*(posicionFinal + posicionAnteultimo)) + (C0 - C1*posicionAnteultimo + C2*posicionAnteultimo*posicionFinal)
            // f'(x) = 2*x*C2 + C1 - C2*(posicionFinal + posicionAnteultimo)
            // f'(x) = 0 <==> 2*x*C2 + C1 - C2*(posicionFinal + posicionAnteultimo) = 0 <==> x = (C2*(posicionFinal + posicionAnteultimo) - C1) / (2*C2)
            double minimo = (((C2 * (posicionFinal + posicionAnteultimo)) - C1) / (2d * C2));
            Console.WriteLine("El minimo calculado es " + minimo.ToString());
            double valorMinimo = (C0 + (C1 * (minimo - posicionAnteultimo)) + (C2 * (minimo - posicionAnteultimo) * (minimo - posicionFinal)));
            Console.WriteLine("El valor de la funcion en el minimo es " + valorMinimo.ToString());
            // La precesión coincide con la diferencia angular de dos perihelios consecutivos, encontrandose el perihelio anterior en el angulo: theta = k*N = 2pi
            double precesion = (minimo);
            Console.WriteLine("El valor calculado de la precesion es " + precesion.ToString());
            double precesionSegundosDeArco = precesion / Pi() * 3600d * 180d * 365.25d * 100d / 87.97d;
            Console.WriteLine("El valor calculado de la precesion en segundos de arco por siglo terrestre es " + precesionSegundosDeArco.ToString());
            #endregion
        }

        #endregion

        #region LeyesDeKepler

        private static float calcularArea(float r)
        {
            return (0.5f * (Pow(r, 2)));
        }

        private static float calcularPeriodo(float area)
        {
            return ((2f * area) / h);
        }

        private static float realizarOperacionesArea(string fileName, long N, float areaAnterior, float areaCalculada, float k_anterior, float k)
        {
            float areaExtrapolada;
            if (areaAnterior == 0)
            {
                areaExtrapolada = areaCalculada; // Si es el primer resultado, no lo extrapolo
            }
            else
            {
                // Aplico la Extrapolacion de Richardson y obtengo un resultado con (O4)
                areaExtrapolada = extrapolacionRichardson(areaAnterior, areaCalculada, k_anterior, k, 2f);
            }

            // Armo el mensaje para mostrar para guardar en la tabla.
            var messageArea = string.Format("{0};{1};{2}{3}",
                "2pi/" + N.ToString(),
                areaCalculada.ToString(),
                areaExtrapolada.ToString(),
                Environment.NewLine);
            File.AppendAllText(fileName, messageArea); // Lo agrego al archivo.
            return areaExtrapolada;
        }

        private static void realizarOperacionesPrimeraLey(string fileNamePrimeraLey, long N, Ejes semiejesAnterior, Ejes semiejes)
        {
            float semiejeMayorCalculado = semiejes.getSemiejeMayor();
            float semiejeMenorCalculado = semiejes.getSemiejeMenor();
            float deltaSemiejeMayor = (Abs(semiejeMayorCalculado - semiejesAnterior.getSemiejeMayor()));
            float deltaSemiejeMenor = (Abs(semiejeMenorCalculado - semiejesAnterior.getSemiejeMenor()));

            var messagePrimeraLey = string.Format("{0};{1};{2};{3};{4}{5}", N,
                semiejeMayorCalculado.ToString(),
                deltaSemiejeMayor.ToString(),
                semiejeMenorCalculado.ToString(),
                deltaSemiejeMenor.ToString(),
                Environment.NewLine);
            File.AppendAllText(fileNamePrimeraLey, messagePrimeraLey); //Lo agrego al archivo.
        }

        private static float realizarOperacionesSegundaLey(string fileNameSegundaLey, long N, float periodoAnterior, float areaCalculada)
        {
            float periodoCalculado = calcularPeriodo(areaCalculada);

            float deltaPeriodo = Abs(periodoCalculado - periodoAnterior);

            var messageSegundaLey = string.Format("{0};{1};{2}{3}", N,
                periodoCalculado.ToString(),
                deltaPeriodo.ToString(),
                Environment.NewLine);
            File.AppendAllText(fileNameSegundaLey, messageSegundaLey); // Lo agrego al archivo.
            return periodoCalculado;
        }

        private static float realizarOperacionesTerceraLey(string fileNameTerceraLey, long N, float periodo, Ejes semiejes, float resultadoTerceraLeyAnterior)
        {
            // Resultados
            float periodoCuadrado = Pow(periodo, 2);
            float semiejeMayorCubico = Pow(semiejes.getSemiejeMayor(), 3);
            float resultadoTerceraLey = (periodoCuadrado / semiejeMayorCubico);

            // Error de la constante
            float deltaResultadoTerceraLey = Abs(resultadoTerceraLey - resultadoTerceraLeyAnterior);

            var messageTerceraLey = string.Format("{0};{1};{2};{3};{4}{5}", N,
                periodoCuadrado.ToString(),
                semiejeMayorCubico.ToString(),
                resultadoTerceraLey.ToString(),
                deltaResultadoTerceraLey.ToString(),
                Environment.NewLine);
            File.AppendAllText(fileNameTerceraLey, messageTerceraLey); // Lo agrego al archivo
            return resultadoTerceraLey;
        }

        #endregion

        #region Energia

        private static float calcularEnergia(float uN, float derivada)
        {
            float velocidadCuadrada = (h2 * (Pow(uN, 2) + Pow(derivada, 2)));
            return ((0.5f * velocidadCuadrada) - (GM * uN));
        }

        private static double calcularEnergia(double uN, double derivada)
        {
            double velocidadCuadrada = (h2 * (Pow(uN, 2) + Pow(derivada, 2)));
            return ((0.5f * velocidadCuadrada) - (GM * uN));
        }

        private static float realizarOperacionesEnergia(long N, long n, float uAnterior, float uActual, float uProximo, float k, string fileNameEnergia)
        {
            float energiaParcial;
            float energiaTotal = 0;
            if (n == 0)
            {
                energiaParcial = calcularEnergia(uActual, calcularDerivadaEnAdelanto(uActual, uProximo, k));
            }
            else energiaParcial = calcularEnergia(uActual, calcularDerivadaCentrada(uAnterior, uProximo, k));
            escribirResultadoEnergiaEnArchivo(n, uActual, energiaParcial, fileNameEnergia);
            energiaTotal += energiaParcial;
            if (n == N - 1)
            {
                energiaParcial = calcularEnergia(uProximo, calcularDerivadaEnAtraso(uActual, uProximo, k));
                escribirResultadoEnergiaEnArchivo(n + 1, uProximo, energiaParcial, fileNameEnergia);
                energiaTotal += energiaParcial;
            }
            return energiaTotal;
        }

        private static double realizarOperacionesEnergia(long N, long n, double uAnterior, double uActual, double uProximo, double k, string fileNameEnergia)
        {
            double energiaParcial;
            double energiaTotal = 0;
            if (n == 0)
            {
                energiaParcial = calcularEnergia(uActual, calcularDerivadaEnAdelanto(uActual, uProximo, k));
            }
            else energiaParcial = calcularEnergia(uActual, calcularDerivadaCentrada(uAnterior, uProximo, k));
            escribirResultadoEnergiaEnArchivo(n, uActual, energiaParcial, fileNameEnergia);
            energiaTotal += energiaParcial;
            if (n == N - 1)
            {
                energiaParcial = calcularEnergia(uProximo, calcularDerivadaEnAtraso(uActual, uProximo, k));
                escribirResultadoEnergiaEnArchivo(n + 1, uProximo, energiaParcial, fileNameEnergia);
                energiaTotal += energiaParcial;
            }
            return energiaTotal;
        }

        private static void escribirResultadoEnergiaEnArchivo(long n, float uActual, float energiaParcial, string fileNameEnergia)
        {
            var messageEnergia = string.Format("{0};{1};{2}{3}", n,
               uActual.ToString(),
               energiaParcial.ToString(),
               Environment.NewLine);
            File.AppendAllText(fileNameEnergia, messageEnergia); // Lo agrego al archivo.
        }

        private static void escribirResultadoEnergiaEnArchivo(long n, double uActual, double energiaParcial, string fileNameEnergia)
        {
            var messageEnergia = string.Format("{0};{1};{2}{3}", n,
               uActual.ToString(),
               energiaParcial.ToString(),
               Environment.NewLine);
            File.AppendAllText(fileNameEnergia, messageEnergia); // Lo agrego al archivo.
        }

        #endregion

        #region Operaciones Matematicas

        static float Sin(double p)
        {
            return (float)Math.Sin(p);
        }

        static float Cos(double p)
        {
            return (float)Math.Cos(p);
        }

        static float Tan(double p)
        {
            return (float)Math.Tan(p);
        }

        static float Atan(double p)
        {
            return (float)Math.Atan(p);
        }

        static float Pow(float number, double pow)
        {
            return (float)Math.Pow(number, pow);
        }

        static double Pow(double number, double pow)
        {
            return Math.Pow(number, pow);
        }

        static float Sqrt(double n)
        {
            return (float)Math.Sqrt(n);
        }

        static float Abs(double n)
        {
            return (float)Math.Abs(n);
        }

        static float Pi()
        {
            return (float)Math.PI;
        }

        static float catetoPitagoras(float hipotenusa, float cateto)
        {
            return Sqrt(Pow(hipotenusa, 2) - Pow(cateto, 2));
        }

        private static float extrapolacionRichardson(float T1, float T2, float h1, float h2, float p)
        {
            float q = (h1 / h2);
            return (T2 + ((T2 - T1) / ((Pow(q, p) - 1))));
        }

        private static void interpolacionNewton(float x0, float f0, float x1, float f1, float x2, float f2, out float C0, out float C1, out float C2)
        {
            C0 = f0; // f[x0]
            C1 = diferenciasDivididas(f0, f1, (x0 - x1)); // f[x0,x1]
            float f1_f2 = diferenciasDivididas(f1, f2, (x1 - x2)); // f[x1,x2]
            C2 = diferenciasDivididas(C1, f1_f2, (x0 - x2)); // f[x0,x1,x2]
        }

        private static void interpolacionNewton(double x0, double f0, double x1, double f1, double x2, double f2, out double C0, out double C1, out double C2)
        {
            C0 = f0; // f[x0]
            C1 = diferenciasDivididas(f0, f1, (x0 - x1)); // f[x0,x1]
            double f1_f2 = diferenciasDivididas(f1, f2, (x1 - x2)); // f[x1,x2]
            C2 = diferenciasDivididas(C1, f1_f2, (x0 - x2)); // f[x0,x1,x2]
        }

        private static float diferenciasDivididas(float fi, float fn, float delta)
        {
            return ((fi - fn) / delta);
        }

        private static double diferenciasDivididas(double fi, double fn, double delta)
        {
            return ((fi - fn) / delta);
        }

        #endregion

        #region Diferenciacion

        private static float calcularDerivadaEnAtraso(float funcionEnAtraso, float funcion, float h)
        {
            return ((funcion - funcionEnAtraso) / h);
        }

        private static double calcularDerivadaEnAtraso(double funcionEnAtraso, double funcion, double h)
        {
            return ((funcion - funcionEnAtraso) / h);
        }

        private static float calcularDerivadaEnAdelanto(float funcion, float funcionEnAdelanto, float h)
        {
            return ((funcionEnAdelanto - funcion) / h);
        }

        private static double calcularDerivadaEnAdelanto(double funcion, double funcionEnAdelanto, double h)
        {
            return ((funcionEnAdelanto - funcion) / h);
        }

        private static float calcularDerivadaCentrada(float funcionEnAtraso, float funcionEnAdelanto, float h)
        {
            return ((funcionEnAdelanto - funcionEnAtraso) / (2f * h));
        }

        private static double calcularDerivadaCentrada(double funcionEnAtraso, double funcionEnAdelanto, double h)
        {
            return ((funcionEnAdelanto - funcionEnAtraso) / (2f * h));
        }

        private static void metodoEuler(float u0, float v0, ref float uN, ref float vN, float k)
        {
            uN = u0 + (k * v0);
            vN = v0 - (k * u0) + (k * (GM / h2));
        }

        private static void metodoEulerRelativista(float u0, float v0, ref float uN, ref float vN, float k)
        {
            uN = u0 + (k * v0);
            vN = v0 - (k * u0) + (k * (GM / h2)) + (k * 3f * GM * Pow(u0, 2) / c2);
        }

        private static void metodoEulerRelativista(double u0, double v0, ref double uN, ref double vN, double k)
        {
            uN = u0 + (k * v0);
            vN = v0 - (k * u0) + (k * (GM / h2)) + (k * 3f * GM * Pow(u0, 2) / c2);
        }

        private static void rungeKuttaOrden4(float u0, float v0, ref float uN, ref float vN, float k)
        {
            float w1 = u0 + k * v0 / 2f;
            float z1 = v0 + k * ((GM / h2) - u0) / 2f;
            float w2 = u0 + k * z1 / 2;
            float z2 = v0 + k * ((GM / h2) - w1) / 2f;
            float w3 = u0 + k * z2;
            float z3 = v0 + k * ((GM / h2) - w2);

            uN = u0 + k * (v0 + 2 * z1 + 2 * z2 + z3) / 6;
            vN = v0 + k * (6 * (GM / h2) - u0 - 2 * w1 - 2 * w2 - w3) / 6;
        }

        private static void rungeKuttaOrden4Relativista(float u0, float v0, ref float uN, ref float vN, float k)
        {
            float w1 = u0 + k * v0 / 2f;
            float z1 = v0 + k * ((GM / h2) - u0 - (3f * GM * Pow(u0, 2) / c2)) / 2f;
            float w2 = u0 + k * z1 / 2;
            float z2 = v0 + k * ((GM / h2) - w1 - (3f * GM * Pow(w1, 2) / c2)) / 2f;
            float w3 = u0 + k * z2;
            float z3 = v0 + k * ((GM / h2) - w2 - (3f * GM * Pow(w2, 2) / c2));

            uN = u0 + k * (v0 + 2 * z1 + 2 * z2 + z3) / 6;
            vN = v0 + k * (6 * (GM / h2) - u0 - 2 * w1 - 2 * w2 - w3) / 6;
        }

        #endregion

    }
}