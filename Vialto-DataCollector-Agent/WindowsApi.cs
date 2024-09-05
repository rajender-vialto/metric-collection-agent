using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Vialto_DataCollector_Agent
{
    public class MemoryMetrics
    {
        public double Total;
        public double Used;
        public double Free;
    }

    public class MemoryMetricsClient
    {
        public MemoryMetrics GetMetrics()
        {
            if (IsUnix())
            {
                return GetUnixMetrics();
            }

            return GetWindowsMetrics();
        }

        private bool IsUnix()
        {
            var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                         RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            return isUnix;
        }

        private MemoryMetrics GetWindowsMetrics()
        {
            var output = "";

            var info = new ProcessStartInfo();
            info.FileName = "wmic";
            info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            var lines = output.Trim().Split("\n");
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }

        private MemoryMetrics GetUnixMetrics()
        {
            var output = "";

            var info = new ProcessStartInfo("free -m");
            info.FileName = "/bin/bash";
            info.Arguments = "-c \"free -m\"";
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
            }

            var lines = output.Split("\n");
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = double.Parse(memory[1]);
            metrics.Used = double.Parse(memory[2]);
            metrics.Free = double.Parse(memory[3]);

            return metrics;
        }
    }

    public class DiskMetricsClient
    {
        public List<DiskMetrics> GetMetrics()
        {
            return GetWindowsMetrics();
        }
        private List<DiskMetrics> GetWindowsMetrics()
        {
            var metrics = new List<DiskMetrics>();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (var driveInfo in allDrives)
            {
                metrics.Add(new DiskMetrics()
                {
                    Name = driveInfo.Name,
                    Free = driveInfo.AvailableFreeSpace / 1024,
                    Total = driveInfo.TotalSize / 1024,
                    Used = (driveInfo.TotalSize - driveInfo.AvailableFreeSpace) / 1024
                });

            }

            return metrics;
        }

    }

    public class DiskMetrics : MemoryMetrics
    {
        public string Name;
    }

}
