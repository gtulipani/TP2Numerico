using System;
using System.IO;

namespace TP2
{
    class Program
    {
        // Declaro las constantes del problema.
        static long maximoN = (long)(Pow(2, 15));//int.MaxValue;
        static float np = 96570;
        static float lambda = np / 90000;
        static float m1 = lambda * 1.9891f * Pow(10, 30);
        static float m2 = lambda * 3.301f * Pow(10, 23);
        static float e = 0.2056f / lambda;
        static float a = (Pow(lambda, 2)) * 5.791f * Pow(10, 7);
        static float G = 6.673f * Pow(10, -11);
        static float GM = G * (m1 + m2);
        static float h2 = a * GM * (1f - Pow(e, 2));
        static float h = Sqrt(h2);
        static float q = a * (1 - e);
        static float c = (3 * Pow(10, 8));
        static float c2 = Pow(c, 2);

        static void Main(string[] args)
        {
            // Inicializo la aplicación y obtengo el nombre del archivo donde va 
            // a quedar todo guardado.
            var runName = "Files/";
            /*var folderName = runName + "/Algoritmo1 Clasico/";
            var fileNameArea = InitializeAreaFile(folderName);
            var fileNamePeriodo = InitializePeriodFile(folderName);
            var fileNameEnergia = InitializeEnergyFile(folderName);
            Console.Write("Comenzando con Algoritmo1 Clasico...\n");
            var start = DateTime.Now;
            CorrerAlgoritmoNVeces(fileNameArea, fileNamePeriodo, fileNameEnergia);
            var end = DateTime.Now;
            var span = end - start;
            Console.Write("Finalizado Algoritmo 1 Clasico en " + ((int)span.TotalMilliseconds).ToString() + " milisegundos\n");
            */
            // Cambio la carpeta para la segunda corrida.
            var folderName = runName + "/Algoritmo1 Relativista/";
            var fileNameArea = InitializeAreaFile(folderName);
            var fileNamePeriodo = InitializePeriodFile(folderName);
            var fileNameEnergia = InitializeEnergyFile(folderName);
            Console.Write("Comenzando con Algoritmo1 Relativista...\n");
            var start2 = DateTime.Now;
            CorrerAlgoritmoNVeces(fileNameArea, fileNamePeriodo, fileNameEnergia);
            var end2 = DateTime.Now;
            var span2 = end2 - start2;
            Console.Write("Finalizado Algoritmo 1 Relativista en " + ((int)span2.TotalMilliseconds).ToString() + " milisegundos\n");       
            Console.Read();
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
            var fileName = folder + "results.csv";
            File.AppendAllText(fileName, string.Format("{0};{1};{2}{3}",
                    "h",
                    "A(O2)",
                    "A(O4)",
                    Environment.NewLine));
            return fileName;
        }

        private static string InitializePeriodFile(string folderName)
        {
            // Me aseguro de que exista una carpeta dónde dejar los archivos.
            var folder = folderName + "/Periodo/";
            var directoryInfo = new DirectoryInfo(folder);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            // Blanqueo el archivo de resultados, y le pongo las cabeceras a las columnas.
            var fileName = folder + "results.csv";
            File.AppendAllText(fileName, string.Format("{0};{1};{2}{3}",
                    "N",
                    "T",
                    "E(T)",
                    Environment.NewLine));
            return fileName;
        }

        private static string InitializeEnergyFile(string folderName)
        {
            // Me aseguro de que exista una carpeta dónde dejar los archivos.
            var folder = folderName + "/Energia/";
            var directoryInfo = new DirectoryInfo(folder);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            // Blanqueo el archivo de resultados, y le pongo las cabeceras a las columnas.
            var fileName = folder + "results.csv";
            File.AppendAllText(fileName, string.Format("{0};{1};{2}{3}",
                    "n",
                    "un",
                    "En",
                    Environment.NewLine));
            return fileName;
        }

        #endregion

        #region Float

        private static void CorrerAlgoritmoNVeces(string fileNameArea, string fileNamePeriodo, string fileNameEnergia)
        {
            // A2:
            float[] area = new float[200];
            float[] periodo = new float[200];
            area[0] = 0;
            periodo[0] = 0;
            float kAnterior = 0;
            int index = 1;
            bool calcularEnergia = false;

            for (long N = 4; N <= maximoN && N > 0; N *= 2)
            {
                Console.Write("Procesando para N = " + N.ToString() + "\n");
                float k = (2f * (float)Math.PI) / N; // Calculo el intervalo del ángulo (el paso de discretización)
                if (N >= (maximoN / 8)) // Calculo energia para los 3 ultimos N
                    calcularEnergia = true;
                Algoritmo_Clasico(k, N, ref area[index], calcularEnergia, fileNameEnergia); // Corro el algoritmo y obtengo un resultado con (O2)
                area[index] = realizarOperacionesArea(fileNameArea, N, area[index - 1], area[index], kAnterior, k);
                periodo[index] = realizarOperacionesPeriodo(fileNamePeriodo, N, periodo[index - 1], area[index]);
                kAnterior = k;
                index++;
                Console.Write("PROCESADO\n");
            }
        }

        static void Algoritmo_Clasico(float k, long N, ref float area, bool calculoEnergia, string fileNameEnergia)
        {
            #region VariablesMetodoDiferencial
            float uActual = 1 / (a * (1 - e));
            float uProximo = 0;
            float v0 = 0;
            float vN = 0;
            #endregion

            float uAnterior = uActual;
            float energiaTotal = 0;
            float areaParcial = calcularArea((1 / uActual));
            for (long n = 0; n <= N - 1; n++)
            {
                metodoEulerRelativista(uActual, v0, ref uProximo, ref vN, k);
                #region CalculosArea
                float fN = calcularArea((1f/ uProximo)); //Calculo el area para cada rN
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
                    float energiaParcial = realizarOperacionesEnergia(N, n, uAnterior, uActual, uProximo, k, fileNameEnergia);
                    uAnterior = uActual; // Me guardo el valor anterior para usarlo para el calculo de la derivada
                }
                #endregion 
                uActual = uProximo;
                v0 = vN;
            }

