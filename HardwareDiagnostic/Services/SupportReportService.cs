using HardwareDiagnostic.Models;

namespace HardwareDiagnostic.Services;

public class SupportReportService
{
    public SupportReport BuildReport(HardwareInfo info)
    {
        var evidence = new List<string>();
        var actions = new List<string>();

        var sampleCount = info.PerformanceSamples.Count;
        var avgCpu = sampleCount > 0 ? info.PerformanceSamples.Average(x => x.CpuUsagePercent) : info.CpuUsagePercent;
        var peakCpu = sampleCount > 0 ? info.PerformanceSamples.Max(x => x.CpuUsagePercent) : info.CpuUsagePercent;
        var avgRam = sampleCount > 0 ? info.PerformanceSamples.Average(x => x.AvailableRamGB) : info.AvailableRamGB;
        var minRam = sampleCount > 0 ? info.PerformanceSamples.Min(x => x.AvailableRamGB) : info.AvailableRamGB;
        var avgDisk = sampleCount > 0 ? info.PerformanceSamples.Average(x => x.DiskFreeGB) : info.DiskFreeGB;

        evidence.Add($"CPU ถูกใช้งานเฉลี่ย {avgCpu:F1}% และสูงสุด {peakCpu:F1}% ในช่วงเก็บข้อมูล {sampleCount} จุด");
        evidence.Add($"RAM ที่เหลือเฉลี่ย {avgRam:F1} GB และต่ำสุด {minRam:F1} GB ในช่วงเวลาเดียวกัน");
        evidence.Add($"พื้นที่ดิสก์ฟรีเฉลี่ย {avgDisk:F1} GB");

        if (info.TotalRamGB < 8)
        {
            evidence.Add($"RAM ภายในเครื่องมีขนาด {info.TotalRamGB:F1} GB ซึ่งต่ำกว่าเกณฑ์ทั่วไปสำหรับงานหลายโปรแกรมพร้อมกัน");
            actions.Add("พิจารณาเพิ่ม RAM หากผู้ใช้รายงานว่าเครื่องช้าจากการเปิดโปรแกรมหลายตัวพร้อมกัน");
        }
        else
        {
            evidence.Add($"RAM ภายในเครื่องมีขนาด {info.TotalRamGB:F1} GB ซึ่งดูเพียงพอสำหรับงานทั่วไป");
        }

        if (peakCpu > 85)
        {
            evidence.Add($"CPU ติดสูงถึง {peakCpu:F1}% ชี้ให้เห็นว่าเครื่องถูกโหลดหนักในช่วงที่ตรวจสอบ");
            actions.Add("ตรวจสอบโปรแกรมหรือบริการที่ใช้ CPU สูง และลอง reboot ก่อนตัดสินใจเปลี่ยนฮาร์ดแวร์");
        }
        else
        {
            evidence.Add($"CPU ไม่ได้แสดงสัญญาณโหลดสูงเกินไปในช่วงเก็บข้อมูล");
        }

        if (avgDisk < 20)
        {
            evidence.Add($"พื้นที่ดิสก์ฟรีเหลือน้อย {avgDisk:F1} GB อาจส่งผลต่อความเร็วของระบบ");
            actions.Add("ปลดพื้นที่ดิสก์หรือย้ายไฟล์ขนาดใหญ่ออกก่อนเพื่อยืนยันว่าไม่ใช่ปัญหาจากพื้นที่เก็บข้อมูล");
        }

        if (info.UptimeHours > 72)
        {
            evidence.Add($"เครื่องทำงานต่อเนื่องนาน {info.UptimeHours:F1} ชั่วโมง อาจสะท้อนปัญหาความไม่เสถียรจากการรันต่อเนื่อง");
            actions.Add("แนะนำให้ reboot เครื่องและทดสอบซ้ำก่อนพิจารณาอัปเกรดฮาร์ดแวร์");
        }

        string recommendationLevel;
        string recommendationReason;
        var cause = "อื่น ๆ";

        if (info.TotalRamGB < 8 && minRam < 1.5 && peakCpu > 85)
        {
            recommendationLevel = "HIGH PRIORITY";
            recommendationReason = "หลักฐานแสดงว่าเครื่องมี RAM ต่ำและ CPU ถูกใช้งานหนักในช่วงตรวจสอบ จึงควรพิจารณาอัปเกรดฮาร์ดแวร์อย่างเร่งด่วน";
            cause = "RAM และ CPU";
        }
        else if (info.TotalRamGB < 8 || minRam < 1.5)
        {
            recommendationLevel = "RECOMMEND UPGRADE";
            recommendationReason = "พฤติกรรมช้าของเครื่องมีความสอดคล้องกับปัญหา RAM หรือหน่วยความจำไม่เพียงพอ จึงควรพิจารณาอัปเกรด";
            cause = "RAM";
        }
        else if (peakCpu > 85 || avgDisk < 20 || info.UptimeHours > 72)
        {
            recommendationLevel = "MONITOR";
            recommendationReason = "มีสัญญาณบางอย่างที่ควรติดตามหรือทดสอบเพิ่มก่อนตัดสินใจอัปเกรด";
            cause = "Performance / Disk / Uptime";
        }
        else
        {
            recommendationLevel = "NORMAL";
            recommendationReason = "ยังไม่พบหลักฐานเพียงพอว่าต้องอัปเกรดฮาร์ดแวร์ในทันที";
        }

        var report = new SupportReport
        {
            Summary = $"สรุปข้อมูลประสิทธิภาพของ {info.ComputerName} ตามหลักฐานจากเครื่องจริง ณ {info.CollectedAt:dd/MM/yyyy HH:mm:ss}",
            RecommendationLevel = recommendationLevel,
            RecommendationReason = recommendationReason,
            EvidencePoints = evidence,
            SuggestedActions = actions,
            AiPrompt = BuildAiPrompt(info, evidence, recommendationLevel, cause)
        };

        return report;
    }

    private static string BuildAiPrompt(HardwareInfo info, List<string> evidence, string recommendationLevel, string cause)
    {
        return $"""
คุณเป็นผู้วิเคราะห์งาน Support / Operations

ช่วยสรุปผลการตรวจประสิทธิภาพเครื่องต่อจากหลักฐานจริงต่อไปนี้เพื่อใช้สำหรับ Support / Management

เครื่อง: {info.ComputerName}
ผู้ใช้: {info.UserName}
CPU: {info.Cpu}
RAM: {info.TotalRamGB:F1} GB
CPU Usage ตอนนี้: {info.CpuUsagePercent:F1}%
พื้นที่ดิสก์ฟรี: {info.DiskFreeGB:F1} GB
Uptime: {info.UptimeHours:F1} ชั่วโมง
สาเหตุที่น่าจะเป็น: {cause}
ระดับคำแนะนำ: {recommendationLevel}

หลักฐาน:
- {string.Join("\n- ", evidence)}

กรุณาสรุปเป็นรายงานสั้น ๆ ที่มีส่วนต่อไปนี้:
1. สรุปสั้น ๆ ของปัญหา
2. หลักฐานที่สนับสนุนคำตัดสิน
3. คำแนะนำต่อ Support/Management
4. ควรอัปเกรดฮาร์ดแวร์หรือยัง ควรตรวจสอบต่อก่อน
""";
    }
}
