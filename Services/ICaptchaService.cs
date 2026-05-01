using CaptchaAPI.Models;

namespace CaptchaAPI.Services;

public interface ICaptchaService
{
    CaptchaResponse GenerateImageCaptcha();
    CaptchaResponse GenerateMathCaptcha();
    bool Validate(string CaptchaId, string Answer);
}
