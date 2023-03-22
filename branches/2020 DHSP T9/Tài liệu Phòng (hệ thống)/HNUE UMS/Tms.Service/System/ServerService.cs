using System.Diagnostics;

namespace Ums.Services.System
{
    public class ServerService
    {
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        public ServerService()
        {

            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

        }

        public string GetCurrentCpuUsage()
        {
            return cpuCounter.NextValue() + "%";
        }

        public string GetAvailableRAM()
        {
            return ramCounter.NextValue() + "MB";
        }
    }
}
