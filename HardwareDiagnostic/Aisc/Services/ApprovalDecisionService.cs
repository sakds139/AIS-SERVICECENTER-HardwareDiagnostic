using HardwareDiagnostic.Aisc.Models;
using HardwareDiagnostic.Models;

namespace HardwareDiagnostic.Aisc.Services;

public class ApprovalDecisionService
{
    public ApprovalResult Analyze(
        TicketRequest request,
        HardwareInfo hardware,
        WorkloadType workload)
    {
        var score = 0;

        var evidence = new List<string>();

        var recommendations = new List<string>();

        if (hardware.TotalRamGB <= 16)
        {
            score += 20;

            evidence.Add(
                $"RAM ปัจจุบัน {hardware.TotalRamGB:F0} GB");
        }

        if (hardware.RamUsagePercent > 80)
        {
            score += 20;

            evidence.Add(
                $"RAM Usage {hardware.RamUsagePercent:F0}%");
        }

        switch (workload)
        {
            case WorkloadType.AutomationTester:

                score += 40;

                evidence.Add(
                    "Automation / Regression Test เป็นงานที่ใช้ RAM สูง");

                break;

            case WorkloadType.PowerBI:

                score += 25;

                evidence.Add(
                    "Power BI ใช้ Memory สูง");

                break;

            case WorkloadType.Developer:

                score += 25;

                evidence.Add(
                    "Build Program ใช้พื้นที่และ RAM สูง");

                break;

            case WorkloadType.VideoEditor:

                score += 35;

                evidence.Add(
                    "Video Editing ใช้ RAM สูง");

                break;
        }

        var approve = score >= 60;

        if (approve)
        {
            recommendations.Add(
                "แนะนำอนุมัติคำขอ");
        }
        else
        {
            recommendations.Add(
                "ควรขอข้อมูลเพิ่มเติม");
        }

        return new ApprovalResult
        {
            IsApproved = approve,

            Decision = approve
                ? "APPROVE"
                : "REVIEW",

            Priority = score >= 80
                ? "HIGH"
                : score >= 60
                    ? "MEDIUM"
                    : "LOW",

            Score = score,

            Evidence = evidence,

            Recommendations = recommendations
        };
    }
}