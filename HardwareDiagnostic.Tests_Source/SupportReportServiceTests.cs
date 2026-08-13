using HardwareDiagnostic.Models;
using HardwareDiagnostic.Services;
using Xunit;

namespace HardwareDiagnostic.Tests;

public class SupportReportServiceTests
{
    [Fact]
    public void BuildReport_UsesEvidenceToRecommendHighPriorityWhenMemoryAndCpuAreBad()
    {
        var info = new HardwareInfo
        {
            ComputerName = "TEST-PC",
            UserName = "tester",
            TotalRamGB = 4,
            AvailableRamGB = 1,
            CpuUsagePercent = 90,
            DiskFreeGB = 10,
            UptimeHours = 100,
            Cpu = "Intel i5"
        };

        var service = new SupportReportService();
        var report = service.BuildReport(info);

        Assert.Contains("สูง", report.RecommendationLevel);
        Assert.Contains("RAM", report.AiPrompt);
        Assert.Contains("CPU", report.AiPrompt);
    }
}
