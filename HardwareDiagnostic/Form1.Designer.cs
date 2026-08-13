namespace HardwareDiagnostic;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblSubtitle;

    private System.Windows.Forms.GroupBox grpComputer;
    private System.Windows.Forms.Label lblComputerName;
    private System.Windows.Forms.Label lblUserName;
    private System.Windows.Forms.Label lblComputerValue;
    private System.Windows.Forms.Label lblUserValue;

    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.Button btnGenerateReport;
    private System.Windows.Forms.Button btnExportJson;
    private System.Windows.Forms.Label lblElapsedTime;
    private System.Windows.Forms.Label lblRecommendation;
    private System.Windows.Forms.Timer timerElapsed;

    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.ProgressBar progressBar;

    private System.Windows.Forms.GroupBox grpResult;
    private System.Windows.Forms.TextBox txtResult;
    private System.Windows.Forms.Label lblRequestReason;
    private System.Windows.Forms.TextBox txtRequestReason;
    private System.Windows.Forms.Label lblRequestId;
    private System.Windows.Forms.TextBox txtRequestId;
    private System.Windows.Forms.Label lblApprovalCriteria;
    private System.Windows.Forms.TextBox txtApprovalCriteria;
    private System.Windows.Forms.Button btnExportTxt;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();

        this.lblTitle = new System.Windows.Forms.Label();
        this.lblSubtitle = new System.Windows.Forms.Label();

        this.grpComputer = new System.Windows.Forms.GroupBox();
        this.lblComputerName = new System.Windows.Forms.Label();
        this.lblUserName = new System.Windows.Forms.Label();
        this.lblComputerValue = new System.Windows.Forms.Label();
        this.lblUserValue = new System.Windows.Forms.Label();

        this.btnStart = new System.Windows.Forms.Button();
        this.btnStop = new System.Windows.Forms.Button();
        this.btnGenerateReport = new System.Windows.Forms.Button();
        this.btnExportJson = new System.Windows.Forms.Button();
        this.lblElapsedTime = new System.Windows.Forms.Label();
        this.lblRecommendation = new System.Windows.Forms.Label();
        this.timerElapsed = new System.Windows.Forms.Timer(this.components);

        this.lblStatus = new System.Windows.Forms.Label();
        this.progressBar = new System.Windows.Forms.ProgressBar();

        this.grpResult = new System.Windows.Forms.GroupBox();
        this.txtResult = new System.Windows.Forms.TextBox();
        this.lblRequestReason = new System.Windows.Forms.Label();
        this.txtRequestReason = new System.Windows.Forms.TextBox();
        this.lblRequestId = new System.Windows.Forms.Label();
        this.txtRequestId = new System.Windows.Forms.TextBox();
        this.lblApprovalCriteria = new System.Windows.Forms.Label();
        this.txtApprovalCriteria = new System.Windows.Forms.TextBox();
        this.btnExportTxt = new System.Windows.Forms.Button();

        this.grpComputer.SuspendLayout();
        this.grpResult.SuspendLayout();
        this.SuspendLayout();

        // ==========================================
        // FORM
        // ==========================================

        this.AutoScaleMode =
            System.Windows.Forms.AutoScaleMode.Font;

        this.ClientSize =
            new System.Drawing.Size(900, 860);

        this.MinimumSize =
            new System.Drawing.Size(900, 860);

        this.StartPosition =
            System.Windows.Forms.FormStartPosition.CenterScreen;

        this.Text =
            "AIS SERVICECENTER";

        // ==========================================
        // TITLE
        // ==========================================

        this.lblTitle.AutoSize = true;

        this.lblTitle.Font =
            new System.Drawing.Font(
                "Segoe UI",
                20F,
                System.Drawing.FontStyle.Bold);

        this.lblTitle.Location =
            new System.Drawing.Point(30, 25);

        this.lblTitle.Text =
            "AIS SERVICECENTER DIAGNOSTIC TOOL";

        // ==========================================
        // SUBTITLE
        // ==========================================

        this.lblSubtitle.AutoSize = true;

        this.lblSubtitle.Font =
            new System.Drawing.Font(
                "Segoe UI",
                10F);

        this.lblSubtitle.ForeColor =
            System.Drawing.Color.DimGray;

        this.lblSubtitle.Location =
            new System.Drawing.Point(34, 65);

        this.lblSubtitle.Text =
            "Computer Performance & Hardware Assessment";

        // ==========================================
        // COMPUTER INFORMATION
        // ==========================================

        this.grpComputer.Location =
            new System.Drawing.Point(30, 105);

        this.grpComputer.Size =
            new System.Drawing.Size(840, 105);

        this.grpComputer.Text =
            "Computer Information";

        // Computer Name label

        this.lblComputerName.AutoSize = true;

        this.lblComputerName.Location =
            new System.Drawing.Point(20, 30);

        this.lblComputerName.Text =
            "Computer Name:";

        // Computer Name value

        this.lblComputerValue.AutoSize = true;

        this.lblComputerValue.Location =
            new System.Drawing.Point(150, 30);

        this.lblComputerValue.Font =
            new System.Drawing.Font(
                "Segoe UI",
                9F,
                System.Drawing.FontStyle.Bold);

        this.lblComputerValue.Text =
            Environment.MachineName;

        // User label

        this.lblUserName.AutoSize = true;

        this.lblUserName.Location =
            new System.Drawing.Point(20, 65);

        this.lblUserName.Text =
            "User:";

        // User value

        this.lblUserValue.AutoSize = true;

        this.lblUserValue.Location =
            new System.Drawing.Point(150, 65);

        this.lblUserValue.Font =
            new System.Drawing.Font(
                "Segoe UI",
                9F,
                System.Drawing.FontStyle.Bold);

        this.lblUserValue.Text =
            Environment.UserName;

        // Add computer controls

        this.grpComputer.Controls.Add(
            this.lblComputerName);

        this.grpComputer.Controls.Add(
            this.lblComputerValue);

        this.grpComputer.Controls.Add(
            this.lblUserName);

        this.grpComputer.Controls.Add(
            this.lblUserValue);

        // ==========================================
        // START BUTTON
        // ==========================================

        this.btnStart.Location =
            new System.Drawing.Point(30, 230);

        this.btnStart.Size =
            new System.Drawing.Size(230, 55);

        this.btnStart.Text =
            "START DIAGNOSTIC";

        this.btnStart.Font =
            new System.Drawing.Font(
                "Segoe UI",
                11F,
                System.Drawing.FontStyle.Bold);

        this.btnStart.Cursor =
            System.Windows.Forms.Cursors.Hand;

        this.btnStart.Click +=
            new System.EventHandler(
                this.btnStart_Click);

        // ==========================================
        // STOP BUTTON
        // ==========================================

        this.btnStop = new System.Windows.Forms.Button();
        this.btnStop.Location =
            new System.Drawing.Point(280, 230);

        this.btnStop.Size =
            new System.Drawing.Size(180, 55);

        this.btnStop.Text =
            "STOP";

        this.btnStop.Font =
            new System.Drawing.Font(
                "Segoe UI",
                11F,
                System.Drawing.FontStyle.Bold);

        this.btnStop.Cursor =
            System.Windows.Forms.Cursors.Hand;

        this.btnStop.Enabled = false;
        this.btnStop.Click +=
            new System.EventHandler(
                this.btnStop_Click);

        // ==========================================
        // GENERATE REPORT
        // ==========================================

        this.btnGenerateReport = new System.Windows.Forms.Button();
        this.btnGenerateReport.Location =
            new System.Drawing.Point(480, 230);

        this.btnGenerateReport.Size =
            new System.Drawing.Size(210, 55);

        this.btnGenerateReport.Text =
            "GENERATE REPORT";

        this.btnGenerateReport.Font =
            new System.Drawing.Font(
                "Segoe UI",
                11F,
                System.Drawing.FontStyle.Bold);

        this.btnGenerateReport.Cursor =
            System.Windows.Forms.Cursors.Hand;

        this.btnGenerateReport.Enabled = false;
        this.btnGenerateReport.Click +=
            new System.EventHandler(
                this.btnGenerateReport_Click);

        // ==========================================
        // EXPORT JSON
        // ==========================================

        this.btnExportJson = new System.Windows.Forms.Button();
        this.btnExportJson.Location =
            new System.Drawing.Point(710, 230);

        this.btnExportJson.Size =
            new System.Drawing.Size(160, 55);

        this.btnExportJson.Text =
            "EXPORT JSON";

        this.btnExportJson.Font =
            new System.Drawing.Font(
                "Segoe UI",
                11F,
                System.Drawing.FontStyle.Bold);

        this.btnExportJson.Cursor =
            System.Windows.Forms.Cursors.Hand;

        this.btnExportJson.Enabled = false;
        this.btnExportJson.Click +=
            new System.EventHandler(
                this.btnExportJson_Click);

        // ==========================================
        // STATUS
        // ==========================================

        this.lblStatus.AutoSize = true;

        this.lblStatus.Location =
            new System.Drawing.Point(285, 248);

        this.lblStatus.Font =
            new System.Drawing.Font(
                "Segoe UI",
                10F);

        this.lblStatus.Text =
            "Status: Ready";

        // ==========================================
        // ELAPSED TIME
        // ==========================================

        this.lblElapsedTime = new System.Windows.Forms.Label();
        this.lblElapsedTime.AutoSize = true;
        this.lblElapsedTime.Location =
            new System.Drawing.Point(30, 320);
        this.lblElapsedTime.Font =
            new System.Drawing.Font(
                "Segoe UI",
                10F);
        this.lblElapsedTime.Text =
            "Elapsed Time: 00:00:00";

        // ==========================================
        // RECOMMENDATION
        // ==========================================

        this.lblRecommendation = new System.Windows.Forms.Label();
        this.lblRecommendation.AutoSize = true;
        this.lblRecommendation.Location =
            new System.Drawing.Point(280, 320);
        this.lblRecommendation.Font =
            new System.Drawing.Font(
                "Segoe UI",
                10F,
                System.Drawing.FontStyle.Bold);
        this.lblRecommendation.Text =
            "Recommendation: N/A";

        // ==========================================
        // TIMER
        // ==========================================

        this.timerElapsed.Enabled = false;
        this.timerElapsed.Interval = 1000;
        this.timerElapsed.Tick +=
            new System.EventHandler(
                this.TimerElapsed_Tick);

        // ==========================================
        // PROGRESS BAR
        // ==========================================

        this.progressBar.Location =
            new System.Drawing.Point(30, 350);

        this.progressBar.Size =
            new System.Drawing.Size(840, 25);

        this.progressBar.Minimum = 0;

        this.progressBar.Maximum = 100;

        this.progressBar.Value = 0;

        // ==========================================
        // RESULT GROUP
        // ==========================================

        this.lblRequestId.AutoSize = true;
        this.lblRequestId.Location =
            new System.Drawing.Point(30, 345);
        this.lblRequestId.Text =
            "Request ID / Ticket Number:";
        this.lblRequestId.Font =
            new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

        this.txtRequestId.Location =
            new System.Drawing.Point(30, 372);
        this.txtRequestId.Size =
            new System.Drawing.Size(250, 25);
        this.txtRequestId.Font =
            new System.Drawing.Font("Segoe UI", 10F);

        this.lblRequestReason.AutoSize = true;
        this.lblRequestReason.Location =
            new System.Drawing.Point(30, 410);
        this.lblRequestReason.Text =
            "เหตุผลคำร้องขอ Upgrade:";
        this.lblRequestReason.Font =
            new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

        this.txtRequestReason.Location =
            new System.Drawing.Point(30, 437);
        this.txtRequestReason.Size =
            new System.Drawing.Size(840, 75);
        this.txtRequestReason.Multiline = true;
        this.txtRequestReason.ScrollBars =
            System.Windows.Forms.ScrollBars.Vertical;
        this.txtRequestReason.Font =
            new System.Drawing.Font("Segoe UI", 10F);
        this.txtRequestReason.Text =
            "ตัวอย่าง: เครื่องค้างและช้าเมื่อใช้งาน After Effect, ต้องการเพิ่ม RAM และเปลี่ยน HDD เป็น SSD M.2 สำหรับงานพัฒนาและทดสอบ";

        this.lblApprovalCriteria.AutoSize = true;
        this.lblApprovalCriteria.Location =
            new System.Drawing.Point(30, 525);
        this.lblApprovalCriteria.Text =
            "เกณฑ์การอนุมัติที่ใช้:";
        this.lblApprovalCriteria.Font =
            new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

        this.txtApprovalCriteria.Location =
            new System.Drawing.Point(30, 550);
        this.txtApprovalCriteria.Size =
            new System.Drawing.Size(840, 85);
        this.txtApprovalCriteria.Multiline = true;
        this.txtApprovalCriteria.ScrollBars =
            System.Windows.Forms.ScrollBars.Vertical;
        this.txtApprovalCriteria.ReadOnly = true;
        this.txtApprovalCriteria.BackColor =
            System.Drawing.Color.WhiteSmoke;
        this.txtApprovalCriteria.Font =
            new System.Drawing.Font("Segoe UI", 9F);
        this.txtApprovalCriteria.Text =
            "• คะแนนรวม 55 ขึ้นไปและมีหลักฐานชัดเจน: พิจารณาอนุมัติ\n" +
            "• คำร้องขอเกี่ยวกับ RAM และ RAM < 16 GB พร้อมมีอาการช้า/งานหนัก: อนุมัติพิจารณาเพิ่ม RAM\n" +
            "• คำร้องขอเกี่ยวกับ SSD/HDD และพื้นที่ดิสก์ว่าง < 20 GB พร้อมมีอาการช้า: อนุมัติพิจารณา SSD\n" +
            "• คะแนนต่ำกว่าเกณฑ์หรือข้อมูลไม่เพียงพอ: ขอข้อมูลเพิ่มเติมก่อนอนุมัติ";

        this.btnExportTxt.Location =
            new System.Drawing.Point(30, 650);
        this.btnExportTxt.Size =
            new System.Drawing.Size(180, 35);
        this.btnExportTxt.Text =
            "Export TXT";
        this.btnExportTxt.Click +=
            new System.EventHandler(this.btnExportTxt_Click);

        this.grpResult.Location =
            new System.Drawing.Point(30, 705);

        this.grpResult.Size =
            new System.Drawing.Size(840, 120);

        this.grpResult.Text =
            "Diagnostic Result";

        // ==========================================
        // RESULT TEXTBOX
        // ==========================================

        this.txtResult.Location =
            new System.Drawing.Point(15, 30);

        this.txtResult.Size =
            new System.Drawing.Size(810, 110);

        this.txtResult.Multiline = true;

        this.txtResult.ScrollBars =
            System.Windows.Forms.ScrollBars.Vertical;

        this.txtResult.ReadOnly = true;

        this.txtResult.BackColor =
            System.Drawing.Color.White;

        this.txtResult.Font =
            new System.Drawing.Font(
                "Consolas",
                10F);

        this.txtResult.Text =
            "รอการตรวจสอบ...\r\n\r\n" +
            "กด START DIAGNOSTIC เพื่อเริ่มตรวจสอบเครื่อง";

        // Add result textbox

        this.grpResult.Controls.Add(
            this.txtResult);

        // ==========================================
        // ADD CONTROLS TO FORM
        // ==========================================

        this.Controls.Add(
            this.lblTitle);

        this.Controls.Add(
            this.lblSubtitle);

        this.Controls.Add(
            this.grpComputer);

        this.Controls.Add(
            this.btnStart);

        this.Controls.Add(
            this.lblStatus);

        this.Controls.Add(
            this.lblElapsedTime);

        this.Controls.Add(
            this.lblRecommendation);

        this.Controls.Add(
            this.progressBar);

        this.Controls.Add(
            this.btnStop);

        this.Controls.Add(
            this.btnGenerateReport);

        this.Controls.Add(
            this.btnExportJson);

        this.Controls.Add(
            this.lblRequestId);

        this.Controls.Add(
            this.txtRequestId);

        this.Controls.Add(
            this.lblRequestReason);

        this.Controls.Add(
            this.txtRequestReason);

        this.Controls.Add(
            this.lblApprovalCriteria);

        this.Controls.Add(
            this.txtApprovalCriteria);

        this.Controls.Add(
            this.btnExportTxt);

        this.Controls.Add(
            this.grpResult);

        // ==========================================

        this.grpComputer.ResumeLayout(false);
        this.grpComputer.PerformLayout();

        this.grpResult.ResumeLayout(false);
        this.grpResult.PerformLayout();

        this.ResumeLayout(false);
        this.PerformLayout();

        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion
}