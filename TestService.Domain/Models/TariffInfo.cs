using System.Text.Json.Serialization;

namespace TestService.Services.Implementation;

public class TariffInfo
{
    [JsonPropertyName("tariff_code")]
    public required int Code { get; set; }
    [JsonPropertyName("tariff_name")]
    public required string Name { get; set; }
    [JsonPropertyName("tariff_description")]
    public required string Description { get; set; }
    [JsonPropertyName("delivery_sum")]
    public required float Cost { get; set; }
    [JsonPropertyName("calendar_min")]
    public required int MinDeliveryTime { get; set; }
    [JsonPropertyName("calendar_max")]
    public required int MaxDeliveryTime { get; set; }
}