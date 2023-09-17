using Microsoft.AspNetCore.Mvc;
using TestService.Services.Interfaces;
using TestService.Web.Requests;

namespace TestService.Web.Controllers;

[ApiController]
[Route("api/calculator")]
public class CalculatorController : ControllerBase
{
    private readonly ICalculatorService _calculatorService;

    public CalculatorController(ICalculatorService calculatorService)
    {
        _calculatorService = calculatorService;
    }

    [HttpGet]
    public async Task<IActionResult> Calculate([FromQuery] CalculateRequest request)
    {
        var result = await _calculatorService.CalculateDeliveryCostAsync(request.From,
            request.To, request.Weight, request.Lenght, request.Width, request.Height);
        return Ok(result);
    }
}