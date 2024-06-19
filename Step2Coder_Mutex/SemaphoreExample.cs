using System;
using System.Threading;

namespace SemaphoreExample
{
    internal class Program
    {
        // Erstellen einer Semaphore, die maximal 3 gleichzeitige Zugriffe zulässt
        private static Semaphore sem = new Semaphore(3, 3);

        // Diese Methode simuliert die Arbeit, die nur von einer begrenzten Anzahl von Threads gleichzeitig ausgeführt werden kann
        public static void AccessResource()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} wartet darauf, die Ressource zu betreten...");
            sem.WaitOne(); // Warten, bis ein Slot in der Semaphore verfügbar ist
            Console.WriteLine($"{Thread.CurrentThread.Name} hat die Ressource betreten!");

            // Simulieren von Arbeiten innerhalb der kritischen Sektion
            Thread.Sleep(1000);

            Console.WriteLine($"{Thread.CurrentThread.Name} verlässt die Ressource.");
            sem.Release(); // Freigeben des Slots in der Semaphore
        }

        static void Main(string[] args)
        {
            // Erstellen und Starten von 10 Threads, die alle versuchen, auf die Ressource zuzugreifen
            for (int i = 1; i <= 10; i++)
            {
                Thread t = new Thread(AccessResource);
                t.Name = $"Thread_{i}";
                t.Start();
            }

            Console.ReadLine(); // Verhindern, dass die Anwendung sofort beendet wird
        }
    }
}
