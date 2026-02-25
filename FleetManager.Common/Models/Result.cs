using System.Net;

namespace FleetManager.Common.Models;

public class Result<T>() where T : class?
{
    public required bool Success { get; set; }
    public required HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}
