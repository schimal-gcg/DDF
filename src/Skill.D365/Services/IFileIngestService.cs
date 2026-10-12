namespace Skill.D365.Services
{
    public interface IFileIngestService
    {
        Task<string> ExtractTextAsync(Microsoft.AspNetCore.Http.IFormFile file);
    }
}
