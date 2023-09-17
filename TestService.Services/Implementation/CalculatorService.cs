using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using TestService.Domain.Exceptions;
using TestService.Services.Interfaces;

namespace TestService.Services.Implementation;

public class CalculatorService : ICalculatorService
{
    private readonly string _account = "EMscd6r9JnFiQ3bLoyjJY6eM78JrJceI";
    private readonly string _securePassword = "PjLZkKBHEiLK3YsjtNrt3TGNG0ahs3kG";
    
    public async Task<IEnumerable<TariffInfo>> CalculateDeliveryCostAsync(string senderCityFias, string receiverCityFias, float weight, int length,
        int width, int height)
    {
        var lengthSm = length / 10;
        var widthSm = width / 10;
        var heightSm = height / 10;

        var senderCityCode = await GetCityCodeByFias(senderCityFias);
        var receiverCityCode = await GetCityCodeByFias(senderCityFias);
        
        using var httpClient = new HttpClient();

        var calculationRequestBody = new CalculationRequest
        {
            From = new Location
            {
                Code = senderCityCode
            },
            To = new Location
            {
                Code = receiverCityCode
            },
            Packages = new []
            {
                new GoodsData
                {
                    Weight = weight,
                    Length = lengthSm,
                    Width = widthSm,
                    Height = heightSm
                }
            }
        };

        var accessToken = await GetAccessTokenAsync();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://api.edu.cdek.ru/v2/calculator/tarifflist"),
            Content = JsonContent.Create(calculationRequestBody, mediaType: null, options: null)
        };

        request.Headers.Add("Authorization", $"Bearer {accessToken}");
        
        var response = await httpClient.SendAsync(request);
        var availableTariffList = await response.Content.ReadFromJsonAsync<GetTariffResponse>();
        if (availableTariffList is null)
        {
            throw new NotImplementedException("Available tariff list is null");
        }
        
        return availableTariffList.Tariffs;
    }

    private async Task<int> GetCityCodeByFias(string fias)
    {
        using var httpClient = new HttpClient();
        
        var accessToken = await GetAccessTokenAsync();
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://api.edu.cdek.ru/v2/location/cities/??country_codes=RU")
        };
        
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            // TODO: Add logic
            throw new NotImplementedException($"Get city by fias_guid = {fias} return error");
        }

        var cityInfoList = await response.Content.ReadFromJsonAsync<CityData[]>();
        if (cityInfoList is null)
        {
            // TODO: Add logic
            throw new NotImplementedException("Get city response body is null");
        }

        if (!cityInfoList.Any())
        {
            throw new CityNotFoundException(fias);
        }
        
        return cityInfoList.First().Code;
    }
    
    private async Task<string> GetAccessTokenAsync()
    {
        using var httpClient = new HttpClient();

        var response = await httpClient.PostAsync($"https://api.edu.cdek.ru/v2/oauth/token?client_id={_account}&client_secret={_securePassword}", 
            new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new("client_id", _account),
                new("client_secret", _securePassword),
                new("grant_type", "client_credentials")
            }));

        if (!response.IsSuccessStatusCode)
        {
            // TODO: Add logic
            throw new NotImplementedException();
        }

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (authResponse is null)
        {
            // TODO: Add logic
            throw new NotImplementedException();
        }
        
        return authResponse.AccessToken;
    }
}

file class GetTariffResponse
{
    [JsonPropertyName("tariff_codes")]
    public required IEnumerable<TariffInfo> Tariffs { get; set; }
}

file class CalculationRequest
{
    [JsonPropertyName("from_location")]
    public required Location From { get; set; }
    [JsonPropertyName("to_location")]
    public required Location To { get; set; }
    [JsonPropertyName("packages")]
    public required IEnumerable<GoodsData> Packages { get; set; } 
}

file class GoodsData
{
    [JsonPropertyName("weight")]
    public required float Weight { get; set; }
    [JsonPropertyName("length")]
    public required int Length { get; set; }
    [JsonPropertyName("width")]
    public required int Width { get; set; }
    [JsonPropertyName("height")]
    public required int Height { get; set; }
}

file class Location
{
    public required int Code { get; set; }
}

file class CityData
{
    [JsonPropertyName("code")]
    public required int Code { get; set; }
}

file class AuthResponse
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }
}