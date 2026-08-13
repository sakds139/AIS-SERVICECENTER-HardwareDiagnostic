using HardwareDiagnostic.Models;
using System.Linq;

namespace HardwareDiagnostic.Services;

public class UpgradeRecommendationService
{
    public UpgradeRecommendation AnalyzeRequest(HardwareInfo info, string requestReason)
    {
        var evidence = new List<string>();
        var actions = new List<string>();
        var score = 0;

        var normalizedReason = (requestReason ?? string.Empty).ToLowerInvariant();
        var hasRamRequest = normalizedReason.Contains("ram") || normalizedReason.Contains("แรม") || normalizedReason.Contains("memory");
        var hasSsdRequest = normalizedReason.Contains("ssd") || normalizedReason.Contains("harddisk") || normalizedReason.Contains("hdd") || normalizedReason.Contains("disk");
        var hasPerformanceIssue = normalizedReason.Contains("ช้า") || normalizedReason.Contains("ค้าง") || normalizedReason.Contains("หน่วง") || normalizedReason.Contains("slow") || normalizedReason.Contains("lag");
        var hasWorkloadMention = normalizedReason.Contains("after effect") || normalizedReason.Contains("visual studio") || normalizedReason.Contains("vscode") || normalizedReason.Contains("nodejs") || normalizedReason.Contains("automate") || normalizedReason.Contains("regression") || normalizedReason.Contains("powerbi") || normalizedReason.Contains("excel");

        var requestedPrograms = ExtractRequestedPrograms(normalizedReason);
        var matchingProcesses = info.Processes
            .Where(p => requestedPrograms.Any(requested => p.ProcessName.Contains(requested, StringComparison.OrdinalIgnoreCase) || p.DisplayName.Contains(requested, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        var averageCpu = info.PerformanceSamples.Any()
            ? info.PerformanceSamples.Average(sample => sample.CpuUsagePercent)
            : info.CpuUsagePercent;

        var averageAvailableRam = info.PerformanceSamples.Any()
            ? info.PerformanceSamples.Average(sample => sample.AvailableRamGB)
            : info.AvailableRamGB;

        if (averageAvailableRam < 2 || info.RamUsagePercent >= 80)
        {
            evidence.Add($"หน่วยความจำเหลือเฉลี่ย {averageAvailableRam:F1} GB ขณะใช้งานจริง ทำให้มีโอกาสเกิด paging และช้าได้");
            score += 20;
        }
        else
        {
            evidence.Add($"หน่วยความจำเฉลี่ยเหลือ {averageAvailableRam:F1} GB ขณะใช้งาน");
        }

        if (averageCpu > 75)
        {
            evidence.Add($"CPU เฉลี่ยในช่วงตรวจสอบสูง {averageCpu:F1}% แสดงว่าเครื่องอยู่ในภาวะโหลดสูง");
            score += 10;
        }
        else
        {
            evidence.Add($"CPU เฉลี่ยในช่วงตรวจสอบ {averageCpu:F1}% ซึ่งไม่สูงเกินไป");
        }

        if (info.DiskFreeGB < 20)
        {
            evidence.Add($"พื้นที่ดิสก์ว่างเหลือ {info.DiskFreeGB:F1} GB ซึ่งต่ำและอาจทำให้ระบบช้าจาก I/O");
            score += 10;
        }
        else
        {
            evidence.Add($"พื้นที่ดิสก์ว่างเหลือ {info.DiskFreeGB:F1} GB อยู่ในระดับที่ปลอดภัย");
        }

        if (matchingProcesses.Any())
        {
            foreach (var process in matchingProcesses)
            {
                evidence.Add($"โปรแกรมที่ร้องขอ '{process.DisplayName}' กำลังทำงานและใช้ RAM {process.MemoryMB:F1} MB, CPU {process.CpuUsagePercent:F1}%");
                score += process.MemoryMB > 500 ? 15 : 7;
            }
        }

        var heavyProcesses = info.Processes
            .Where(p => p.MemoryMB >= 300 || p.CpuUsagePercent >= 10)
            .OrderByDescending(p => p.MemoryMB)
            .Take(5)
            .ToList();

        if (heavyProcesses.Any())
        {
            evidence.Add("โปรแกรมที่ใช้ทรัพยากรสูงในปัจจุบัน:");
            score += 5;

            foreach (var process in heavyProcesses)
            {
                evidence.Add($"- {process.DisplayName} ({process.ProcessName}) ใช้ RAM {process.MemoryMB:F1} MB, CPU {process.CpuUsagePercent:F1}%");
            }
        }

        if (hasRamRequest)
        {
            evidence.Add("คำร้องขอมีเนื้อหาที่เกี่ยวกับ RAM และประสิทธิภาพการทำงานหลายโปรแกรมพร้อมกัน");
            score += 15;
        }

        if (hasSsdRequest)
        {
            evidence.Add("คำร้องขอมีเนื้อหาที่เกี่ยวกับ SSD/HDD และความเร็วในการอ่าน/เขียนข้อมูล");
            score += 15;
        }

        if (hasPerformanceIssue)
        {
            evidence.Add("คำร้องขอระบุอาการเครื่องช้า ค้าง หน่วง และมีผลกระทบต่อการทำงานจริง");
            score += 15;
        }

        if (hasWorkloadMention)
        {
            evidence.Add("คำร้องขอระบุ workload ที่หนัก เช่น After Effect, Visual Studio, VS Code, NodeJS, Automation, Excel/PowerBI");
            score += 10;
        }

        var recommendation = "ไม่ระบุ";
        var resultLevel = "NORMAL";
        var priority = "กลาง";
        var summary = "ยังไม่มีหลักฐานเพียงพอที่จะยืนยันความจำเป็นในการอัปเกรดฮาร์ดแวร์";
        var hardware = "-";

        var hasStrongEvidence = score >= 55;
        var isRamNeed = hasRamRequest && info.TotalRamGB < 16 && (score >= 55 || (info.TotalRamGB < 8 && (hasPerformanceIssue || hasWorkloadMention)));
        var isSsdNeed = hasSsdRequest && info.DiskFreeGB < 20 && (score >= 55 || hasPerformanceIssue);

        if (isRamNeed)
        {
            recommendation = "อนุมัติพร้อมพิจารณาอัปเกรด RAM";
            resultLevel = "HIGH PRIORITY";
            priority = "สูง";
            summary = "คำร้องขอมีหลักฐานเพียงพอว่าเครื่องต้องการ RAM เพิ่มเพื่อรองรับงานที่หนักและความล่าช้าจริง";
            hardware = "RAM";
            actions.Add("พิจารณาเพิ่ม RAM ให้ตรงกับ workload ที่ระบุ");
            actions.Add("ตรวจสอบว่าเครื่องมี slot ว่างและรองรับความจุที่เสนอ");
            actions.Add("เก็บหลักฐานเพิ่มเติมจากผู้ใช้หากต้องการให้การอนุมัติรัดกุมขึ้น");
        }
        else if (isSsdNeed)
        {
            recommendation = "อนุมัติพร้อมพิจารณาอัปเกรด SSD";
            resultLevel = "HIGH PRIORITY";
            priority = "สูง";
            summary = "คำร้องขอมีหลักฐานเพียงพอว่าอาจจำเป็นต้องเปลี่ยน HDD เป็น SSD M.2 เพื่อแก้ปัญหาความล่าช้า";
            hardware = "SSD";
            actions.Add("พิจารณาเปลี่ยนจาก HDD เป็น SSD M.2 หากระบบรองรับ");
            actions.Add("ยืนยันความจุและความเข้ากันได้ก่อนดำเนินการ");
        }
        else if (hasStrongEvidence)
        {
            recommendation = "พิจารณาอนุมัติ";
            resultLevel = "RECOMMEND UPGRADE";
            priority = "กลาง";
            summary = "มีหลักฐานบางส่วนที่บ่งชี้ว่าต้องการอัปเกรด แต่ยังต้องมีการตรวจสอบเพิ่มเติมก่อนตัดสินใจสุดท้าย";
            hardware = "RAM/SSD ตามผลตรวจ";
            actions.Add("ขอข้อมูลเพิ่มเติมจากผู้ใช้ เช่น สเปกเครื่องและผลลัพธ์จากการวัดประสิทธิภาพ");
            actions.Add("ตรวจสอบว่า workload ที่ระบุจริง ๆ สอดคล้องกับการใช้งานในเครื่อง");
        }
        else if (hasRamRequest || hasSsdRequest || hasPerformanceIssue)
        {
            recommendation = "ควรขอข้อมูลเพิ่มเติม";
            resultLevel = "MONITOR";
            priority = "ต่ำ";
            summary = "คำร้องขอมีความเกี่ยวข้องกับฮาร์ดแวร์ แต่ยังไม่เพียงพอที่จะยืนยันได้ว่าต้องอัปเกรดทันที";
            hardware = "RAM/SSD ตามผลตรวจ";
            actions.Add("ต้องมีหลักฐานเพิ่มเติมก่อนพิจารณาอนุมัติ");
            actions.Add("ขอข้อมูลจากผู้ใช้หรือทีมช่างเพิ่มเติมก่อนดำเนินการ");
        }
        else
        {
            recommendation = "ไม่มีหลักฐานเพียงพอสำหรับการอัปเกรด";
            resultLevel = "NORMAL";
            priority = "ต่ำ";
            summary = "ข้อมูลปัจจุบันยังไม่สนับสนุนการอัปเกรดฮาร์ดแวร์ในทันที";
            hardware = "-";
        }

        var scoreLabel = score >= 70 ? "สูงมาก" : score >= 45 ? "สูง" : score >= 25 ? "กลาง" : "ต่ำ";

        var report = new UpgradeRecommendation
        {
            Verdict = recommendation,
            ResultLevel = resultLevel,
            Priority = priority,
            Summary = summary,
            RecommendedHardware = hardware,
            Score = score,
            ScoreLabel = scoreLabel,
            EvidencePoints = evidence,
            SuggestedActions = actions,
            AiPrompt = BuildAiPrompt(info, requestReason ?? string.Empty, recommendation, priority, evidence, score)
        };

        return report;
    }

    private static IReadOnlyList<string> ExtractRequestedPrograms(string normalizedReason)
    {
        var programKeywords = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "after effect", "After Effect" },
            { "adobe premiere", "Premiere" },
            { "visual studio", "Visual Studio" },
            { "vscode", "VS Code" },
            { "excel", "Excel" },
            { "powerbi", "PowerBI" },
            { "photoshop", "Photoshop" },
            { "autocad", "AutoCAD" },
            { "nodejs", "NodeJS" },
            { "chrome", "Chrome" },
            { "edge", "Edge" },
            { "teams", "Teams" },
            { "outlook", "Outlook" },
            { "sql server", "SQL Server" },
            { "discord", "Discord" }
        };

        return programKeywords
            .Where(kvp => normalizedReason.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase))
            .Select(kvp => kvp.Value)
            .ToList();
    }

