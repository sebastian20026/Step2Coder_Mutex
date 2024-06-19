using System;
using System.Threading;
using System.IO;
using OfficeOpenXml;
using System.Collections.Generic;
namespace Step2Coder_Mutex
{
    internal class Program
    {
        private static Mutex m1 = new Mutex();
        private static string filepath = @"C:\Users\FP2402392\Downloads\test.txt";
        private static string excelpath = @"C:\Users\FP2402392\Documents\sigma.xlsx";
        // simullieren von Mutex absichern dass man auf eine datei nicht schreibt und liest gleichzetiig ...
        //loese mittels mutex und c# 
        //ueberblick ueber die methode in mutex...
        public static void ReadFromFile(string text)
        {
            m1.WaitOne(1000); // nur auf eine ressource zugreifen..
            try
            {
                if (File.Exists(filepath))
                {
                    string content = File.ReadAllText(filepath);
                    Console.WriteLine("Dateniinhalt gelesen: " + content);

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;



                    using (var package = new ExcelPackage(new FileInfo(excelpath)))
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        worksheet.Cells[1, 1].Value = "Dateiinhalt";
                        worksheet.Cells[2, 1].Value = content;
                        package.Save();

                    }


                }
                else
                { Console.WriteLine("datei nicht verfügbar");
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Fehler beim Lesen der Datei : "+ex.Message);
            }
            finally
            {
                m1.ReleaseMutex(); //gibt das mutex frei
            }
            

           //  Console.WriteLine("Thread" + Thread);
            //hier kommt rein was die funktion macht
            //m1.ReleaseMutex();
        }   
        
        public static void WriteToFile(string text)
        {
            m1.WaitOne();
            try
            {
                File.AppendAllText(filepath, text + Environment.NewLine);
                Console.WriteLine("Text in datei geschreiben" + text);
            }
            catch ( Exception ex)
            {
                Console.WriteLine("Fehler beim schreiben in die fatei " +ex.Message);
            }
            finally
            { m1.ReleaseMutex(); // gibt das mutex frei
            }
            


        }

        static void Main(string[] args)
        {
            
            Thread t1 = new Thread(() => ReadFromFile(""));
            Thread t2 = new Thread(() => WriteToFile("beispieltext"));

            t1.Start();
            t2.Start();

            t1.Join(); // wartet bis t1 beendet ist
            t2.Join(); //Wartet, bis t2 beendet ist

            Console.WriteLine("Operration abgeschlossen.");
        }
    }
}
