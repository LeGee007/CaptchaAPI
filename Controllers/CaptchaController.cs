using CaptchaAPI.Models;
using CaptchaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CaptchaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaptchaController : ControllerBase
{
    private readonly ICaptchaService _captchaService;

    public CaptchaController(ICaptchaService captchaService)
    {
        _captchaService = captchaService;
    }

    [HttpGet("math")]
    public IActionResult GetMathCaptcha()
    {
        var result = _captchaService.GenerateMathCaptcha();
        return Ok(result);
    }

    [HttpGet("image")]
    public IActionResult GetImageCaptcha()
    {
        var result = _captchaService.GenerateImageCaptcha();
        return Ok(result);
    }

    [HttpPost("validate")]
    public IActionResult Validate([FromBody] CaptchaValidateRequest request)
    {
        bool isValid = _captchaService.Validate(request.CaptchaId, request.Answer);

        if (isValid)
            return Ok(new { success = true, message = "To'g'ri!" });

        return BadRequest(new { success = false, message = "Noto'g'ri!" });
    }
}
