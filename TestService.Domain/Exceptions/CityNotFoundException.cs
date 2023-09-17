namespace TestService.Domain.Exceptions;

public class CityNotFoundException : Exception
{
    public CityNotFoundException(string cityFias) : base($"City with FIAS = {cityFias} not found")
    {
        
    }
}