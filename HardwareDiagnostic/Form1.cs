using System.Diagnostics;
using System.Text.Json;
using HardwareDiagnostic.Models;
using HardwareDiagnostic.Services;

namespace HardwareDiagnostic;

public partial class Form1 : Form
{
    private readonly HardwareCollector _collector = new();
    private CancellationTokenSource? _diagnosticCancellation;
    private readonly Stopwatch _stopwatch = new();
    private HardwareInfo? _currentInfo;
    private SupportReport? _currentSupportReport;
    private UpgradeRecommendation? _currentUpgradeRecommendation;

    public Form1()
    {
        InitializeComponent();
    }

    private async void btnStart_Click(object? sender, EventArgs e)
    {
        btnStart.Enabled = false;
        btnStop.Enabled = true;
        btnGenerateReport.Enabled = false;
        btnExportJson.Enabled = false;

        progressBar.Value = 0;
        txtResult.Clear();
        lblRecommendation.Text = "Recommendation: N/A";
        lblStatus.Text = "Status: Starting diagnostic...";

        _diagnosticCancellation?.Cancel();
        _diagnosticCancellation?.Dispose();
        _diagnosticCancellation = new CancellationTokenSource();

        _stopwatch.Restart();
        timerElapsed.Start();

        try
        {
            progressBar.Value = 10;

            var userRequest = string.IsNullOrWhiteSpace(txtRequestReason.Text)
                ? "ผู้ใช้แจ้งว่าเครื่องค้าง ช้า หน่วง ขณะใช้งาน After Effect และต้องการเพิ่ม RAM เพื่อรองรับการประมวลผลและงานที่ใช้ Visual Studio, VS Code, NodeJS, Automation Regression Test ของ SKYFBB และงาน Data Analysis / Excel / PowerBI / OWS AUTIN / Remote AISCAS"
                : txtRequestReason.Text.Trim();

            var requestId = string.IsNullOrWhiteSpace(txtRequestId.Text)
                ? "N/A"
                : txtRequestId.Text.Trim();

            lblStatus.Text = "Status: Collecting hardware data...";
            var info = await _collector.CollectAsync(_diagnosticCancellation.Token);
            _currentInfo = info;

            progressBar.Value = 60;
            lblStatus.Text = "Status: Analyzing results...";

            var reportService = new SupportReportService();
            var report = reportService.BuildReport(info);
            _currentSupportReport = report;

            var upgradeService = new UpgradeRecommendationService();
            var upgradeReport = upgradeService.AnalyzeRequest(info, userRequest);
            _currentUpgradeRecommendation = upgradeReport;

            progressBar.Value = 100;
            lblStatus.Text = "Status: Diagnostic completed.";
            lblRecommendation.Text = $"Recommendation: {upgradeReport.Verdict}";

            ShowResult(info, report, upgradeReport, requestId);
            btnGenerateReport.Enabled = true;
            btnExportJson.Enabled = true;
        }
        catch (OperationCanceledException)
        {
            lblStatus.Text = "Status: Diagnostic canceled.";
            txtResult.Text = "การตรวจสอบถูกยกเลิกโดยผู้ใช้";
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Status: Error occurred.";
            MessageBox.Show(ex.Message, "Diagnostic error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            _stopwatch.Stop();
            timerElapsed.Stop();
        }
    }

    private void btnStop_Click(object? sender, EventArgs e)
    {
        btnStop.Enabled = false;
        lblStatus.Text = "Status: Stopping...";
        _diagnosticCancellation?.Cancel();
    }

    private void TimerElapsed_Tick(object? sender, EventArgs e)
    {
        if (_stopwatch.IsRunning)
        {
            lblElapsedTime.Text = $"Elapsed Time: {_stopwatch.Elapsed:hh\\:mm\\:ss}";
        }
    }

    private void btnGenerateReport_Click(object? sender, EventArgs e)
    {
        if (_currentInfo is null || _currentSupportReport is null || _currentUpgradeRecommendation is null)
        {
            MessageBox.Show("ไม่มีข้อมูลเพียงพอสำหรับสร้างรายงาน", "Generate Report", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            using var dialog = new SaveFileDialog();
            dialog.Filter = "HTML files (*.html)|*.html";
            dialog.FileName = $"HardwareDiagnostic_{DateTime.Now:yyyyMMdd_HHmmss}.html";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, BuildHtmlReport(_currentInfo, _currentSupportReport, _currentUpgradeRecommendation));
                MessageBox.Show($"รายงานถูกสร้างเรียบร้อยแล้ว\n{dialog.FileName}", "Generate Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Generate Report failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnExportJson_Click(object? sender, EventArgs e)
    {
        if (_currentInfo is null)
        {
            MessageBox.Show("ไม่มีข้อมูลเพียงพอสำหรับส่งออก JSON", "Export JSON", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            using var dialog = new SaveFileDialog();
            dialog.Filter = "JSON files (*.json)|*.json";
            dialog.FileName = $"HardwareDiagnostic_{DateTime.Now:yyyyMMdd_HHmmss}.json";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var exportObject = new
                {
                    computer = new
                    {
                        _currentInfo.ComputerName,
                        _currentInfo.UserName,
                        _currentInfo.Manufacturer,
                        _currentInfo.Model,
                        _currentInfo.SerialNumber
                    },
                    windows = new
                    {
                        _currentInfo.OperatingSystem,
                        _currentInfo.OsEdition,
                        _currentInfo.WindowsVersion,
                        _currentInfo.BuildNumber,
                        _currentInfo.Architecture,
                        _currentInfo.BootTime,
                        _currentInfo.UptimeHours
                    },
                    hardware = new
                    {
                        _currentInfo.Cpu,
                        _currentInfo.CpuCores,
                        _currentInfo.LogicalProcessors,
                        _currentInfo.TotalRamGB,
                        _currentInfo.AvailableRamGB,
                        _currentInfo.RamUsagePercent,
                        _currentInfo.SystemDriveLetter,
                        _currentInfo.DiskTotalGB,
                        _currentInfo.DiskFreeGB,
                        _currentInfo.DiskFreePercent,
                        _currentInfo.Disks,
                        _currentInfo.MemoryModules
                    },
                    performance = _currentInfo.PerformanceSamples,
                    processes = _currentInfo.Processes,
                    analysis = _currentSupportReport,
                    recommendation = _currentUpgradeRecommendation
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(exportObject, options);
                File.WriteAllText(dialog.FileName, json);
                MessageBox.Show($"Export JSON completed\n{dialog.FileName}", "Export JSON", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Export JSON failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private string BuildHtmlReport(HardwareInfo info, SupportReport report, UpgradeRecommendation upgradeReport)
    {
        var diskRows = info.Disks.Any()
            ? string.Join("", info.Disks.Select(d => $"<tr><td>{d.Model}</td><td>{d.MediaType}</td><td>{d.InterfaceType}</td><td>{d.SizeGB:F1} GB</td><td>{d.SerialNumber}</td></tr>"))
            : "<tr><td colspan=5>ไม่มีข้อมูลดิสก์ทางกายภาพ</td></tr>";

        return $"<!DOCTYPE html>\n<html lang=\"th\">\n<head><meta charset=\"utf-8\"><title>Hardware Diagnostic Report</title><style>body{{font-family:Segoe UI,Arial,sans-serif;line-height:1.4}}table{{width:100%;border-collapse:collapse}}th,td{{border:1px solid #ccc;padding:8px;text-align:left}}th{{background:#f4f4f4}}</style></head><body>\n" +
               $"<h1>AIS SERVICE CENTER Hardware Diagnostic Tool</h1>\n" +
               $"<h2>Computer Information</h2>\n" +
               $"<p><strong>Computer:</strong> {info.ComputerName}<br><strong>User:</strong> {info.UserName}<br><strong>OS:</strong> {info.OperatingSystem} ({info.Architecture})<br><strong>Version:</strong> {info.WindowsVersion} Build {info.BuildNumber}</p>\n" +
               $"<h2>Hardware Summary</h2>\n" +
               $"<p><strong>CPU:</strong> {info.Cpu}<br><strong>Cores:</strong> {info.CpuCores} / <strong>Logical:</strong> {info.LogicalProcessors}<br><strong>RAM:</strong> {info.TotalRamGB:F1} GB ({info.AvailableRamGB:F1} GB available)<br><strong>System Drive:</strong> {info.SystemDriveLetter} {info.DiskUsedGB:F1}/{info.DiskTotalGB:F1} GB ({info.DiskFreePercent:F1}% free)</p>\n" +
               $"<h3>Disk devices</h3><table><thead><tr><th>Model</th><th>Media Type</th><th>Interface</th><th>Size</th><th>Serial</th></tr></thead><tbody>{diskRows}</tbody></table>\n" +
               $"<h2>Performance Summary</h2>\n" +
               $"<p><strong>Current CPU:</strong> {info.CpuUsagePercent:F1}%<br><strong>Current RAM usage:</strong> {info.RamUsagePercent:F1}%<br><strong>Uptime:</strong> {info.UptimeHours:F1} hours</p>\n" +
               $"<h2>Recommendation</h2>\n" +
               $"<p><strong>Verdict:</strong> {upgradeReport.Verdict}<br><strong>Priority:</strong> {upgradeReport.Priority}<br><strong>Summary:</strong> {upgradeReport.Summary}</p>\n" +
               $"<h2>Evidence</h2>\n" +
               $"<ul>{string.Join("", upgradeReport.EvidencePoints.Select(point => $"<li>{point}</li>"))}</ul>\n" +
               $"<h2>Suggested Actions</h2>\n" +
               $"<ul>{string.Join("", upgradeReport.SuggestedActions.Select(action => $"<li>{action}</li>"))}</ul>\n" +
               $"</body></html>";
    }

    private void ShowResult(HardwareInfo info, SupportReport report, UpgradeRecommendation upgradeReport, string requestId)
    {
        string disks = info.Disks.Count > 0
            ? string.Join(
                Environment.NewLine,
                info.Disks.Select(disk =>
                    $"Model      : {disk.Model}{Environment.NewLine}" +
                    $"Media Type : {disk.MediaType}{Environment.NewLine}" +
                    $"Size       : {disk.SizeGB:F1} GB"))
            : "ไม่มีข้อมูลดิสก์ทางกายภาพ";

        var evidenceLines = string.Join(
            Environment.NewLine,
            report.EvidencePoints.Select(point => $"- {point}"));

        var actionLines = string.Join(
            Environment.NewLine,
            report.SuggestedActions.Select(action => $"- {action}"));

        var upgradeEvidenceLines = string.Join(
            Environment.NewLine,
            upgradeReport.EvidencePoints.Select(point => $"- {point}"));

        var upgradeActionLines = string.Join(
            Environment.NewLine,
            upgradeReport.SuggestedActions.Select(action => $"- {action}"));

        var processSummaryLines = info.Processes.Take(10)
            .Select(p => $"{p.DisplayName} ({p.ProcessName}) PID {p.ProcessId} - RAM {p.MemoryMB:F1} MB, CPU {p.CpuUsagePercent:F1}%")
            .ToList();

        var processSummary = processSummaryLines.Any()
            ? string.Join(Environment.NewLine, processSummaryLines)
            : "ไม่พบข้อมูลโปรเซสที่ใช้งานหนักในขณะตรวจสอบ";

        var approvalCriteriaText = """
เกณฑ์การอนุมัติที่ใช้
- คะแนนรวม 55 ขึ้นไปและมีหลักฐานชัดเจน: พิจารณาอนุมัติ
- คำร้องขอเกี่ยวกับ RAM และ RAM < 16 GB พร้อมมีอาการช้า/งานหนัก: อนุมัติพิจารณาเพิ่ม RAM
- คำร้องขอเกี่ยวกับ SSD/HDD และพื้นที่ดิสก์ว่าง < 20 GB พร้อมมีอาการช้า: อนุมัติพิจารณา SSD
- คะแนนต่ำกว่าเกณฑ์หรือข้อมูลไม่เพียงพอ: ขอข้อมูลเพิ่มเติมก่อนอนุมัติ
""";

        txtResult.Text = $"""
========================================
        ผลลัพธ์การตรวจสอบฮาร์ดแวร์
========================================

Request ID / Ticket Number
{requestId}

ชื่อเครื่อง
{info.ComputerName}

ผู้ใช้งาน
{info.UserName}

----------------------------------------
คอมพิวเตอร์
----------------------------------------

ผู้ผลิต
{info.Manufacturer}

รุ่น
{info.Model}

หมายเลขซีเรียล
{info.SerialNumber}

----------------------------------------
ระบบปฏิบัติการ
----------------------------------------

ระบบปฏิบัติการ
{info.OperatingSystem}

เวอร์ชัน Windows
{info.WindowsVersion}

----------------------------------------
CPU
----------------------------------------

CPU
{info.Cpu}

จำนวน Core
{info.CpuCores}

จำนวน Logical Processor
{info.LogicalProcessors}

การใช้ CPU ตอนนี้
{info.CpuUsagePercent:F1} %

----------------------------------------
หน่วยความจำ
----------------------------------------

RAM ทั้งหมด
{info.TotalRamGB:F1} GB

RAM ที่เหลือ
{info.AvailableRamGB:F1} GB

การใช้ RAM
{info.RamUsagePercent:F1} %

----------------------------------------
ดิสก์ระบบ
----------------------------------------

ทั้งหมด
{info.DiskTotalGB:F1} GB

ว่าง
{info.DiskFreeGB:F1} GB

เปอร์เซ็นต์ที่ว่าง
{info.DiskFreePercent:F1} %

----------------------------------------
ดิสก์ทางกายภาพ
----------------------------------------

{disks}
----------------------------------------
รายงานสนับสนุนจากหลักฐาน
----------------------------------------

สรุป
{report.Summary}

ระดับคำแนะนำ
{report.RecommendationLevel}

เหตุผล
{report.RecommendationReason}

หลักฐาน
{evidenceLines}

ข้อแนะนำต่อไป
{actionLines}


{report.AiPrompt}

----------------------------------------
คำร้องขอ Upgrade จากผู้ใช้
----------------------------------------

ผลการวิเคราะห์
{upgradeReport.Verdict}

คะแนนพิจารณา
{upgradeReport.Score} ({upgradeReport.ScoreLabel})

ระดับความสำคัญ
{upgradeReport.Priority}

สรุป
{upgradeReport.Summary}

ฮาร์ดแวร์ที่แนะนำ
{upgradeReport.RecommendedHardware}

หลักฐาน
{upgradeEvidenceLines}

ข้อแนะนำต่อไป
{upgradeActionLines}

เกณฑ์การอนุมัติ
{approvalCriteriaText}

Prompt สำหรับ AI / Support Review
{upgradeReport.AiPrompt}

----------------------------------------
โปรเซสที่ใช้งานหน่วยความจำและ CPU สูง
{processSummary}

----------------------------------------
ระบบ
----------------------------------------

เวลาทำงานต่อเนื่อง
{info.UptimeHours:F1} ชั่วโมง

========================================
""";
    }

    private void btnExportTxt_Click(object? sender, EventArgs e)
    {
        try
        {
            using var dialog = new SaveFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt";
            dialog.FileName = $"HardwareDiagnostic_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, txtResult.Text);
                MessageBox.Show($"ส่งออกรายงานเรียบร้อยแล้ว\n{dialog.FileName}", "Export completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Export failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void txtApprovalCriteria_TextChanged(object sender, EventArgs e)
    {

    }
}

