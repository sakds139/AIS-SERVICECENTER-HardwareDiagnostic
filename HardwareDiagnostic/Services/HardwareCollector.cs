using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management;
using System.Threading;
using HardwareDiagnostic.Models;

namespace HardwareDiagnostic.Services;

public class HardwareCollector
{
    public HardwareInfo Collect() => CollectInternal(CancellationToken.None);

    public Task<HardwareInfo> CollectAsync(CancellationToken cancellationToken = default) =>
        Task.Run(() => CollectInternal(cancellationToken), cancellationToken);

    private HardwareInfo CollectInternal(CancellationToken cancellationToken)
    {
        var info = new HardwareInfo
        {
            CollectedAt = DateTime.Now,
            ComputerName = Environment.MachineName,
            UserName = Environment.UserName
        };

        CollectComputerInfo(info, cancellationToken);
        CollectCpuInfo(info, cancellationToken);
        CollectMemoryInfo(info, cancellationToken);
        CollectMemoryModules(info, cancellationToken);
        CollectDiskInfo(info, cancellationToken);
        CollectCpuUsage(info, cancellationToken);
        CollectPerformanceSamples(info, cancellationToken);
        CollectProcessInfo(info, cancellationToken);
        CollectUptime(info, cancellationToken);

        return info;
    }

