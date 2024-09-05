using System;

namespace Vialto_DataCollector_Agent
{
    class Program
    {
        static void Main(string[] args)
        {

            var memory = new MemoryMetricsClient();
            var metrics = memory.GetMetrics();
            Console.WriteLine("Total :" + metrics.Total);
            Console.WriteLine("Used :" + metrics.Used);
            Console.WriteLine("Free :" + metrics.Free);

            var disk = new DiskMetricsClient();
            var diskMetrics = disk.GetMetrics();

            foreach(var a in diskMetrics)
            {
                Console.WriteLine("Name :" + a.Name);
                Console.WriteLine("Total :" + a.Total);
                Console.WriteLine("Used :" + a.Used);
                Console.WriteLine("Free :" + a.Free);
            }

            Console.WriteLine("Hello World!"); 
        }
    }
}

