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
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        lblTitle = new Label();
        lblSubtitle = new Label();
        grpComputer = new GroupBox();
        lblComputerName = new Label();
        lblComputerValue = new Label();
        lblUserName = new Label();
        lblUserValue = new Label();
        btnStart = new Button();
        btnStop = new Button();
        btnGenerateReport = new Button();
        btnExportJson = new Button();
        lblElapsedTime = new Label();
        lblRecommendation = new Label();
        timerElapsed = new System.Windows.Forms.Timer(components);
        lblStatus = new Label();
        progressBar = new ProgressBar();
        grpResult = new GroupBox();
        txtResult = new TextBox();
        lblRequestReason = new Label();
        txtRequestReason = new TextBox();
        lblRequestId = new Label();
        txtRequestId = new TextBox();
        lblApprovalCriteria = new Label();
        txtApprovalCriteria = new TextBox();
        btnExportTxt = new Button();
        grpComputer.SuspendLayout();
        grpResult.SuspendLayout();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.White;
        lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblTitle.ForeColor = Color.ForestGreen;
        lblTitle.Location = new Point(30, 25);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(655, 46);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "AIS SERVICECENTER DIAGNOSTIC TOOL";
        // 
        // lblSubtitle
        // 
        lblSubtitle.AutoSize = true;
        lblSubtitle.Font = new Font("Segoe UI", 10F);
        lblSubtitle.ForeColor = Color.DimGray;
        lblSubtitle.Location = new Point(34, 65);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(364, 23);
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Text = "Computer Performance & Hardware Assessment";
        // 
        // grpComputer
        // 
        grpComputer.Controls.Add(lblComputerName);
        grpComputer.Controls.Add(lblComputerValue);
        grpComputer.Controls.Add(lblUserName);
        grpComputer.Controls.Add(lblUserValue);
        grpComputer.Location = new Point(30, 105);
        grpComputer.Name = "grpComputer";
        grpComputer.Size = new Size(840, 105);
        grpComputer.TabIndex = 2;
        grpComputer.TabStop = false;
        grpComputer.Text = "Computer Information";
        // 
        // lblComputerName
        // 
        lblComputerName.AutoSize = true;
        lblComputerName.Location = new Point(20, 30);
        lblComputerName.Name = "lblComputerName";
        lblComputerName.Size = new Size(129, 21);
        lblComputerName.TabIndex = 0;
        lblComputerName.Text = "Computer Name:";
        // 
        // lblComputerValue
        // 
        lblComputerValue.AutoSize = true;
        lblComputerValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblComputerValue.Location = new Point(150, 30);
        lblComputerValue.Name = "lblComputerValue";
        lblComputerValue.Size = new Size(119, 20);
        lblComputerValue.TabIndex = 1;
        lblComputerValue.Text = "CPADL2010194";
        // 
        // lblUserName
        // 
        lblUserName.AutoSize = true;
        lblUserName.Location = new Point(20, 65);
        lblUserName.Name = "lblUserName";
        lblUserName.Size = new Size(45, 21);
        lblUserName.TabIndex = 2;
        lblUserName.Text = "User:";
        // 
        // lblUserValue
        // 
        lblUserValue.AutoSize = true;
        lblUserValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblUserValue.Location = new Point(150, 65);
        lblUserValue.Name = "lblUserValue";
        lblUserValue.Size = new Size(74, 20);
        lblUserValue.TabIndex = 3;
        lblUserValue.Text = "sakdas45";
        // 
        // btnStart
        // 
        btnStart.BackColor = Color.FromArgb(192, 64, 0);
        btnStart.Cursor = Cursors.Hand;
        btnStart.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnStart.Location = new Point(30, 230);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(230, 55);
        btnStart.TabIndex = 3;
        btnStart.Text = "START DIAGNOSTIC";
        btnStart.UseVisualStyleBackColor = false;
        btnStart.Click += btnStart_Click;
        // 
        // btnStop
        // 
        btnStop.BackColor = Color.FromArgb(0, 192, 192);
        btnStop.Cursor = Cursors.Hand;
        btnStop.Enabled = false;
        btnStop.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnStop.Location = new Point(280, 230);
        btnStop.Name = "btnStop";
        btnStop.Size = new Size(180, 55);
        btnStop.TabIndex = 8;
        btnStop.Text = "STOP";
        btnStop.UseVisualStyleBackColor = false;
        btnStop.Click += btnStop_Click;
        // 
        // btnGenerateReport
        // 
        btnGenerateReport.BackColor = Color.FromArgb(0, 192, 192);
        btnGenerateReport.Cursor = Cursors.Hand;
        btnGenerateReport.Enabled = false;
        btnGenerateReport.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnGenerateReport.Location = new Point(480, 230);
        btnGenerateReport.Name = "btnGenerateReport";
        btnGenerateReport.Size = new Size(210, 55);
        btnGenerateReport.TabIndex = 9;
        btnGenerateReport.Text = "ส่งออกข้อมูล";
        btnGenerateReport.UseVisualStyleBackColor = false;
        btnGenerateReport.Click += btnGenerateReport_Click;
        // 
        // btnExportJson
        // 
        btnExportJson.BackColor = Color.FromArgb(0, 192, 192);
        btnExportJson.Cursor = Cursors.Hand;
        btnExportJson.Enabled = false;
        btnExportJson.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnExportJson.Location = new Point(710, 230);
        btnExportJson.Name = "btnExportJson";
        btnExportJson.Size = new Size(160, 55);
        btnExportJson.TabIndex = 10;
        btnExportJson.Text = "EXPORT JSON";
        btnExportJson.UseVisualStyleBackColor = false;
        btnExportJson.Click += btnExportJson_Click;
        // 
        // lblElapsedTime
        // 
        lblElapsedTime.AutoSize = true;
        lblElapsedTime.Font = new Font("Segoe UI", 10F);
        lblElapsedTime.Location = new Point(30, 320);
        lblElapsedTime.Name = "lblElapsedTime";
        lblElapsedTime.Size = new Size(181, 23);
        lblElapsedTime.TabIndex = 5;
        lblElapsedTime.Text = "Elapsed Time: 00:00:00";
        // 
        // lblRecommendation
        // 
        lblRecommendation.AutoSize = true;
        lblRecommendation.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblRecommendation.Location = new Point(280, 320);
        lblRecommendation.Name = "lblRecommendation";
        lblRecommendation.Size = new Size(194, 23);
        lblRecommendation.TabIndex = 6;
        lblRecommendation.Text = "Recommendation: N/A";
        // 
        // timerElapsed
        // 
        timerElapsed.Interval = 1000;
        timerElapsed.Tick += TimerElapsed_Tick;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Segoe UI", 10F);
        lblStatus.Location = new Point(285, 248);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(111, 23);
        lblStatus.TabIndex = 4;
        lblStatus.Text = "Status: Ready";
        // 
        // progressBar
        // 
        progressBar.Location = new Point(30, 350);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(840, 25);
        progressBar.TabIndex = 7;
        // 
        // grpResult
        // 
        grpResult.Controls.Add(txtResult);
        grpResult.Location = new Point(30, 705);
        grpResult.Name = "grpResult";
        grpResult.Size = new Size(840, 120);
        grpResult.TabIndex = 18;
        grpResult.TabStop = false;
        grpResult.Text = "Diagnostic Result";
        // 
        // txtResult
        // 
        txtResult.BackColor = Color.White;
        txtResult.Font = new Font("Consolas", 10F);
        txtResult.Location = new Point(15, 30);
        txtResult.Multiline = true;
        txtResult.Name = "txtResult";
        txtResult.ReadOnly = true;
        txtResult.ScrollBars = ScrollBars.Vertical;
        txtResult.Size = new Size(810, 110);
        txtResult.TabIndex = 0;
        txtResult.Text = "รอการตรวจสอบ...\r\n\r\nกด START DIAGNOSTIC เพื่อเริ่มตรวจสอบเครื่อง";
        // 
        // lblRequestReason
        // 
        lblRequestReason.AutoSize = true;
        lblRequestReason.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblRequestReason.Location = new Point(30, 410);
        lblRequestReason.Name = "lblRequestReason";
        lblRequestReason.Size = new Size(210, 23);
        lblRequestReason.TabIndex = 13;
        lblRequestReason.Text = "เหตุผลคำร้องขอ Upgrade:";
        // 
        // txtRequestReason
        // 
        txtRequestReason.BackColor = SystemColors.Menu;
        txtRequestReason.Font = new Font("Segoe UI", 10F);
        txtRequestReason.Location = new Point(30, 437);
        txtRequestReason.Multiline = true;
        txtRequestReason.Name = "txtRequestReason";
        txtRequestReason.ScrollBars = ScrollBars.Vertical;
        txtRequestReason.Size = new Size(840, 75);
        txtRequestReason.TabIndex = 14;
        txtRequestReason.Text = "กรอกเหตุผลในการขอเพิ่ม RAM";
        txtRequestReason.TextChanged += txtRequestReason_TextChanged;
        // 
        // lblRequestId
        // 
        lblRequestId.AutoSize = true;
        lblRequestId.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblRequestId.Location = new Point(30, 345);
        lblRequestId.Name = "lblRequestId";
        lblRequestId.Size = new Size(241, 23);
        lblRequestId.TabIndex = 11;
        lblRequestId.Text = "Request ID / Ticket Number:";
        // 
        // txtRequestId
        // 
        txtRequestId.Font = new Font("Segoe UI", 10F);
        txtRequestId.Location = new Point(30, 372);
        txtRequestId.Name = "txtRequestId";
        txtRequestId.Size = new Size(250, 30);
        txtRequestId.TabIndex = 12;
        // 
        // lblApprovalCriteria
        // 
        lblApprovalCriteria.AutoSize = true;
        lblApprovalCriteria.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblApprovalCriteria.Location = new Point(30, 525);
        lblApprovalCriteria.Name = "lblApprovalCriteria";
        lblApprovalCriteria.Size = new Size(168, 23);
        lblApprovalCriteria.TabIndex = 15;
        lblApprovalCriteria.Text = "เกณฑ์การอนุมัติที่ใช้:";
        // 
        // txtApprovalCriteria
        // 
        txtApprovalCriteria.BackColor = Color.WhiteSmoke;
        txtApprovalCriteria.Font = new Font("Segoe UI", 9F);
        txtApprovalCriteria.Location = new Point(30, 550);
        txtApprovalCriteria.Multiline = true;
        txtApprovalCriteria.Name = "txtApprovalCriteria";
        txtApprovalCriteria.ReadOnly = true;
        txtApprovalCriteria.ScrollBars = ScrollBars.Vertical;
        txtApprovalCriteria.Size = new Size(840, 85);
        txtApprovalCriteria.TabIndex = 16;
        txtApprovalCriteria.Text = resources.GetString("txtApprovalCriteria.Text");
        txtApprovalCriteria.TextChanged += txtApprovalCriteria_TextChanged;
        // 
        // btnExportTxt
        // 
        btnExportTxt.BackColor = Color.Gray;
        btnExportTxt.Location = new Point(30, 650);
        btnExportTxt.Name = "btnExportTxt";
        btnExportTxt.Size = new Size(180, 35);
        btnExportTxt.TabIndex = 17;
        btnExportTxt.Text = "Export TXT";
        btnExportTxt.UseVisualStyleBackColor = false;
        btnExportTxt.Click += btnExportTxt_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(9F, 21F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(882, 813);
        Controls.Add(lblTitle);
        Controls.Add(lblSubtitle);
        Controls.Add(grpComputer);
        Controls.Add(btnStart);
        Controls.Add(lblStatus);
        Controls.Add(lblElapsedTime);
        Controls.Add(lblRecommendation);
        Controls.Add(progressBar);
        Controls.Add(btnStop);
        Controls.Add(btnGenerateReport);
        Controls.Add(btnExportJson);
        Controls.Add(lblRequestId);
        Controls.Add(txtRequestId);
        Controls.Add(lblRequestReason);
        Controls.Add(txtRequestReason);
        Controls.Add(lblApprovalCriteria);
        Controls.Add(txtApprovalCriteria);
        Controls.Add(btnExportTxt);
        Controls.Add(grpResult);
        MinimumSize = new Size(900, 860);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "AIS SERVICECENTER";
        grpComputer.ResumeLayout(false);
        grpComputer.PerformLayout();
        grpResult.ResumeLayout(false);
        grpResult.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}