    private static string BuildAiPrompt(HardwareInfo info, string requestReason, string recommendation, string priority, List<string> evidence, int score)
    {
        return $"""
คุณเป็นผู้วิเคราะห์งาน Support / IT Operations

ช่วยประเมินคำร้องขอ Upgrade ฮาร์ดแวร์จากผู้ใช้โดยอิงหลักฐานจากเครื่องจริง

เครื่อง: {info.ComputerName}
ผู้ใช้: {info.UserName}
RAM: {info.TotalRamGB:F1} GB
CPU Usage: {info.CpuUsagePercent:F1}%
พื้นที่ดิสก์ว่าง: {info.DiskFreeGB:F1} GB
คำร้องขอ: {requestReason}

หลักฐาน:
- {string.Join("\n- ", evidence)}

คำแนะนำที่ควรให้: {recommendation}
ระดับความสำคัญ: {priority}
คะแนนพิจารณา: {score}

กรุณาสรุปเป็นรายงานสั้น ๆ สำหรับ Support/Management โดยเน้นว่า
1. เหตุผลขอ Upgrade มีน้ำหนักเพียงพอหรือไม่
2. ควรอนุมัติอุปกรณ์ใดตามเกณฑ์มาตรฐาน
3. ควรดำเนินการอะไรต่อเพื่อยืนยันความจำเป็น
4. ถ้าคะแนนต่ำกว่าเกณฑ์ โปรดระบุว่าไม่ควรอนุมัติในปัจจุบันและต้องขอข้อมูลเพิ่มเติม
""";
    }
}
