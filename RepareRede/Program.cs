using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace RepareRede
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Reparar Rede";
            var status = IPStatus.NoResources;
            do
            { 
                var ping = new Ping();                
                var replay = ping.Send("216.58.202.14");//google.com

                if (replay.Status == IPStatus.Success)
                {
                    "Sucesso no ping".Log(ConsoleColor.Green);
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    "Falha no ping".Log(ConsoleColor.Red);
                    "Recuperar rede".Log(ConsoleColor.Yellow);
                    try
                    {
                        RecuperarRede();
                    }
                    catch{ }
                    
                    System.Threading.Thread.Sleep(10000);
                }

            } while (true);           
        }

        static void RecuperarRede()
        {
            using (var proc = new Process())
            {
                proc.StartInfo = new ProcessStartInfo(@"cmd");
                proc.StartInfo.Arguments = @"/k netsh interface set interface ""ethernet"" DISABLED";
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;

                proc.Start();
                "Desabilitando placa de rede".Log(ConsoleColor.Yellow);
                proc.WaitForExit(10000);

                proc.CloseMainWindow();
                proc.Close();
                proc.Dispose();
            }

            using (var proc = new Process())
            {
                proc.StartInfo = new ProcessStartInfo(@"cmd");
                proc.StartInfo.Arguments = @"/k netsh interface set interface ""ethernet"" ENABLED";
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;

                proc.Start();
                "Habilitando placa de rede".Log(ConsoleColor.Yellow);
                proc.WaitForExit(10000);

                proc.CloseMainWindow();
                proc.Close();
                proc.Dispose();
            }
        }
    }

    public static class StringExtensions
    {
        public static string Log(this string str, ConsoleColor cor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = cor;
            Console.WriteLine(string.Format("{0}: {1}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff"), str));
            return str;
        }
    }
}