using System;
using System.Collections.Generic;

namespace HardwareDiagnostic.Models;

public class HardwareInfo
{
    public DateTime CollectedAt { get; set; }

    public string ComputerName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;

    public string OperatingSystem { get; set; } = string.Empty;
    public string OsEdition { get; set; } = string.Empty;
    public string WindowsVersion { get; set; } = string.Empty;
    public string BuildNumber { get; set; } = string.Empty;
    public string Architecture { get; set; } = string.Empty;

    public string Cpu { get; set; } = string.Empty;
    public int CpuCores { get; set; }
    public int LogicalProcessors { get; set; }

    public double TotalRamGB { get; set; }
    public double AvailableRamGB { get; set; }
    public double RamUsagePercent { get; set; }
    public List<MemoryModule> MemoryModules { get; set; } = new();

    public double CpuUsagePercent { get; set; }

    public double DiskTotalGB { get; set; }
    public double DiskFreeGB { get; set; }
    public double DiskFreePercent { get; set; }
    public string SystemDriveLetter { get; set; } = string.Empty;

    public DateTime BootTime { get; set; }
    public double UptimeHours { get; set; }

    public List<DiskInfo> Disks { get; set; } = new();
    public List<PerformanceSample> PerformanceSamples { get; set; } = new();
    public List<ProcessInfo> Processes { get; set; } = new();

    public double DiskUsedGB => Math.Max(0, DiskTotalGB - DiskFreeGB);
}

public class DiskInfo
{
    public string Model { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public double SizeGB { get; set; }
    public string InterfaceType { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
}

public class PerformanceSample
{
    public DateTime Timestamp { get; set; }
    public double CpuUsagePercent { get; set; }
    public double AvailableRamGB { get; set; }
    public double DiskFreeGB { get; set; }
}

public class ProcessInfo
{
    public int ProcessId { get; set; }
    public string ProcessName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public double MemoryMB { get; set; }
    public double PrivateMemoryMB { get; set; }
    public double CpuUsagePercent { get; set; }
    public int ThreadCount { get; set; }
}

public class MemoryModule
{
    public string DeviceLocator { get; set; } = string.Empty;
    public string BankLabel { get; set; } = string.Empty;
    public double CapacityGB { get; set; }
    public int SpeedMHz { get; set; }
    public string Manufacturer { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string MemoryType { get; set; } = string.Empty;
}
