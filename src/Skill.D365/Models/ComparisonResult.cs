using System.Text.Json.Serialization;

namespace Skill.D365.Models
{
    public class ComparisonResult
    {
        public string ProjectName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int MatchScore { get; set; }
        public string RequirementFile { get; set; } = string.Empty;
        public string? ScriptFile { get; set; }
        public Summary Summary { get; set; } = new Summary();
        public List<ComparisonItem> Comparison { get; set; } = new List<ComparisonItem>();
        public List<AotSuggestion> AotSuggestions { get; set; } = new List<AotSuggestion>();
        public List<string> Risks { get; set; } = new List<string>();
        public string Conclusion { get; set; } = string.Empty;
    }

    public class Summary
    {
        public int RequirementsDetected { get; set; }
        public int TestCasesDetected { get; set; }
        public int MatchedRequirements { get; set; }
        public int MissingRequirements { get; set; }
        public int MismatchedRequirements { get; set; }
    }

    public class ComparisonItem
    {
        public string RequirementId { get; set; } = string.Empty;
        public string RequirementText { get; set; } = string.Empty;
        public string? TestCaseId { get; set; }
        public string? TestCaseText { get; set; }
        public string Result { get; set; } = string.Empty;
        public string Evidence { get; set; } = string.Empty;
    }

    public class AotSuggestion
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}