            areaParcial *= (0.5f * k);
            area = areaParcial;
        }

        static float Algoritmo2_Clasico(float k, long N)
        {
            float u0 = 1 / (a * (1 - e));
            float v0 = 0;

            float uN = 0;
            float vN = 0;

            // Corro el algoritmo.
            for (long n = 0; n < N - 1; n++)
            {
                float w1 = u0 + k * v0 / 2f;
                float z1 = v0 + k * ((GM / h2) - u0) / 2f;
                float w2 = u0 + k * z1 / 2;
                float z2 = v0 + k * ((GM / h2) - w1) / 2f;
                float w3 = u0 + k * z2;
                float z3 = v0 + k * ((GM / h2) - w2);

                uN = u0 + k * (v0 + 2 * z1 + 2 * z2 + z3) / 6;
                vN = v0 + k * (6 * (GM / h2) - u0 - 2 * w1 - 2 * w2 - w3) / 6;

                u0 = uN;
                v0 = vN;
            }

            return uN;
        }

        #endregion

        #region Double

        // B1
        static double Algoritmo1D_TP1(double k, long N)
        {
            double u0 = 1 / (a * (1 - e));
            double v0 = 0;

            double uN = 0;
            double vN = 0;

            // Corro el algoritmo.
            for (long n = 0; n < N - 1; n++)
            {
                uN = u0 + k * v0;
                vN = v0 - k * u0 + k * (GM / h2);

                u0 = uN;
                v0 = vN;
            }

            return uN;
        }

        // C1
        static double Algoritmo2D_TP1(double k, long N)
        {
            double u0 = 1 / (a * (1 - e));
            double v0 = 0;

            double uN = 0;
            double vN = 0;

            // Corro el algoritmo.
            for (long n = 0; n < N - 1; n++)
            {
                double w1 = u0 + k * v0 / 2f;
                double z1 = v0 + k * ((GM / h2) - u0) / 2f;
                double w2 = u0 + k * z1 / 2;
                double z2 = v0 + k * ((GM / h2) - w1) / 2f;
                double w3 = u0 + k * z2;
                double z3 = v0 + k * ((GM / h2) - w2);

                uN = u0 + k * (v0 + 2 * z1 + 2 * z2 + z3) / 6;
                vN = v0 + k * (6 * (GM / h2) - u0 - 2 * w1 - 2 * w2 - w3) / 6;

                u0 = uN;
                v0 = vN;
            }

            return uN;
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

        private static float realizarOperacionesPeriodo(string fileNamePeriodo, long N, float periodoAnterior, float areaCalculada)
        {
            float periodoCalculado = calcularPeriodo(areaCalculada);

            float deltaPeriodo = Math.Abs(periodoCalculado - periodoAnterior);

            var messagePeriodo = string.Format("{0};{1};{2}{3}", N,
                periodoCalculado.ToString(),
                deltaPeriodo.ToString(),
                Environment.NewLine);
            File.AppendAllText(fileNamePeriodo, messagePeriodo); // Lo agrego al archivo.
            return periodoCalculado;
        }

        #endregion

        #region Energia

        private static float calcularEnergia(float uN, float derivada)
        {
            float velocidadCuadrada = (h2 * (Pow(uN, 2) + Pow(derivada, 2)));
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
            escribirResultadoEnArchivo(n, uActual, energiaParcial, fileNameEnergia);
            energiaTotal += energiaParcial;
            if (n == N - 1)
            {
                energiaParcial = calcularEnergia(uProximo, calcularDerivadaEnAtraso(uActual, uProximo, k));
                escribirResultadoEnArchivo(n + 1, uProximo, energiaParcial, fileNameEnergia);
                energiaTotal += energiaParcial;
            }
            return energiaTotal;
        }

        private static void escribirResultadoEnArchivo(long n, float uActual, float energiaParcial, string fileNameEnergia)
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

        static float Pow(float number, double pow)
        {
            return (float)Math.Pow(number, pow);
        }

        static float Sqrt(double n)
        {
            return (float)Math.Sqrt(n);
        }

        private static float extrapolacionRichardson(float T1, float T2, float h1, float h2, float p)
        {
            float q = (h1 / h2);
            return (T2 + ((T2 - T1) / ((Pow(q, p) - 1))));
        }

        #endregion

        #region Diferenciacion

        private static float calcularDerivadaEnAtraso(float funcionEnAtraso, float funcion, float h)
        {
            return ((funcion - funcionEnAtraso) / h);
        }

        private static float calcularDerivadaEnAdelanto(float funcion, float funcionEnAdelanto, float h)
        {
            return ((funcionEnAdelanto - funcion) / h);
        }

        private static float calcularDerivadaCentrada(float funcionEnAtraso, float funcionEnAdelanto, float h)
        {
            return ((funcionEnAdelanto - funcionEnAtraso) / (2f * h));
        }

        private static bool metodoEuler(float u0, float v0, ref float uN, ref float vN, float k)
        {
            uN = u0 + k * v0;
            vN = v0 - k * u0 + k * (GM / h2);
            return true;
        }

        private static bool metodoEulerRelativista(float u0, float v0, ref float uN, ref float vN, float k)
        {
            metodoEuler(u0, v0, ref uN, ref vN, k);
            vN += (k * 3 * GM * Pow(u0,2) / c2);
            return true;
        }

        #endregion

    }
}
