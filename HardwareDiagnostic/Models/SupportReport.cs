namespace HardwareDiagnostic.Models;

public class SupportReport
{
    public string Summary { get; set; } = "";

    public string RecommendationLevel { get; set; } = "";

    public string RecommendationReason { get; set; } = "";

    public List<string> EvidencePoints { get; set; } = new();

    public List<string> SuggestedActions { get; set; } = new();

    public string AiPrompt { get; set; } = "";
}
