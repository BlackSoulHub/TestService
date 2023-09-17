namespace TestService.Web.Requests;

public class CalculateRequest
{
    public required string From { get; set; }
    public required string To { get; set; }
    public required float Weight { get; set; }
    public required int Lenght { get; set; }
    public required int Height { get; set; }
    public required int Width { get; set; }
}