namespace HardwareDiagnostic.Models;

public class UpgradeRecommendation
{
    public string Verdict { get; set; } = "";
    public string ResultLevel { get; set; } = "";
    public string Priority { get; set; } = "";
    public string Summary { get; set; } = "";
    public string RecommendedHardware { get; set; } = "";
    public int Score { get; set; }
    public string ScoreLabel { get; set; } = "";
    public List<string> EvidencePoints { get; set; } = new();
    public List<string> SuggestedActions { get; set; } = new();
    public string AiPrompt { get; set; } = "";
}
