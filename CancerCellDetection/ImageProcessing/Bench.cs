using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public static class Bench
    {
        public static void ComputeCpuMemoryUsage(Action action, long length, int iteration = 1,
            [CallerMemberName] string caller = null)
        {
            //Calcul du temps d'exécution pour n itération
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iteration; i++)
                action();
            stopwatch.Stop();
            //Affichage des résultats dans la sortie standard
            Process proc = Process.GetCurrentProcess();
            CultureInfo ci = new CultureInfo("en-US");
            CultureInfo.CurrentCulture = ci;
            Console.WriteLine("Method : {0}", caller);
            Console.WriteLine("Time elapsed={0}", stopwatch.Elapsed);
            //Affichage de la charge mémoire maximum
            Console.WriteLine("Memory usage (bytes)={0}", proc.PeakWorkingSet64);
            Console.WriteLine("Memory usage (MByte)={0}", proc.PeakWorkingSet64 / 1048576);
            Console.WriteLine("Lenght : {0}", length);
            //Sauvegarde des résultats dans un fichier CSV
            string str = $"{caller};{stopwatch.Elapsed.TotalSeconds};{proc.PeakWorkingSet64};{proc.PeakWorkingSet64 / 1048576};{length};{iteration}";
            var writer = File.AppendText(@".\performance.csv");
            writer.AutoFlush = true;
            writer.WriteLine(str);
            writer.Close();
        }


    }
}
