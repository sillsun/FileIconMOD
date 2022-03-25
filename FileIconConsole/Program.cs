using System;
using System.IO;
using System.Text;
using System.Linq;
namespace FileIconConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var argDic = ConsoleHelper.ConvertToArgDic(args);
                foreach (var item in argDic)
                {
                    Console.WriteLine($"{item.Key} : {item.Value.FirstOrDefault()}");
                }

                if (argDic.TryGetValue("-type", out var type))
                {
                    var reg = new RegBuilder(argDic);
                    using (StreamWriter sr = new StreamWriter("run.reg", false, Encoding.Default))
                    {
                        sr.Write(reg.ToString());
                    }
                    var p1 = System.Diagnostics.Process.Start("regedit.exe", "/s run.reg");
                    p1.WaitForExit();
                }


                RestartExplorer();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }



        }

        static void RestartExplorer()
        {
            var paths = ExplorerHelper.GetOpenedDirectory();

            var p2 = System.Diagnostics.Process.Start("cmd.exe", "/c taskkill /f /im Explorer.exe");
            p2.WaitForExit();
            var p3 = System.Diagnostics.Process.Start("cmd.exe", "/c start explorer.exe");
            p3.WaitForExit();

            paths.Reverse();
            paths.ForEach(p =>
            {
                if (Directory.Exists(p))
                {
                    var pp  = System.Diagnostics.Process.Start("cmd.exe", $"/c explorer.exe {p}");
                    pp.WaitForExit();
                    Console.WriteLine($"Start : {p}");
                }
            });
        }
    }



}
