namespace HardwareDiagnostic.Aisc.Models;

public class ApprovalResult
{
    public bool IsApproved { get; set; }

    public string Decision { get; set; } = "";

    public string Priority { get; set; } = "";

    public int Score { get; set; }

    public List<string> Evidence { get; set; } = new();

    public List<string> Recommendations { get; set; } = new();
}