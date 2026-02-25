namespace FleetManager.Common.Models;

public record ValidationResult(bool IsValid, string? ValidationMessage = null);