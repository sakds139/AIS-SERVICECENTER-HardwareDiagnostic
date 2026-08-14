using HardwareDiagnostic.Aisc.Models;

namespace HardwareDiagnostic.Aisc.Services;

public class WorkloadAnalyzerService
{
    public WorkloadType Analyze(string reason)
    {
        var text = reason.ToLower();

        if (text.Contains("regression"))
            return WorkloadType.AutomationTester;

        if (text.Contains("automation"))
            return WorkloadType.AutomationTester;

        if (text.Contains("automate"))
            return WorkloadType.AutomationTester;

        if (text.Contains("skyfbb"))
            return WorkloadType.AutomationTester;

        if (text.Contains("powerbi"))
            return WorkloadType.PowerBI;

        if (text.Contains("power bi"))
            return WorkloadType.PowerBI;

        if (text.Contains("python"))
            return WorkloadType.DataAnalyst;

        if (text.Contains("vscode"))
            return WorkloadType.Developer;

        if (text.Contains("vs code"))
            return WorkloadType.Developer;

        if (text.Contains("visual studio"))
            return WorkloadType.Developer;

        if (text.Contains("build"))
            return WorkloadType.Developer;

        if (text.Contains("after effect"))
            return WorkloadType.VideoEditor;

        return WorkloadType.Office;
    }
}