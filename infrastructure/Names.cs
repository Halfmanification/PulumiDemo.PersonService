namespace PulumiDemo.PersonService.Infrastructure;

public record Names(string ProjectName, string ProjectShortName, string Environment)
{
    public string ResourceGroup => $"{SanitizeResourceName(ProjectName)}-{Environment}-rg";
    public string StorageAccount => $"{SanitizeResourceName(ProjectShortName)}{Environment}sa";

    private static string SanitizeResourceName(string name) => name
        .ToLowerInvariant()
        .Replace(".", "-")
        .Trim();
}