using Microsoft.AspNetCore.Mvc;
using TestService.Services.Interfaces;

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

    [HttpGet("")]
    public async Task<IActionResult> Calculate()
    {
        var result = await _calculatorService.CalculateDeliveryCostAsync("1dc8892d-986d-4bb2-8551-6d1f8066e36b", "98eb8fd9-1ea0-457b-b539-ba6396eb6e01", 100.0f, 5000, 5000, 5000);
        return Ok(result);
    }
}