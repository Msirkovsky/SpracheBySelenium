using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCasesDSL;
using SeleniumEngine;
using System.IO;
//using Sprache;
using TestCasesDSL.HtmlConvertor;
using SeleniumTests;

namespace RunTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryName;
            directoryName = GetDirectoryName(args);

            FileInfo[] fis = GetFiles(directoryName);
            List<string> chybneTesty = new List<string>();
            using (SeleniumEngine.HtmlSeleniumEngine.Engine e = new SeleniumEngine.HtmlSeleniumEngine.Engine())
            {
                Console.WriteLine();
                Console.WriteLine();
                foreach (var file in fis)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(file.Name + " running...");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    TestResult result = RunTest(e, file);
                    VyhodnotAndZobrazTestResult(chybneTesty, file, result);
                }
                WriteFinalResult(chybneTesty);
            }
        }

        private static FileInfo[] GetFiles(string directoryName)
        {
            DirectoryInfo di = new DirectoryInfo(directoryName);
            FileInfo[] fis = di.GetFiles();
            return fis;
        }

        private static string GetDirectoryName(string[] args)
        {
            string directoryName;
            if (args.Length == 0)
                directoryName = "default";
            else
                directoryName = args[0];
            return directoryName;
        }

        private static TestResult RunTest(SeleniumEngine.HtmlSeleniumEngine.Engine e, FileInfo file)
        {
            Convertor c = new Convertor();
            HtmlTestCase testCase = c.Load(File.ReadAllText(file.FullName));
            TestResult result = e.Run(testCase, Console.Write);
            return result;
        }

        private static void VyhodnotAndZobrazTestResult(List<string> chybneTesty, FileInfo file, TestResult result)
        {
            TestResultFormatter formatter = new TestResultFormatter();
            bool nalezenaChyba;
            string text = formatter.Format(result, out nalezenaChyba);
            if (nalezenaChyba)
            {
                chybneTesty.Add(file.Name);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("Došlo k chybě");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("OK");
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;

            
        }

        private static void WriteFinalResult(List<string> chybneTesty)
        {
            if (chybneTesty.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Testy, které skončili s chybou:");
                chybneTesty.ForEach(p => Console.WriteLine(p));
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Všechny testy prošly v pořádku");
            }
        }


    }
}