    private void CollectComputerInfo(HardwareInfo info, CancellationToken cancellationToken)
    {
        ThrowIfCancellationRequested(cancellationToken);

        try
        {
            using var computerSearcher =
                new ManagementObjectSearcher(
                    "SELECT Manufacturer, Model FROM Win32_ComputerSystem");

            foreach (ManagementObject obj in computerSearcher.Get())
            {
                info.Manufacturer = GetStringValue(obj, "Manufacturer");
                info.Model = GetStringValue(obj, "Model");
                break;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        ThrowIfCancellationRequested(cancellationToken);

        try
        {
            using var biosSearcher =
                new ManagementObjectSearcher(
                    "SELECT SerialNumber FROM Win32_BIOS");

            foreach (ManagementObject obj in biosSearcher.Get())
            {
                info.SerialNumber = GetStringValue(obj, "SerialNumber");
                break;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        ThrowIfCancellationRequested(cancellationToken);

        try
        {
            using var osSearcher =
                new ManagementObjectSearcher(
                    "SELECT Caption, Version, BuildNumber, OSArchitecture FROM Win32_OperatingSystem");

            foreach (ManagementObject obj in osSearcher.Get())
            {
                info.OperatingSystem = GetStringValue(obj, "Caption");
                info.OsEdition = GetStringValue(obj, "Caption");
                info.WindowsVersion = GetStringValue(obj, "Version");
                info.BuildNumber = GetStringValue(obj, "BuildNumber");
                info.Architecture = GetStringValue(obj, "OSArchitecture");
                break;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private void CollectCpuInfo(HardwareInfo info, CancellationToken cancellationToken)
    {
        ThrowIfCancellationRequested(cancellationToken);

        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT Name, NumberOfCores, NumberOfLogicalProcessors FROM Win32_Processor");

            foreach (ManagementObject obj in searcher.Get())
            {
                info.Cpu = GetStringValue(obj, "Name");
                info.CpuCores = GetIntValue(obj, "NumberOfCores");
                info.LogicalProcessors = GetIntValue(obj, "NumberOfLogicalProcessors");
                break;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private void CollectMemoryInfo(HardwareInfo info, CancellationToken cancellationToken)
    {
        ThrowIfCancellationRequested(cancellationToken);

        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");

            foreach (ManagementObject obj in searcher.Get())
            {
                var totalKB = GetDoubleValue(obj, "TotalVisibleMemorySize");
                var freeKB = GetDoubleValue(obj, "FreePhysicalMemory");

                info.TotalRamGB = totalKB / 1024 / 1024;
                info.AvailableRamGB = freeKB / 1024 / 1024;

                if (info.TotalRamGB > 0)
                {
                    info.RamUsagePercent = ((info.TotalRamGB - info.AvailableRamGB) / info.TotalRamGB) * 100;
                }

                break;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private void CollectMemoryModules(HardwareInfo info, CancellationToken cancellationToken)
    {
        ThrowIfCancellationRequested(cancellationToken);

        info.MemoryModules.Clear();

        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT BankLabel, DeviceLocator, Capacity, Speed, Manufacturer, PartNumber, SerialNumber, MemoryType FROM Win32_PhysicalMemory");

            foreach (ManagementObject obj in searcher.Get())
            {
                var module = new MemoryModule
                {
                    BankLabel = GetStringValue(obj, "BankLabel"),
                    DeviceLocator = GetStringValue(obj, "DeviceLocator"),
                    CapacityGB = GetDoubleValue(obj, "Capacity") / 1024 / 1024 / 1024,
                    SpeedMHz = GetIntValue(obj, "Speed"),
                    Manufacturer = GetStringValue(obj, "Manufacturer"),
                    PartNumber = GetStringValue(obj, "PartNumber"),
                    SerialNumber = GetStringValue(obj, "SerialNumber"),
                    MemoryType = GetStringValue(obj, "MemoryType")
                };

                info.MemoryModules.Add(module);
                ThrowIfCancellationRequested(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private void CollectDiskInfo(HardwareInfo info, CancellationToken cancellationToken)
    {
        ThrowIfCancellationRequested(cancellationToken);

        var systemDrive = DriveInfo.GetDrives()
            .Where(d => d.IsReady && d.DriveType == DriveType.Fixed)
            .OrderBy(d => d.Name.Equals("C:\\", StringComparison.OrdinalIgnoreCase) ? 0 : 1)
            .FirstOrDefault();

        if (systemDrive is not null)
        {
            info.SystemDriveLetter = systemDrive.Name.TrimEnd(Path.DirectorySeparatorChar);
            info.DiskTotalGB = systemDrive.TotalSize / 1024.0 / 1024 / 1024;
            info.DiskFreeGB = systemDrive.AvailableFreeSpace / 1024.0 / 1024 / 1024;

            if (info.DiskTotalGB > 0)
            {
                info.DiskFreePercent = (info.DiskFreeGB / info.DiskTotalGB) * 100;
            }
        }

        ThrowIfCancellationRequested(cancellationToken);

        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT Model, MediaType, Size, InterfaceType, SerialNumber, DeviceID FROM Win32_DiskDrive");

            foreach (ManagementObject obj in searcher.Get())
            {
                var disk = new DiskInfo
                {
                    Model = GetStringValue(obj, "Model"),
                    MediaType = GetStringValue(obj, "MediaType"),
                    SizeGB = GetDoubleValue(obj, "Size") / 1024 / 1024 / 1024,
                    InterfaceType = GetStringValue(obj, "InterfaceType"),
                    SerialNumber = GetStringValue(obj, "SerialNumber"),
                    DeviceId = GetStringValue(obj, "DeviceID")
                };

                info.Disks.Add(disk);
                ThrowIfCancellationRequested(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private void CollectCpuUsage(HardwareInfo info, CancellationToken cancellationToken)
    {
        ThrowIfCancellationRequested(cancellationToken);

        try
        {
            using var counter =
                new PerformanceCounter(
                    "Processor",
                    "% Processor Time",
                    "_Total");

            counter.NextValue();
            Thread.Sleep(1000);
            info.CpuUsagePercent = counter.NextValue();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            info.CpuUsagePercent = 0;
        }
    }

    private void CollectPerformanceSamples(HardwareInfo info, CancellationToken cancellationToken)
    {
        ThrowIfCancellationRequested(cancellationToken);

        info.PerformanceSamples.Clear();

        try
        {
            using var counter =
                new PerformanceCounter(
                    "Processor",
                    "% Processor Time",
                    "_Total");

            const int intervalSeconds = 5;
            const int sampleCount = 12;

            for (var i = 0; i < sampleCount; i++)
            {
                ThrowIfCancellationRequested(cancellationToken);

                counter.NextValue();
                Thread.Sleep(1000);
                var cpuUsage = counter.NextValue();
                var availableRamGb = GetAvailableRamGb();
                var diskFreeGb = GetCurrentDiskFreeGb();

                info.PerformanceSamples.Add(new PerformanceSample
                {
                    Timestamp = DateTime.Now,
                    CpuUsagePercent = cpuUsage,
                    AvailableRamGB = availableRamGb,
                    DiskFreeGB = diskFreeGb
                });

                if (i < sampleCount - 1)
                {
                    for (var j = 0; j < intervalSeconds - 1; j++)
                    {
                        ThrowIfCancellationRequested(cancellationToken);
                        Thread.Sleep(1000);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private void CollectProcessInfo(HardwareInfo info, CancellationToken cancellationToken)
    {
        ThrowIfCancellationRequested(cancellationToken);

        info.Processes.Clear();

        try
        {
            var processSamples = new List<(Process Process, TimeSpan CpuTime, ProcessInfo Info)>();

            foreach (var process in Process.GetProcesses())
            {
                ThrowIfCancellationRequested(cancellationToken);

                try
                {
                    var displayName = string.IsNullOrWhiteSpace(process.MainWindowTitle)
                        ? process.ProcessName
                        : process.MainWindowTitle;

                    var processInfo = new ProcessInfo
                    {
                        ProcessId = process.Id,
                        ProcessName = process.ProcessName,
                        DisplayName = displayName,
                        MemoryMB = process.WorkingSet64 / 1024.0 / 1024.0,
                        PrivateMemoryMB = process.PrivateMemorySize64 / 1024.0 / 1024.0,
                        ThreadCount = process.Threads.Count
                    };

                    processSamples.Add((process, process.TotalProcessorTime, processInfo));
                }
                catch
                {
                    process.Dispose();
                }
            }

            Thread.Sleep(1000);

            foreach (var sample in processSamples)
            {
                ThrowIfCancellationRequested(cancellationToken);

                try
                {
                    var nextCpuTime = sample.Process.TotalProcessorTime;
                    var cpuPercent = ((nextCpuTime - sample.CpuTime).TotalMilliseconds / 1000.0) / Environment.ProcessorCount * 100.0;
                    sample.Info.CpuUsagePercent = Math.Max(0, Math.Min(100, cpuPercent));
                    info.Processes.Add(sample.Info);
                }
                catch
                {
                }
                finally
                {
                    sample.Process.Dispose();
                }
            }

            info.Processes = info.Processes
                .OrderByDescending(p => p.MemoryMB)
                .Take(30)
                .ToList();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private static double GetAvailableRamGb()
    {
        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT FreePhysicalMemory FROM Win32_OperatingSystem");

            foreach (ManagementObject obj in searcher.Get())
            {
                var freeKb = GetDoubleValue(obj, "FreePhysicalMemory");
                return freeKb / 1024 / 1024;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return 0;
    }

    private static double GetCurrentDiskFreeGb()
    {
        try
        {
            var systemDrive = DriveInfo.GetDrives()
                .Where(d => d.IsReady && d.DriveType == DriveType.Fixed)
                .OrderBy(d => d.Name.Equals("C:\\", StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                .FirstOrDefault();

            return systemDrive?.AvailableFreeSpace / 1024.0 / 1024 / 1024 ?? 0;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return 0;
        }
    }

    private void CollectUptime(HardwareInfo info, CancellationToken cancellationToken)
    {
        ThrowIfCancellationRequested(cancellationToken);

        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT LastBootUpTime FROM Win32_OperatingSystem");

            foreach (ManagementObject obj in searcher.Get())
            {
                var bootString = GetStringValue(obj, "LastBootUpTime");

                if (!string.IsNullOrWhiteSpace(bootString))
                {
                    var bootTime = ManagementDateTimeConverter.ToDateTime(bootString);
                    info.BootTime = bootTime;
                    info.UptimeHours = (DateTime.Now - bootTime).TotalHours;
                }

                break;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    private static void ThrowIfCancellationRequested(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    private static string GetStringValue(ManagementBaseObject obj, string propertyName)
    {
        try
        {
            return obj[propertyName]?.ToString() ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    private static int GetIntValue(ManagementBaseObject obj, string propertyName, int defaultValue = 0)
    {
        try
        {
            var value = obj[propertyName];

            return value switch
            {
                null => defaultValue,
                int intValue => intValue,
                string text when int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) => parsed,
                _ => Convert.ToInt32(value, CultureInfo.InvariantCulture)
            };
        }
        catch
        {
            return defaultValue;
        }
    }

    private static double GetDoubleValue(ManagementBaseObject obj, string propertyName, double defaultValue = 0)
    {
        try
        {
            var value = obj[propertyName];

            return value switch
            {
                null => defaultValue,
                double doubleValue => doubleValue,
                string text when double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var parsed) => parsed,
                _ => Convert.ToDouble(value, CultureInfo.InvariantCulture)
            };
        }
        catch
        {
            return defaultValue;
        }
    }
}
