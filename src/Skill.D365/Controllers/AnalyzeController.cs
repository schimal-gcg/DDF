using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skill.D365.Models;
using Skill.D365.Services;

namespace Skill.D365.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyzeController : ControllerBase
{
    private readonly IFileIngestService _ingest;

    public AnalyzeController(IFileIngestService ingest)
    {
        _ingest = ingest;
    }

    [HttpPost]
    [RequestSizeLimit(50_000_000)]
    public async Task<IActionResult> PostAnalyze([FromForm] IFormFile requirement, [FromForm] IFormFile? script)
    {
        if (requirement == null)
            return BadRequest("requirement file is required");

        var reqText = await _ingest.ExtractTextAsync(requirement);
        string? scriptText = null;
        if (script != null)
        {
            scriptText = await _ingest.ExtractTextAsync(script);
        }

        // TODO: call extractor, comparator and AOT advisor. For scaffold return example JSON.

        var result = new ComparisonResult
        {
            ProjectName = Path.GetFileNameWithoutExtension(requirement.FileName),
            Status = scriptText == null ? "sin_script_asociado" : "paired_valid",
            MatchScore = scriptText == null ? 0 : 80,
            RequirementFile = requirement.FileName,
            ScriptFile = script?.FileName,
            Summary = new Summary
            {
                RequirementsDetected = 5,
                TestCasesDetected = scriptText == null ? 0 : 12,
                MatchedRequirements = scriptText == null ? 0 : 4,
                MissingRequirements = scriptText == null ? 5 : 1,
                MismatchedRequirements = 0
            },
            Comparison = new List<ComparisonItem>
            {
                new ComparisonItem
                {
                    RequirementId = "REQ-01",
                    RequirementText = "Validar la tasa de cambio en transferencia internacional.",
                    TestCaseId = scriptText == null ? null : "TC-07",
                    TestCaseText = scriptText == null ? null : "Verificar tasa de cambio aplicada al movimiento.",
                    Result = scriptText == null ? "faltante" : "coincide",
                    Evidence = scriptText == null ? "No hay script asociado" : "Mismo concepto y campo"
                }
            },
            AotSuggestions = new List<AotSuggestion>
            {
                new AotSuggestion { Type = "table", Name = "TransferenciasInternacionales", Reason = "Registrar operación, tasa, monto y estado" },
                new AotSuggestion { Type = "class", Name = "TransferenciasInternacionalesService", Reason = "Lógica de validación y cálculo" }
            },
            Risks = new List<string>
            {
                scriptText == null ? "No hay script asociado - imposible comparar" : "Falta cubrir validación de límite máximo"
            },
            Conclusion = scriptText == null ? "No se pudo comparar: falta script" : "Coincidencia parcial"
        };

        return Ok(result);
    }
}
